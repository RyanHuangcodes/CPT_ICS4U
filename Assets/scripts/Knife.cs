using UnityEngine;
//gpt
public class Knife : MonoBehaviour
{
    public int damage;
    private bool hasHit = false;

    void Start()
    {
        Invoke(nameof(SelfDestruct), 2f);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasHit) return;

        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Debug.Log("Hit for " + damage + ", enemy has " + enemy.GetHealth() + " HP left.");
        }

        hasHit = true;
        Destroy(gameObject); // Destroy immediately on hit
    }

    void SelfDestruct()
    {
        if (!hasHit)
        {
            Destroy(gameObject); // Only destroy if it hasn't hit anything yet
        }
    }
}
