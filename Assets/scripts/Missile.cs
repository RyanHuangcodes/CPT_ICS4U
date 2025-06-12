using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Missile : MonoBehaviour
{
    [Header("Movement")]
    public float Speed = 8f;

    [Header("Damage")]
    public LayerMask EnemyLayer;
    public int Damage = 25;
    public float ExplosionRadius = 2f;
    public float KnockbackForce = 1f;

    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.isKinematic = false;
        GetComponent<Collider2D>().isTrigger = true;
    }

    /// <summary>
    /// Call immediately after Instantiate().
    /// </summary>
    public void Setup(Vector2 direction)
    {
        _rb.linearVelocity = direction.normalized * Speed;
        Destroy(gameObject, 5f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & EnemyLayer) == 0)
        {
            return;
        }

        Vector2 center = transform.position;
        Collider2D[] hits = Physics2D.OverlapCircleAll(center, ExplosionRadius, EnemyLayer);
        foreach (var hit in hits)
        {
            Enemy e = hit.GetComponent<Enemy>();
            if (e != null)
            {
                e.TakeDamage(Damage);
                Vector2 knockDir = (e.transform.position - center).normalized;
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