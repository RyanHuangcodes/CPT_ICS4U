// PauseManager.cs
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PauseManager : MonoBehaviour
{
    public void GoToPaused()
    {
        var player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            Debug.LogError("[PauseManager] No Player!");
            return;
        }

        int gold = GoldManager.Instance.GetGold();
        int score = ScoreManager.Instance?.GetScore() ?? 0;

        var towerData = new List<TowerSaveData>();
        var towers = Object.FindObjectsByType<Tower>(
            FindObjectsInactive.Exclude,
            FindObjectsSortMode.None
        );
        foreach (var tower in towers)
        {
            string type = tower.name.Replace("(Clone)", "").Trim();
            towerData.Add(new TowerSaveData(
                type,
                tower.transform.position,
                tower.GetHealth(),
                tower.GetMaxHealth(),
                tower.GetLevel()
            ));
        }

        var enemyData = new List<EnemySaveData>();
        var enemies = Object.FindObjectsByType<Enemy>(
            FindObjectsInactive.Exclude,
            FindObjectsSortMode.None
        );
        foreach (var enemy in enemies)
        {
            string type = enemy.name.Replace("(Clone)", "").Trim();
            enemyData.Add(new EnemySaveData(
                type,
                enemy.transform.position,
                enemy.GetHealth()
            ));
        }

        var wm = WaveManager.Instance;
        int currentWave        = wm.CurrentWave;
        int spawnedThisWave    = wm.SpawnedInCurrentWave;
        float timeUntilNext    = wm.TimeUntilNextSpawn;
        float healthMul        = wm.CurrentHealthMultiplier;
        float damageMul        = wm.CurrentDamageMultiplier;
        int postBossCycle      = wm.PostBossCycle;
        int knifeTier          = KnifeThrower.Instance.CurrentUpgradeTier;

        var ent = player.GetComponent<Entity>();
        int playerHealth    = ent.GetHealth();
        int playerMaxHealth = ent.GetMaxHealth();

        int baseCount       = BasePlacementTracker.Instance.GetPlacedCount();
        int mineCount       = GoldMinePlacementTracker.Instance.GetPlacedCount();
        int machineGunCount = MachineGunPlacementTracker.Instance.GetPlacedCount();  // ← new
        int dualMGCount     = DualMachineGunPlacementTracker.Instance.GetPlacedCount();
        int cannonCount     = CannonPlacementTracker.Instance.GetPlacedCount();
        int missileCount    = MissileLauncherPlacementTracker.Instance.GetPlacedCount();

        var data = new SaveData(
            player.transform.position,
            gold,
            score,
            playerHealth,
            playerMaxHealth,
            towerData,
            enemyData,
            baseCount,
            mineCount,
            machineGunCount,
            dualMGCount,
            cannonCount,
            missileCount,
            currentWave,
            spawnedThisWave,
            timeUntilNext,
            healthMul,
            damageMul,
            postBossCycle,
            knifeTier
        );

        SaveManager.SaveGame(data);
        SceneManager.LoadScene("PauseScene");
    }
}
