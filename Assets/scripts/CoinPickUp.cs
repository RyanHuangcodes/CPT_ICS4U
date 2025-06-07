using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    public int value = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (CoinManager.Instance != null)
                CoinManager.Instance.AddCoins(value);

            Destroy(gameObject);
        }
    }
}
