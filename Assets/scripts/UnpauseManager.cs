using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
//gpt
public class UnpauseManager : MonoBehaviour
{
    public GameObject BasePrefab;
    public GameObject GoldMinePrefab;

    private Dictionary<string, GameObject> _prefabMap;

    public void ResumeGame()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene("GameScene");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        SaveData data = SaveManager.LoadGame();
        if (data == null) return;

        // Restore player position
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
            player.transform.position = data.PlayerPosition;

        // Restore gold
        GoldManager.Instance.SetGold(data.Gold);

        // Restore placement counts
        BasePlacementTracker.Instance.SetPlacedCount(data.BasePlaced);
        GoldMinePlacementTracker.Instance.SetPlacedCount(data.GoldMinePlaced);

        // Manually mapped prefab references
        _prefabMap = new Dictionary<string, GameObject>
        {
            { "Base", BasePrefab },
            { "GoldMine", GoldMinePrefab }
        };

        // Restore towers
        foreach (TowerSaveData towerData in data.Towers)
        {
            if (_prefabMap.TryGetValue(towerData.Type, out GameObject prefab))
            {
                GameObject tower = Instantiate(prefab, towerData.Position, Quaternion.identity);
                Tower towerComp = tower.GetComponent<Tower>();
                towerComp.SetHealth(towerData.Health);
                towerComp.SetLevel(towerData.Level);
                towerComp.SetInitialized(true);
            }
            else
            {
                Debug.LogWarning("Missing prefab in UnpauseManager for tower type: " + towerData.Type);
            }
        }
    }
}
