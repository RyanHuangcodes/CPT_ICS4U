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

        var data = SaveManager.LoadGame();
        if (data == null)
        {
            return;
        }
        if (ScoreManager.Instance != null)
            ScoreManager.Instance.SetScore(data.Score);

        // Restore Player
        var player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            player.transform.position = data.PlayerPosition;
            var ent = player.GetComponent<Entity>();
            ent.SetMaxHealth(data.PlayerMaxHealth);
            ent.SetHealth(data.PlayerHealth);
            var p = player.GetComponent<Player>();
            if (p != null) p.UpdateHealthText();
        }

        // Restore Gold
        GoldManager.Instance.SetGold(data.Gold);

        // Restore placement counts
        BasePlacementTracker.Instance.SetPlacedCount(data.BasePlaced);
        GoldMinePlacementTracker.Instance.SetPlacedCount(data.GoldMinePlaced);
        MachineGunPlacementTracker.Instance.SetPlacedCount(data.MachineGunPlaced);  
        DualMachineGunPlacementTracker.Instance.SetPlacedCount(data.DualMachineGunPlaced);
        CannonPlacementTracker.Instance.SetPlacedCount(data.CannonPlaced);
        MissileLauncherPlacementTracker.Instance.SetPlacedCount(data.MissileLauncherPlaced);

        // Prefab lookup
        _prefabMap = new Dictionary<string, GameObject>
        {
            { BasePrefab.name,            BasePrefab },
            { GoldMinePrefab.name,        GoldMinePrefab },
            { MachineGunPrefab.name,      MachineGunPrefab },
            { DualMachineGunPrefab.name,  DualMachineGunPrefab },
            { CannonPrefab.name,          CannonPrefab },
            { MissileLauncherPrefab.name, MissileLauncherPrefab },
            { CommonEnemyPrefab.name,     CommonEnemyPrefab }
        };
        if (BossEnemyPrefab != null)
            _prefabMap[BossEnemyPrefab.name] = BossEnemyPrefab;

        // Restore Towers
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

        // Restore Wave State
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

        // Re-apply wave color entirely
        wm.ApplyWaveColor();

        // Restore Enemies
        foreach (var e in data.Enemies)
        {
            if (_prefabMap.TryGetValue(e.Type, out var pf))
            {
                var go = Instantiate(pf, e.Position, Quaternion.identity);
                var entEnemy = go.GetComponent<Entity>();
                entEnemy.SetHealth(e.Health);
            }
        }

        // Knife tier + shop UI
        KnifeThrower.Instance?.SetUpgradeTier(data.KnifeTier);
        ShopManager.Instance?.RefreshUI();

        // Finally start waves again
        wm.StartWaves();
    }
}
