// WaveManager.cs
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Timing")]
    public float InitialDelay     = 120f;
    public float WaveInterval     = 90f;

    [Header("Regular Waves")]
    public float SpawnInterval    = 5f;
    public int   BaseEnemyCount   = 24;
    public int   EnemiesIncrement = 4;

    [Header("Wave Prefabs")]
    public GameObject CommonEnemyPrefab;
    public GameObject BossEnemyPrefab;
    public int        TotalWaves = 100;

    [Header("Spawn Geometry")]
    public float   SpawnDistance = 10f;
    public Transform BaseTransform;

    [Header("Post-Boss Upgrades")]
    public Color[] PostBossColors;
    public float   HealthMultiplierPerCycle = 1.2f;
    public float   DamageMultiplierPerCycle = 1.1f;

    [Header("Events")]
    public UnityEvent<int> OnWaveStarted;
    public UnityEvent<int> OnBossWaveStarted;

    [Header("UI")]
    public TMP_Text AnnouncementText;    // “Wave X in 10 seconds!”
    public TMP_Text UpcomingWaveText;    // “Upcoming wave: X”

    // internal state
    private int   _currentWave;
    private int   _spawnedInCurrentWave;
    private float _timeUntilNextSpawn;
    private float _healthMul = 1f;
    private float _damageMul = 1f;
    private int   _postBossCycle;
    private bool  _started;

    public static WaveManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (AnnouncementText != null)
            AnnouncementText.gameObject.SetActive(false);

        CycleCommonEnemyColor();
    }

    private void Update()
    {
        if (!_started && BaseTransform == null)
        {
            var b = GameObject.FindWithTag("Base");
            if (b != null) BaseTransform = b.transform;
        }
    }

    // Exposed getters for saving
    public int   CurrentWave             => _currentWave;
    public int   SpawnedInCurrentWave    => _spawnedInCurrentWave;
    public float TimeUntilNextSpawn      => _timeUntilNextSpawn;
    public float CurrentHealthMultiplier => _healthMul;
    public float CurrentDamageMultiplier => _damageMul;
    public int   PostBossCycle           => _postBossCycle;

    public void SetWaveState(
        int wave,
        int spawnedInWave,
        float timeUntilNextSpawn,
        float healthMul,
        float damageMul,
        int postBossCycle
    ) {
        _currentWave          = wave;
        _spawnedInCurrentWave = spawnedInWave;
        _timeUntilNextSpawn   = timeUntilNextSpawn;
        _healthMul            = healthMul;
        _damageMul            = damageMul;
        _postBossCycle        = postBossCycle;
    }

    public void StartWaves()
    {
        if (_started || BaseTransform == null) return;
        _started = true;

        // Update UI immediately for loaded wave
        OnWaveStarted?.Invoke(_currentWave);
        UpdateUpcomingWaveUI(_currentWave + 1);

        StartCoroutine(SpawnAllWaves());
    }

    private IEnumerator SpawnAllWaves()
    {
        // Determine initial delay
        float delay = _timeUntilNextSpawn > 0f ? _timeUntilNextSpawn : InitialDelay;

        // Schedule warning & update upcoming
        int upcoming = _currentWave + 1;
        if (upcoming <= TotalWaves)
        {
            StartCoroutine(ShowWaveWarning(upcoming, delay));
            UpdateUpcomingWaveUI(upcoming);
        }

        _timeUntilNextSpawn = delay;
        yield return new WaitForSeconds(_timeUntilNextSpawn);

        while (_currentWave < TotalWaves)
        {
            _currentWave++;
            _spawnedInCurrentWave = 0;

            if (_currentWave % 10 == 0)
            {
                // compute how many cycles have completed (0 for waves 1-10, 1 for 11-20, etc.)
                int cycleIdx = Mathf.Max(0, (_currentWave - 1) / 10);
                float bossHealthMul = Mathf.Pow(HealthMultiplierPerCycle, cycleIdx);
                float bossDamageMul = Mathf.Pow(DamageMultiplierPerCycle, cycleIdx);

                SpawnBossWave(_currentWave, bossHealthMul, bossDamageMul);
                OnBossWaveStarted?.Invoke(_currentWave);

                // advance the color cycle for common enemies
                _postBossCycle++;
                _healthMul *= HealthMultiplierPerCycle;
                _damageMul *= DamageMultiplierPerCycle;
                CycleCommonEnemyColor();
            }
            else
            {
                yield return StartCoroutine(
                    SpawnRegularWave(_currentWave, _spawnedInCurrentWave)
                );
                OnWaveStarted?.Invoke(_currentWave);
            }

            // Schedule next warning & update upcoming
            upcoming = _currentWave + 1;
            if (upcoming <= TotalWaves)
            {
                StartCoroutine(ShowWaveWarning(upcoming, WaveInterval));
                UpdateUpcomingWaveUI(upcoming);
            }

            _timeUntilNextSpawn = WaveInterval;
            yield return new WaitForSeconds(_timeUntilNextSpawn);
        }
    }

    private IEnumerator SpawnRegularWave(int waveNumber, int alreadySpawned)
    {
        int total     = BaseEnemyCount + EnemiesIncrement * (waveNumber - 1);
        int remaining = total - alreadySpawned;

        while (remaining > 0)
        {
            int chunk = Mathf.Min(24, remaining);
            SpawnChunk(waveNumber, chunk);
            _spawnedInCurrentWave += chunk;
            remaining -= chunk;

            _timeUntilNextSpawn = SpawnInterval;
            yield return new WaitForSeconds(_timeUntilNextSpawn);
        }
    }

    private void SpawnChunk(int waveNumber, int count)
    {
        Vector3 origin = BaseTransform.position;
        Vector2[] dirs = {
            Vector2.up, Vector2.right, Vector2.down, Vector2.left,
            new Vector2(1,1).normalized, new Vector2(1,-1).normalized,
            new Vector2(-1,-1).normalized, new Vector2(-1,1).normalized
        };

        Vector3[] pts = new Vector3[8];
        for (int i = 0; i < 8; i++)
            pts[i] = origin + (Vector3)(dirs[i] * SpawnDistance);

        if (count <= 8)
        {
            int[] idx = (waveNumber % 2 == 0)
                ? new int[]{4,5,6,7,0,1,2,3}
                : new int[]{0,1,2,3,4,5,6,7};

            for (int i = 0; i < count; i++)
                SpawnEnemyAt(pts[idx[i]]);
        }
        else
        {
            int perDir   = count / 8;
            int leftover = count % 8;
            for (int i = 0; i < 8; i++)
            {
                int c = perDir + (i < leftover ? 1 : 0);
                for (int j = 0; j < c; j++)
                    SpawnEnemyAt(pts[i]);
            }
        }
    }

    private void SpawnEnemyAt(Vector3 pos)
    {
        var go = Instantiate(CommonEnemyPrefab, pos, Quaternion.identity);
        var e  = go.GetComponent<Entity>();
        e.SetHealth(Mathf.RoundToInt(e.GetMaxHealth() * _healthMul));
        e.SetDamage(Mathf.RoundToInt(e.GetDamage()     * _damageMul));
    }

    private void SpawnBossWave(int waveNumber, float healthMul, float damageMul)
    {
        Vector3 pos = BaseTransform.position + Vector3.up * SpawnDistance;
        var boss = Instantiate(BossEnemyPrefab, pos, Quaternion.identity);
        var e    = boss.GetComponent<Entity>();
        e.SetHealth(Mathf.RoundToInt(e.GetMaxHealth() * 2f * healthMul));
        e.SetDamage(Mathf.RoundToInt(e.GetDamage()     * 1.5f * damageMul));
    }

    private void CycleCommonEnemyColor()
    {
        if (PostBossColors.Length == 0) return;
        int idx = Mathf.Clamp(_postBossCycle, 0, PostBossColors.Length - 1);
        var r = CommonEnemyPrefab.GetComponentInChildren<SpriteRenderer>();
        if (r != null) r.color = PostBossColors[idx];
    }

    private IEnumerator ShowWaveWarning(int waveNumber, float delay)
    {
        yield return new WaitForSeconds(delay - 10f);

        bool isBoss = (waveNumber % 10 == 0);
        AnnouncementText.text = isBoss
            ? $"Boss Wave {waveNumber} in 10 seconds!"
            : $"Wave {waveNumber} in 10 seconds!";

        AnnouncementText.gameObject.SetActive(true);
        yield return new WaitForSeconds(10f);
        AnnouncementText.gameObject.SetActive(false);
    }

    private void UpdateUpcomingWaveUI(int upcoming)
    {
        if (UpcomingWaveText != null)
            UpcomingWaveText.text = $"Upcoming wave: {upcoming}";
    }
}
