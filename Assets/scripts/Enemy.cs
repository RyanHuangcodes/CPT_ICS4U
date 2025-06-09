using UnityEngine;

public class Enemy : Entity
{
    public Transform BaseTransform;
    public LayerMask ObstacleLayers;
    public float AttackRange = 2f;
    public float AttackCooldown = 0.75f;
    public float MoveForce = 20f;
    public Rigidbody2D _rb;
    public Vector2 _moveDirection;
    public float _lastAttackTime;

    public void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public virtual void Update()
    {
        if (BaseTransform == null)
        {
            var baseObj = GameObject.FindGameObjectWithTag("Base");
            if (baseObj != null)
                BaseTransform = baseObj.transform;
            else
            {
                _moveDirection = Vector2.zero;
                return;
            }
        }

        Transform target = AcquireTarget();
        if (target == null)
        {
            _moveDirection = Vector2.zero;
            return;
        }

        float distance = Vector2.Distance(transform.position, target.position);

        if (distance <= AttackRange && Time.time >= _lastAttackTime + AttackCooldown)
        {
            Attack(target);
            _lastAttackTime = Time.time;
            _moveDirection = Vector2.zero;
        }
        else
        {
            _moveDirection = (target.position - transform.position).normalized;
        }
    }

    public void FixedUpdate()
    {
        if (_moveDirection != Vector2.zero)
        {
            _rb.AddForce(_moveDirection * MoveForce, ForceMode2D.Force);

            float maxSpeed = GetSpeed();
            if (_rb.linearVelocity.magnitude > maxSpeed)
                _rb.linearVelocity = _rb.linearVelocity.normalized * maxSpeed;
        }
    }
    //gpt
    protected virtual Transform AcquireTarget()
    {
        Vector2 dirToBase = (BaseTransform.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dirToBase, Mathf.Infinity, ObstacleLayers);
        return hit.collider != null ? hit.collider.transform : BaseTransform;
    }
    //endgpt
    protected virtual void Attack(Transform target)
    {
        var entity = target.GetComponent<Entity>();
        if (entity != null)
        {
            entity.TakeDamage(GetDamage());
            Debug.Log($"Hit for {GetDamage()}, target HP now {entity.GetHealth()}.");
        }
    }

    public void ApplyKnockback(Vector2 direction, float force)
    {
        if (_rb != null && force > 0f)
            _rb.AddForce(direction * force, ForceMode2D.Impulse);
    }

    protected override void Die()
    {
        Debug.Log("Enemy died");
        base.Die();
    }
}
