using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour
{
    void Start()
    {

    }
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("GameScene");
    }

    public void OpenPreferences()
    {
        SceneManager.LoadSceneAsync("PreferencesScene");
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadSceneAsync("MainMenu");
        DeleteTempData();
    }

    public void PauseScreen()
    {
        SceneManager.LoadSceneAsync("PauseScene");
    }



    public void QuitGame()
    {
        DeleteTempData();

        // DataWrapper wrapper = new DataWrapper(Coins);

        // string json = JsonUtility.ToJson(wrapper, true);

        // string savePath = Path.Combine(Application.persistentDataPath, "GameProgress.json");

        // File.WriteAllText(savePath, json);

        // Application.Quit();

    }
    // public class DataWrapper
    // {
    //     public List<Coin> coins;

    //     public DataWrapper(List<Coin> coins)
    //     {
    //         this.coins = coins;
    //     }
    // }
    //gpt
    private static void DeleteTempData()
    {
        string path = Path.Combine(Application.persistentDataPath, "player.json");
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Player save deleted.");
        }
    }
    //end gpt
}
