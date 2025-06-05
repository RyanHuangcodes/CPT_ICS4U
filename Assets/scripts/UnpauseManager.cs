using UnityEngine;
using UnityEngine.SceneManagement;
//gpt
public class UnpauseManager : MonoBehaviour
{
    public void ResumeGame()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene("GameScene");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GameScene")
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;

            GameObject player = GameObject.FindWithTag("Player");
            Vector2? pos = SaveManager.LoadPlayerPosition();

            if (player != null && pos.HasValue)
            {
                player.transform.position = pos.Value;
            }
        }
    }
}
