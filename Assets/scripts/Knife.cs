using UnityEngine;
//gpt
public class Knife : MonoBehaviour
{
    public int Damage;
    private bool _hasHit = false;

    void Start()
    {
        Invoke(nameof(SelfDestruct), 2f);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (_hasHit) return;

        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(Damage);
            Debug.Log("Hit for " + Damage + ", enemy has " + enemy.GetHealth() + " HP left.");
        }

        _hasHit = true;
        Destroy(gameObject); // Destroy immediately on hit
    }

    void SelfDestruct()
    {
        if (!_hasHit)
        {
            Destroy(gameObject); // Only destroy if it hasn't hit anything yet
        }
    }
}
