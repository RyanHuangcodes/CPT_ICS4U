using UnityEngine;

public class Cannonball : MonoBehaviour
{
    public float Speed = 10f;
    public LayerMask EnemyLayer;

    private Vector2 _direction;
    private int _damage;
    private float _splashRadius;
    private float _knockbackForce;
    private bool _fired = false;

    public void Setup(Vector2 direction, int damage, float splashRadius, float knockbackForce)
    {
        _direction = direction.normalized;
        _damage = damage;
        _splashRadius = splashRadius;
        _knockbackForce = knockbackForce;
        _fired = true;

        Destroy(gameObject, 5f); // Auto-cleanup
    }

    void Update()
    {
        if (_fired)
            transform.Translate(_direction * Speed * Time.deltaTime, Space.World);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        int otherLayer = 1 << other.gameObject.layer;
        bool isEnemyLayer = (otherLayer & EnemyLayer) != 0;

        if (!isEnemyLayer)
        {
            return;
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _splashRadius, EnemyLayer);
        foreach (var hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(_damage);
                Vector2 knockDir = (enemy.transform.position - transform.position).normalized;
                enemy.ApplyKnockback(knockDir, _knockbackForce);
            }
        }

        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _splashRadius);
    }

    // Gpt code
}
