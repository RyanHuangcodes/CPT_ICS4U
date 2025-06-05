using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public void GoToPaused()
    {
        //gpt
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            SaveManager.SavePlayerPosition(player.transform.position);
        }
        //gpt end
        SceneManager.LoadScene("PauseScene");
    }
}
