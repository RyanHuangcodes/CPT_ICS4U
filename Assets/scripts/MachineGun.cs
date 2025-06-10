using UnityEngine;

public class MachineGunTower : Tower
{
    [Header("Machine Gun Stats")]
    public float FireRate = 8f;
    public float Range = 5f;
    public int Damage = 2;
    public float KnockbackForce = 0.3f;

    [Header("Muzzle Flash")]
    public GameObject MuzzleFlashPrefab;
    public float MuzzleFlashDuration = 0.2f;

    [Header("Muzzle Offset (Local)")]
    [Tooltip("Offset along the turret's local right (X) axis")]
    public float MuzzleOffsetX = 0f;
    [Tooltip("Offset along the turret's local up (Y) axis")]
    public float MuzzleOffsetY = 0.5f;

    private float _fireCooldown;
    private float _rangeSquared;

    protected override void Start()
    {
        base.Start();
        SetInitialized(true);
        // SetHealth(100);
        // SetMaxHealth(100);
        // SetDamage(Damage);
        // SetSpeed(0f);

        _fireCooldown = 0f;
        _rangeSquared = Range * Range;
    }

    private void Update()
    {
        _fireCooldown -= Time.deltaTime;
        if (_fireCooldown > 0f) return;

        Enemy target = EnemyFinder.FindClosestEnemyInRange(
            (Vector2)transform.position,
            Range
        );
        if (target == null) return;

        Vector2 dir = (target.transform.position - transform.position).normalized;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        Vector3 localOffset = new Vector3(MuzzleOffsetX, MuzzleOffsetY, 0f);
        Vector3 worldPos = transform.TransformPoint(localOffset);

        if (MuzzleFlashPrefab != null)
        {
            var flash = Instantiate(
                MuzzleFlashPrefab,
                worldPos,
                transform.rotation
            );
            Destroy(flash, MuzzleFlashDuration);
        }

        target.TakeDamage(Damage);
        target.ApplyKnockback(dir, KnockbackForce);

        _fireCooldown = 1f / FireRate;
    }

    protected override void Die()
    {
        MachineGunPlacementTracker.Instance?.Decrement();
        base.Die();
    }
}
