// PauseManager.cs
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PauseManager : MonoBehaviour
{
    public void GoToPaused()
    {
        // 1) Player
        var player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            Debug.LogError("[PauseManager] No Player!");
            return;
        }

        // 2) Gold
        int gold = GoldManager.Instance.GetGold();

        // 3) Towers — use the new API
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

        // 4) Enemies — also use the new API
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

        // 5) Wave State
        var wm = WaveManager.Instance;
        int   currentWave        = wm.CurrentWave;
        int   spawnedThisWave    = wm.SpawnedInCurrentWave;
        float timeUntilNextSpawn = wm.TimeUntilNextSpawn;
        float healthMul          = wm.CurrentHealthMultiplier;
        float damageMul          = wm.CurrentDamageMultiplier;
        int   postBossCycle      = wm.PostBossCycle;

        // 6) Knife tier
        int knifeTier = KnifeThrower.Instance.CurrentUpgradeTier;

        // 7) Build SaveData (including dual‐MG count)
        var data = new SaveData(
            player.transform.position,
            gold,
            towerData,
            enemyData,
            BasePlacementTracker.Instance.GetPlacedCount(),
            GoldMinePlacementTracker.Instance.GetPlacedCount(),
            CannonPlacementTracker.Instance.GetPlacedCount(),
            DualMachineGunPlacementTracker.Instance.GetPlacedCount(),
            currentWave,
            spawnedThisWave,
            timeUntilNextSpawn,
            healthMul,
            damageMul,
            postBossCycle,
            knifeTier
        );

        // 8) Save and switch to pause scene
        SaveManager.SaveGame(data);
        SceneManager.LoadScene("PauseScene");
    }
}
