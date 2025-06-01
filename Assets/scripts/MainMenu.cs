using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour
{
    public List<Coin> coins = new List<Coin>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OpenPreferences()
    {
        SceneManager.LoadScene("PreferencesScene");
    }
    public void QuitGame()
    {
        DataWrapper wrapper = new DataWrapper(coins);

        string json = JsonUtility.ToJson(wrapper, true);

        string savePath = Path.Combine(Application.persistentDataPath, "GameProgress.json");

        File.WriteAllText(savePath, json);

        Application.Quit();
    }
    public class DataWrapper
    {
        public List<Coin> coins;

        public DataWrapper(List<Coin> coins)
        {
            this.coins = coins;
        }
    }

}
