using UnityEngine;
//gpt code
public class PauseManager : MonoBehaviour
{
    private bool isPaused = false;

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f; // Pause
        }
        else
        {
            Time.timeScale = 1f; // Resume
        }
    }
}
