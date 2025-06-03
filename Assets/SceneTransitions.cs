using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitions : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("GameScene");
    }

    public void PreferenceSceneTransition()
    {
        SceneManager.LoadSceneAsync("PreferenceScene");
    }

    public void ShopSceneTransition()
    {
        SceneManager.LoadSceneAsync("ShopScene");
    }

    public void ScoreSceneTransition()
    {
        SceneManager.LoadSceneAsync("ScoresScene");
    }

}
