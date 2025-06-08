using UnityEngine;
using UnityEngine.SceneManagement;

public class Base : Tower
{
    protected override void Start()
    {
        
        SetHealth(300);
        SetDamage(0);
        SetSpeed(0f);
        SetInitialized(true);
    }

    //game over
    protected override void Die()
    {
        Debug.Log("GAME OVER, score: tbd");
        SceneManager.LoadScene("LoseScene");
    }
}
