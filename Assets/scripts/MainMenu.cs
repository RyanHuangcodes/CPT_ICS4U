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
        //prevents gold from persisting between scenes
        Destroy(GoldManager.Instance.gameObject); 
        SaveManager.DeleteSave(); 
    }

    public void PauseScreen()
    {
        SceneManager.LoadSceneAsync("PauseScene");
    }

    public void QuitGame()
    {
        SaveManager.DeleteSave(); 
        Application.Quit();
    }
}
