using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitions : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("GameScene");
    }

}
