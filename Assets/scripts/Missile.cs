using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Missile : MonoBehaviour
{
    public float Speed = 8f;
    public LayerMask EnemyLayer;
    public int       Damage          = 25;
    public float     ExplosionRadius = 2f;
    public float     KnockbackForce  = 1f;

    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.bodyType = RigidbodyType2D.Dynamic;
        GetComponent<Collider2D>().isTrigger = true;
    }

    public void Setup(Vector2 direction, int damage, float explosionRadius, float knockbackForce)
    {
        Damage          = damage;
        ExplosionRadius = explosionRadius;
        KnockbackForce  = knockbackForce;
        _rb.linearVelocity    = direction.normalized * Speed;    
        Destroy(gameObject, 2.5f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((EnemyLayer.value & (1 << other.gameObject.layer)) == 0)
            return;

        Vector2 center = transform.position;
        var hits = Physics2D.OverlapCircleAll(center, ExplosionRadius, EnemyLayer);
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<Enemy>(out var e))
            {
                e.TakeDamage(Damage);
                Vector2 knockDir = ((Vector2)e.transform.position - center).normalized;
                e.ApplyKnockback(knockDir, KnockbackForce);
            }
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, ExplosionRadius);
    }
}
