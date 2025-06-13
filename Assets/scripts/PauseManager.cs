// PauseManager.cs
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PauseManager : MonoBehaviour
{
    public void GoToPaused()
    {
        // 1) Collect all game state into SaveData
        var player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            Debug.LogError("[PauseManager] No Player!");
            return;
        }

        int gold    = GoldManager.Instance.GetGold();
        int score   = ScoreManager.Instance?.GetScore() ?? 0;
        var ent     = player.GetComponent<Entity>();
        int pHealth = ent.GetHealth();
        int pMax    = ent.GetMaxHealth();

        var towerData = new List<TowerSaveData>();
        var towers = Object.FindObjectsByType<Tower>(
            FindObjectsInactive.Exclude,
            FindObjectsSortMode.None
        );
        foreach (var tw in towers)
        {
            string type = tw.name.Replace("(Clone)", "").Trim();
            towerData.Add(new TowerSaveData(
                type,
                tw.transform.position,
                tw.GetHealth(),
                tw.GetMaxHealth(),
                tw.GetLevel()
            ));
        }

        var enemyData = new List<EnemySaveData>();
        var enemies = Object.FindObjectsByType<Enemy>(
            FindObjectsInactive.Exclude,
            FindObjectsSortMode.None
        );
        foreach (var en in enemies)
        {
            string type = en.name.Replace("(Clone)", "").Trim();
            enemyData.Add(new EnemySaveData(
                type,
                en.transform.position,
                en.GetHealth()
            ));
        }

        var wm = WaveManager.Instance;
        var data = new SaveData(
            player.transform.position,
            gold,
            score,
            pHealth,
            pMax,
            towerData,
            enemyData,
            BasePlacementTracker.Instance.GetPlacedCount(),
            GoldMinePlacementTracker.Instance.GetPlacedCount(),
            MachineGunPlacementTracker.Instance.GetPlacedCount(),
            DualMachineGunPlacementTracker.Instance.GetPlacedCount(),
            CannonPlacementTracker.Instance.GetPlacedCount(),
            PiercingCannonPlacementTracker.Instance.GetPlacedCount(),
            MissileLauncherPlacementTracker.Instance.GetPlacedCount(),
            wm.CurrentWave,
            wm.SpawnedInCurrentWave,
            wm.TimeUntilNextSpawn,
            wm.CurrentHealthMultiplier,
            wm.CurrentDamageMultiplier,
            wm.PostBossCycle,
            KnifeThrower.Instance.CurrentUpgradeTier
        );

        // 2) Quick‐save to temp file
        SaveManager.SaveQuick(data);

        // 3) Go to pause scene
        SceneManager.LoadScene("PauseScene");
    }
}
