using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class UnpauseManager : MonoBehaviour
{
    [Header("Tower Prefabs")]
    public GameObject BasePrefab;
    public GameObject GoldMinePrefab;
    public GameObject MachineGunPrefab;

    [Header("Enemy Prefabs")]
    public GameObject CommonEnemyPrefab;
    public GameObject BossEnemyPrefab;  // optional

    private Dictionary<string, GameObject> _prefabMap;

    public void ResumeGame()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene("GameScene");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        var data = SaveManager.LoadGame();
        if (data == null)
        {
            Debug.LogWarning("[UnpauseManager] No save data found.");
            return;
        }

        // Restore player & gold
        var player = GameObject.FindWithTag("Player");
        if (player != null) player.transform.position = data.PlayerPosition;
        GoldManager.Instance.SetGold(data.Gold);

        // Restore placement trackers
        BasePlacementTracker.Instance.SetPlacedCount(data.BasePlaced);
        GoldMinePlacementTracker.Instance.SetPlacedCount(data.GoldMinePlaced);

        // Build prefab map using only assigned prefabs
        _prefabMap = new Dictionary<string, GameObject>
        {
            { BasePrefab.name,       BasePrefab       },
            { GoldMinePrefab.name,   GoldMinePrefab   },
            { MachineGunPrefab.name, MachineGunPrefab },

            { CommonEnemyPrefab.name, CommonEnemyPrefab }
        };

        if (BossEnemyPrefab != null)
            _prefabMap[BossEnemyPrefab.name] = BossEnemyPrefab;

        // Restore towers
        Debug.Log($"[UnpauseManager] Restoring {data.Towers.Count} towers");
        foreach (var t in data.Towers)
        {
            Debug.Log($"[UnpauseManager] Tower type='{t.Type}' pos={t.Position} hp={t.Health} lvl={t.Level}");
            if (_prefabMap.TryGetValue(t.Type, out var towerPf))
            {
                var go = Instantiate(towerPf, t.Position, Quaternion.identity);
                var comp = go.GetComponent<Tower>();
                comp.SetLevel(t.Level);
                comp.SetHealth(t.Health);
                comp.SetInitialized(true);
            }
            else
            {
                Debug.LogWarning($"[UnpauseManager] No prefab found for tower type '{t.Type}'");
            }
        }

        // Restore enemies
        Debug.Log($"[UnpauseManager] Restoring {data.Enemies.Count} enemies");
        foreach (var e in data.Enemies)
        {
            Debug.Log($"[UnpauseManager] Enemy type='{e.Type}' pos={e.Position} hp={e.Health}");
            if (_prefabMap.TryGetValue(e.Type, out var enemyPf))
            {
                var go = Instantiate(enemyPf, e.Position, Quaternion.identity);
                var comp = go.GetComponent<Entity>();
                comp.SetHealth(e.Health);
            }
            else
            {
                Debug.LogWarning($"[UnpauseManager] No prefab found for enemy type '{e.Type}', skipping.");
            }
        }

        // Restore wave state & start
        var wm = WaveManager.Instance;
        wm.BaseTransform = GameObject.FindWithTag("Base").transform;
        wm.SetWaveState(
            data.CurrentWave,
            data.SpawnedInCurrentWave,
            data.TimeUntilNextSpawn,
            data.HealthMultiplier,
            data.DamageMultiplier,
            data.PostBossCycle
        );
        wm.StartWaves();
    }
}
