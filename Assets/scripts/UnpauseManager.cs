// UnpauseManager.cs
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class UnpauseManager : MonoBehaviour
{
    [Header("Tower Prefabs")]
    public GameObject BasePrefab;
    public GameObject GoldMinePrefab;
    public GameObject MachineGunPrefab;
    public GameObject DualMachineGunPrefab;
    public GameObject CannonPrefab;
    public GameObject PiercingCannonPrefab;
    public GameObject MissileLauncherPrefab;

    [Header("Enemy Prefabs")]
    public GameObject CommonEnemyPrefab;
    public GameObject BossEnemyPrefab;

    private Dictionary<string, GameObject> _prefabMap;

    public void ResumeGame()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene("GameScene");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        // 1) Load quick‐save
        var data = SaveManager.LoadQuick();
        if (data == null)
        {
            Debug.LogWarning("[UnpauseManager] No save data found.");
            return;
        }

        // 2) Restore player
        var player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            player.transform.position = data.PlayerPosition;
            var ent = player.GetComponent<Entity>();
            ent.SetMaxHealth(data.PlayerMaxHealth);
            ent.SetHealth(data.PlayerHealth);
            var p = player.GetComponent<Player>();
            if (p != null)
                p.UpdateHealthText();
        }

        // 3) Restore gold & score
        GoldManager.Instance.SetGold(data.Gold);
        if (ScoreManager.Instance != null)
            ScoreManager.Instance.SetScore(data.Score);

        // 4) Restore placement trackers
        BasePlacementTracker.Instance.SetPlacedCount(data.BasePlaced);
        GoldMinePlacementTracker.Instance.SetPlacedCount(data.GoldMinePlaced);
        MachineGunPlacementTracker.Instance.SetPlacedCount(data.MachineGunPlaced);
        DualMachineGunPlacementTracker.Instance.SetPlacedCount(data.DualMachineGunPlaced);
        CannonPlacementTracker.Instance.SetPlacedCount(data.CannonPlaced);
        PiercingCannonPlacementTracker.Instance.SetPlacedCount(data.PiercingCannonPlaced);
        MissileLauncherPlacementTracker.Instance.SetPlacedCount(data.MissileLauncherPlaced);

        // 5) Build prefab lookup
        _prefabMap = new Dictionary<string, GameObject>
        {
            { BasePrefab.name,            BasePrefab },
            { GoldMinePrefab.name,        GoldMinePrefab },
            { MachineGunPrefab.name,      MachineGunPrefab },
            { DualMachineGunPrefab.name,  DualMachineGunPrefab },
            { CannonPrefab.name,          CannonPrefab },
            { PiercingCannonPrefab.name,  PiercingCannonPrefab },
            { MissileLauncherPrefab.name, MissileLauncherPrefab },
            { CommonEnemyPrefab.name,     CommonEnemyPrefab }
        };
        if (BossEnemyPrefab != null)
            _prefabMap[BossEnemyPrefab.name] = BossEnemyPrefab;

        // 6) Instantiate towers
        foreach (var t in data.Towers)
        {
            if (_prefabMap.TryGetValue(t.Type, out var pf))
            {
                var go = Instantiate(pf, t.Position, Quaternion.identity);
                var tw = go.GetComponent<Tower>();
                tw.SetMaxHealth(t.MaxHealth);
                tw.SetHealth(t.Health);
                tw.SetLevel(t.Level);
                tw.SetInitialized(true);
            }
        }

        // 7) Restore wave state
        var wm = WaveManager.Instance;
        wm.BaseTransform = GameObject.FindWithTag("Base")?.transform;
        wm.SetWaveState(
            data.CurrentWave,
            data.SpawnedInCurrentWave,
            data.TimeUntilNextSpawn,
            data.HealthMultiplier,
            data.DamageMultiplier,
            data.PostBossCycle
        );
        wm.ApplyWaveColor();

        // 8) Instantiate enemies
        foreach (var e in data.Enemies)
        {
            if (_prefabMap.TryGetValue(e.Type, out var pf))
            {
                var go = Instantiate(pf, e.Position, Quaternion.identity);
                var entEnemy = go.GetComponent<Entity>();
                entEnemy.SetHealth(e.Health);
            }
        }

        // 9) Knife tier & shop UI
        KnifeThrower.Instance?.SetUpgradeTier(data.KnifeTier);
        ShopManager.Instance?.RefreshUI();

        // 10) Start waves
        wm.StartWaves();
    }
}
