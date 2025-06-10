using UnityEngine;
//gpt
[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Knife : MonoBehaviour
{
    public int Damage;
    private bool _hasHit = false;

    private void Awake()
    {
        var rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;

        var col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private void Start()
    {
        Invoke(nameof(SelfDestruct), 2f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_hasHit) return;

        var enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(Damage);
            Debug.Log($"Hit for {Damage}, enemy has {enemy.GetHealth()} HP left.");
        }

        _hasHit = true;
        Destroy(gameObject);
    }

    private void SelfDestruct()
    {
        if (!_hasHit)
            Destroy(gameObject);
    }
}
