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

        // delete your temp save
        SaveManager.DeleteQuick();

        // also zero out all placement trackers
        if (BasePlacementTracker.Instance != null)          BasePlacementTracker.Instance.SetPlacedCount(0);
        if (GoldMinePlacementTracker.Instance != null)      GoldMinePlacementTracker.Instance.SetPlacedCount(0);
        if (MachineGunPlacementTracker.Instance != null)    MachineGunPlacementTracker.Instance.SetPlacedCount(0);
        if (DualMachineGunPlacementTracker.Instance != null) DualMachineGunPlacementTracker.Instance.SetPlacedCount(0);
        if (CannonPlacementTracker.Instance != null)        CannonPlacementTracker.Instance.SetPlacedCount(0);
        if (PiercingCannonPlacementTracker.Instance != null) PiercingCannonPlacementTracker.Instance.SetPlacedCount(0);
        if (MissileLauncherPlacementTracker.Instance != null) MissileLauncherPlacementTracker.Instance.SetPlacedCount(0);
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
