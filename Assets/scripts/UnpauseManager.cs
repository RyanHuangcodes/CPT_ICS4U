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

        // Build prefab map
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
        foreach (var t in data.Towers)
        {
            if (_prefabMap.TryGetValue(t.Type, out var pf))
            {
                var go = Instantiate(pf, t.Position, Quaternion.identity);
                var comp = go.GetComponent<Tower>();
                comp.SetMaxHealth(t.MaxHealth);    // NEW
                comp.SetHealth(t.Health);
                comp.SetLevel(t.Level);
                comp.SetInitialized(true);
            }
            else
            {
                Debug.LogWarning($"[UnpauseManager] No prefab for tower '{t.Type}'");
            }
        }

        // Restore enemies
        foreach (var e in data.Enemies)
        {
            if (_prefabMap.TryGetValue(e.Type, out var pf))
            {
                var go = Instantiate(pf, e.Position, Quaternion.identity);
                var comp = go.GetComponent<Entity>();
                comp.SetHealth(e.Health);
            }
            else
            {
                Debug.LogWarning($"[UnpauseManager] No prefab for enemy '{e.Type}'");
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

        // Restore knife tier & refresh shop UI
        if (KnifeThrower.Instance != null)
            KnifeThrower.Instance.SetUpgradeTier(data.KnifeTier);
        if (ShopManager.Instance != null)
            ShopManager.Instance.RefreshUI();

        wm.StartWaves();
    }
}
