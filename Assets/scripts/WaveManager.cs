// WaveManager.cs
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Timing")]
    public float InitialDelay  = 120f;
    public float WaveInterval  = 90f;

    [Header("Regular Waves")]
    public float SpawnInterval    = 5f;
    public int   BaseEnemyCount   = 24;
    public int   EnemiesIncrement = 4;

    [Header("Wave Prefabs")]
    public GameObject CommonEnemyPrefab;
    public GameObject BossEnemyPrefab;
    public int        TotalWaves = 40;

    [Header("Spawn Geometry")]
    public float   SpawnDistance = 10f;
    public Transform BaseTransform;

    [Header("Wave Colors")]
    [Tooltip("0=waves 1–9, 1=10–19, 2=20–29, 3=30–39")]
    public Color[] WaveColors;

    [Header("Post-Boss Upgrades")]
    public float HealthMultiplierPerCycle = 1.2f;
    public float DamageMultiplierPerCycle = 1.1f;

    [Header("Events")]
    public UnityEvent<int> OnWaveStarted;
    public UnityEvent<int> OnBossWaveStarted;

    [Header("UI")]
    public TMP_Text AnnouncementText;
    public TMP_Text UpcomingWaveText;

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

        ApplyWaveColor();
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
    )
    {
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

        OnWaveStarted?.Invoke(_currentWave);
        UpdateUpcomingWaveUI(_currentWave + 1);
        StartCoroutine(SpawnAllWaves());
    }

    private IEnumerator SpawnAllWaves()
    {
        float delay   = _timeUntilNextSpawn > 0f ? _timeUntilNextSpawn : InitialDelay;
        int   upcoming = _currentWave + 1;
        if (upcoming <= TotalWaves)
        {
            StartCoroutine(ShowWaveWarning(upcoming, delay));
            UpdateUpcomingWaveUI(upcoming);
        }

        _timeUntilNextSpawn = delay;
        yield return new WaitForSeconds(delay);

        while (_currentWave < TotalWaves)
        {
            _currentWave++;
            _spawnedInCurrentWave = 0;

            ApplyWaveColor();

            if (_currentWave % 10 == 0)
            {
                SpawnBossWave(_currentWave, _healthMul, _damageMul);
                OnBossWaveStarted?.Invoke(_currentWave);

                _postBossCycle++;
                _healthMul *= HealthMultiplierPerCycle;
                _damageMul *= DamageMultiplierPerCycle;
            }
            else
            {
                yield return StartCoroutine(SpawnRegularWave());
                OnWaveStarted?.Invoke(_currentWave);
            }

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

    private IEnumerator SpawnRegularWave()
    {
        int total     = BaseEnemyCount + EnemiesIncrement * (_currentWave - 1);
        int remaining = total - _spawnedInCurrentWave;

        while (remaining > 0)
        {
            int chunk = Mathf.Min(24, remaining);
            SpawnChunk(chunk);
            _spawnedInCurrentWave += chunk;
            remaining -= chunk;

            _timeUntilNextSpawn = SpawnInterval;
            yield return new WaitForSeconds(_timeUntilNextSpawn);
        }
    }

    private void SpawnChunk(int count)
    {
        Vector3 origin = BaseTransform.position;
        Vector2[] dirs = {
            Vector2.up, Vector2.right, Vector2.down, Vector2.left,
            new Vector2(1,1).normalized, new Vector2(1,-1).normalized,
            new Vector2(-1,-1).normalized, new Vector2(-1,1).normalized
        };

        for (int i = 0; i < count; i++)
        {
            Vector3 pos = origin + (Vector3)dirs[i % dirs.Length] * SpawnDistance;
            SpawnEnemyAt(pos);
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

    public void ApplyWaveColor()
    {
        if (WaveColors == null || WaveColors.Length == 0) return;
        int idx = Mathf.Clamp(_currentWave / 10, 0, WaveColors.Length - 1);
        var sr = CommonEnemyPrefab.GetComponentInChildren<SpriteRenderer>();
        if (sr != null) sr.color = WaveColors[idx];
    }

    private IEnumerator ShowWaveWarning(int waveNumber, float delay)
    {
        yield return new WaitForSeconds(delay - 10f);

        bool isBoss = (waveNumber % 10 == 0);
        if (AnnouncementText != null)
        {
            AnnouncementText.text = isBoss
                ? $"Boss Wave {waveNumber} in 10 seconds!"
                : $"Wave {waveNumber} in 10 seconds!";
            AnnouncementText.gameObject.SetActive(true);
            yield return new WaitForSeconds(10f);
            AnnouncementText.gameObject.SetActive(false);
        }
    }

    private void UpdateUpcomingWaveUI(int upcoming)
    {
        if (UpcomingWaveText != null)
            UpcomingWaveText.text = $"Upcoming wave: {upcoming}";
    }
}
