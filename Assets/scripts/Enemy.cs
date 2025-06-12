using UnityEngine;

public class Enemy : Entity
{
    public Transform BaseTransform;
    public LayerMask ObstacleLayers;
    public float AttackRange = 2f;
    public float AttackCooldown = 0.75f;
    public float MoveForce = 20f;
    public Rigidbody2D Rb;
    public Vector2 MoveDirection;
    public float LastAttackTime;

    private Animator _anim;
    private SpriteRenderer _sprite;
    private Vector2 _lastPosition;

    private bool _isAttacking = false;

    public void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
        _lastPosition = transform.position;
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
                MoveDirection = Vector2.zero;
                SetAnimation(false);
                return;
            }
        }

        Transform target = AcquireTarget();
        if (target == null)
        {
            MoveDirection = Vector2.zero;
            SetAnimation(false);
            return;
        }

        float distance = Vector2.Distance(transform.position, target.position);

        if (distance <= AttackRange && Time.time >= LastAttackTime + AttackCooldown)
        {
            MoveDirection = Vector2.zero;
            Rb.linearVelocity = Vector2.zero;
            _isAttacking = true;

            SetAnimation(false);
            FaceTarget(target.position);
            Attack(target);
            LastAttackTime = Time.time;
        }
        else
        {
            MoveDirection = (target.position - transform.position).normalized;
            _isAttacking = false;

            SetAnimation(true);
            FaceTarget(target.position);
        }

        _lastPosition = transform.position;
    }

    public void FixedUpdate()
    {
        if (!_isAttacking && MoveDirection != Vector2.zero)
        {
            Rb.AddForce(MoveDirection * MoveForce, ForceMode2D.Force);

            float maxSpeed = GetSpeed();
            if (Rb.linearVelocity.magnitude > maxSpeed)
                Rb.linearVelocity = Rb.linearVelocity.normalized * maxSpeed;
        }
    }

    protected virtual Transform AcquireTarget()
    {
        Vector2 dirToBase = (BaseTransform.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dirToBase, Mathf.Infinity, ObstacleLayers);
        return hit.collider != null ? hit.collider.transform : BaseTransform;
    }

    protected virtual void Attack(Transform target)
    {
        var entity = target.GetComponent<Entity>();
        if (entity != null)
        {
            entity.TakeDamage(GetDamage());
            Debug.Log($"Hit for {GetDamage()}, target HP now {entity.GetHealth()}.");
        }
    }

    public virtual void ApplyKnockback(Vector2 direction, float force)
    {
        if (Rb != null && force > 0f)
            Rb.AddForce(direction * force, ForceMode2D.Impulse);
    }

    protected override void Die()
    {
        Debug.Log("Enemy died");
        base.Die();
    }

    private void SetAnimation(bool isMoving)
    {
        if (_anim != null)
            _anim.SetBool("isMoving", isMoving);
    }

    private void FaceTarget(Vector3 targetPos)
    {
        Vector2 direction = targetPos - transform.position;

        if (_sprite != null)
            _sprite.flipX = direction.x < 0;
    }
}
