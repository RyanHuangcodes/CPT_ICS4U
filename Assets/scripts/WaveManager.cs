using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using TMPro;            // for TextMeshPro support

public class WaveManager : MonoBehaviour
{
    [Header("Wave Timing")]
    public float InitialDelay = 120f;
    public float WaveInterval = 90f;

    [Header("Regular Waves")]
    public float SpawnInterval = 5f;
    public int   BaseEnemyCount = 24;
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
    public TMP_Text AnnouncementText;    // assign your centered TextMeshProUGUI here

    // internal state
    private int   _currentWave;
    private int   _spawnedInCurrentWave;
    private float _timeUntilNextSpawn;
    private float _healthMul = 1f;
    private float _damageMul = 1f;
    private int   _postBossCycle;
    private bool  _started;

    // singleton
    public static WaveManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // hide announcement initially
        if (AnnouncementText != null)
            AnnouncementText.gameObject.SetActive(false);

        // set the first common-enemy color (PostBossColors[0]) 
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

    /// <summary>
    /// Call on unpause to re-seed all wave state.
    /// </summary>
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
        StartCoroutine(SpawnAllWaves());
    }

    private IEnumerator SpawnAllWaves()
    {
        // schedule first warning
        StartCoroutine(ShowWaveWarning(1, InitialDelay));

        // initial delay
        _timeUntilNextSpawn = InitialDelay;
        yield return new WaitForSeconds(_timeUntilNextSpawn);

        while (_currentWave < TotalWaves)
        {
            _currentWave++;
            _spawnedInCurrentWave = 0;

            if (_currentWave % 10 == 0)
            {
                SpawnBossWave(_currentWave);
                OnBossWaveStarted?.Invoke(_currentWave);

                // after boss, bump cycle and recolor for the next batch
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

            // schedule next warning if there is one
            int nextWave = _currentWave + 1;
            if (nextWave <= TotalWaves)
                StartCoroutine(ShowWaveWarning(nextWave, WaveInterval));

            // wait interval
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

    private void SpawnBossWave(int waveNumber)
    {
        Vector3 pos = BaseTransform.position + Vector3.up * SpawnDistance;
        var boss = Instantiate(BossEnemyPrefab, pos, Quaternion.identity);
        var e    = boss.GetComponent<Entity>();
        e.SetHealth(Mathf.RoundToInt(e.GetMaxHealth() * _healthMul * 2f));
        e.SetDamage(Mathf.RoundToInt(e.GetDamage()     * _damageMul * 1.5f));
    }

    /// <summary>
    /// Applies PostBossColors[_postBossCycle] to the CommonEnemyPrefab.
    /// Called once at Awake(), and again after each boss wave.
    /// </summary>
    private void CycleCommonEnemyColor()
    {
        if (PostBossColors.Length == 0) return;
        int idx = Mathf.Clamp(_postBossCycle, 0, PostBossColors.Length - 1);
        var r = CommonEnemyPrefab.GetComponentInChildren<SpriteRenderer>();
        if (r != null) r.color = PostBossColors[idx];
    }

    /// <summary>
    /// Shows “Wave X in 10 seconds!” (or “Boss Wave X…”) centered for the final 10s before spawn.
    /// </summary>
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
}
