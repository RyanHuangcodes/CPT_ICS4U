using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
//gpt
public class PauseManager : MonoBehaviour
{
    public void GoToPaused()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null) return;

        int gold = GoldManager.Instance.GetGold();

        List<TowerSaveData> towerData = new List<TowerSaveData>();
        foreach (Tower tower in GameObject.FindObjectsByType<Tower>(FindObjectsSortMode.None))
        {
            towerData.Add(new TowerSaveData(
                tower.name.Replace("(Clone)", "").Trim(),
                tower.transform.position,
                tower.GetHealth(),
                tower.GetLevel()
            ));
        }

        SaveData data = new SaveData(
            player.transform.position,
            gold,
            towerData,
            BasePlacementTracker.Instance.GetPlacedCount(),
            GoldMinePlacementTracker.Instance.GetPlacedCount()
        );

        SaveManager.SaveGame(data);
        SceneManager.LoadScene("PauseScene");
    }
}
