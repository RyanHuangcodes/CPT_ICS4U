using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PauseManager : MonoBehaviour
{
    /// <summary>
    /// Call this when the player hits “Pause.” It gathers
    /// player, gold, towers, enemies, wave state—and writes
    /// it all out via SaveManager.
    /// </summary>
    public void GoToPaused()
    {
        // 1) Player
        var player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            Debug.LogError("[PauseManager] No Player found!");
            return;
        }

        // 2) Gold
        int gold = GoldManager.Instance.GetGold();

        // 3) Towers
        var towerData = new List<TowerSaveData>();
        var towers = Object.FindObjectsByType<Tower>(
            FindObjectsInactive.Exclude,
            FindObjectsSortMode.None
        );
        Debug.Log($"[PauseManager] Found {towers.Length} towers");
        foreach (var tower in towers)
        {
            string type = tower.name.Replace("(Clone)", "").Trim();
            towerData.Add(new TowerSaveData(
                type,
                tower.transform.position,
                tower.GetHealth(),
                tower.GetLevel()
            ));
        }

        // 4) Enemies
        var enemyData = new List<EnemySaveData>();
        var enemies = Object.FindObjectsByType<Enemy>(
            FindObjectsInactive.Exclude,
            FindObjectsSortMode.None
        );
        Debug.Log($"[PauseManager] Found {enemies.Length} enemies");
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

        // 6) Build SaveData
        var data = new SaveData(
            player.transform.position,
            gold,
            towerData,
            enemyData,
            BasePlacementTracker.Instance.GetPlacedCount(),
            GoldMinePlacementTracker.Instance.GetPlacedCount(),
            currentWave,
            spawnedThisWave,
            timeUntilNextSpawn,
            healthMul,
            damageMul,
            postBossCycle
        );

        // 7) Write & switch
        SaveManager.SaveGame(data);
        SceneManager.LoadScene("PauseScene");
    }
}
