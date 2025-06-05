using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public void GoToPaused()
    {
        SceneManager.LoadScene("PauseScene");
    }
}
