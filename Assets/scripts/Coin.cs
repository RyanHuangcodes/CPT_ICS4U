using UnityEngine;

public class HealthCoin : MonoBehaviour
{
    public int BonusMaxHealth = 100;

    private void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<Player>();
            if (player != null)
            {
                int newMax = player.GetMaxHealth() + BonusMaxHealth;
                player.SetMaxHealth(newMax);
                player.SetHealth(newMax);
                player.UpdateHealthText();
            }
            Destroy(gameObject);
        }
    }
}
