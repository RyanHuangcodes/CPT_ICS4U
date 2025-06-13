using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class BigBullet : MonoBehaviour
{
    [Header("Flight")]
    public float Speed = 15f;
    public float Range = 12f;

    [Header("Damage")]
    public int Damage = 50;
    public LayerMask EnemyLayer;

    private Vector2 _direction;
    private Vector2 _startPos;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.bodyType = RigidbodyType2D.Dynamic;
        _rb.gravityScale = 0f;
        GetComponent<Collider2D>().isTrigger = true;
    }


    public void Setup(Vector2 direction, int damage, float speed, float range, LayerMask enemyLayer)
    {
        _direction = direction.normalized;
        Damage = damage;
        Speed = speed;
        Range = range;
        EnemyLayer = enemyLayer;
        _startPos = transform.position;

        _rb.linearVelocity = _direction * Speed;
    }

    private void Update()
    {
        if (Vector2.Distance(_startPos, transform.position) >= Range)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((EnemyLayer.value & (1 << other.gameObject.layer)) == 0) return;

        if (other.TryGetComponent<Enemy>(out var e))
        {
            e.TakeDamage(Damage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(_startPos, Range);
    }
}