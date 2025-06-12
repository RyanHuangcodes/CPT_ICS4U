using UnityEngine;
//gpt
[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(SpriteRenderer))]
public class Cannonball : MonoBehaviour
{
    [Header("Movement")]
    public float Speed = 10f;

    [Header("Damage")]
    public LayerMask EnemyLayer;
    private int     _damage;
    private float   _splashRadius;
    private float   _knockbackForce;

    private Rigidbody2D _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.bodyType = RigidbodyType2D.Kinematic;

        var col = GetComponent<Collider2D>();
        col.isTrigger = true;

        var sr = GetComponent<SpriteRenderer>();
        sr.sortingOrder = 10;  // draw on top
    }

    public void Setup(Vector2 direction, int damage, float splashRadius, float knockbackForce)
    {
        _damage        = damage;
        _splashRadius  = splashRadius;
        _knockbackForce= knockbackForce;

        _rb.linearVelocity = direction.normalized * Speed;


        Destroy(gameObject, 2f);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & EnemyLayer) == 0)
            return;

        Vector2 center = transform.position;

        Collider2D[] hits = Physics2D.OverlapCircleAll(center, _splashRadius, EnemyLayer);
        foreach (var hit in hits)
        {
            Enemy e = hit.GetComponent<Enemy>();
            if (e == null) continue;

            Vector2 enemyPos = e.transform.position;

            Vector2 dir  = (enemyPos - center).normalized;
            float   dist = Vector2.Distance(enemyPos, center);

            float falloff = Mathf.Clamp01(1f - (dist / _splashRadius));
            float force   = _knockbackForce * falloff;

            e.TakeDamage(_damage);
            e.ApplyKnockback(dir, force);
        }

        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _splashRadius);
    }
}
