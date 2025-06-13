using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public UnpauseManager unpauseManager;

    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void SaveAndReturnToMenu()
    {
        var data = SaveManager.LoadQuick();
        if (data != null)
        {
            SaveManager.SavePermanent(data);
            SaveManager.DeleteQuick();
        }
        SceneManager.LoadScene("MainMenu");
    }

    public void Continue()
    {
        var data = SaveManager.LoadPermanent();
        if (data != null)
        {
            SaveManager.SaveQuick(data);
        }
        unpauseManager.ResumeGame();
    }

    public void DeletePermanentSave()
    {
        SaveManager.DeletePermanent();
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
        if (GoldManager.Instance != null)
            Destroy(GoldManager.Instance.gameObject);
        SaveManager.DeleteQuick();
    }

    public void OpenPreferences()
    {
        SceneManager.LoadScene("PreferencesScene");
    }

    public void PauseScreen()
    {
        SceneManager.LoadScene("PauseScene");
    }

    public void QuitGame()
    {
        SaveManager.DeleteQuick();
        Application.Quit();
    }
}
