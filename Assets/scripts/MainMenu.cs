using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
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
    // Update is called once per frame
    void Update()
    {
        
    }
}
