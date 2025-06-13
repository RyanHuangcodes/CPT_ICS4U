using UnityEngine;
//gpt
public class DualMachineGunTower : Tower
{
    [Header("Dual Machine Gun Stats")]
    public float FireRate = 8f;
    public float Range = 5f;
    public int Damage = 2;
    public float KnockbackForce = 0.3f;

    [Header("Muzzle Flash")]
    public GameObject MuzzleFlashPrefab;
    public float MuzzleFlashDuration = 0.2f;

    [Header("Muzzle Offsets (Local)")]
    [Tooltip("Local offset for barrel #1 when turret is at 0° (up)")]
    public Vector2 MuzzleOffset1 = new Vector2(-0.3f, 1.4f);
    [Tooltip("Local offset for barrel #2 when turret is at 0° (up)")]
    public Vector2 MuzzleOffset2 = new Vector2( 0.35f,1.4f);

    private float _fireCooldown;
    private float _rangeSquared;

    protected override void Start()
    {
        base.Start();
        SetInitialized(true);
        _fireCooldown = 0f;
        _rangeSquared = Range * Range;
    }

    protected override void Update()
    {
        base.Update();
        _fireCooldown -= Time.deltaTime;
        if (_fireCooldown > 0f) return;

        // 1) find nearest enemy
        Enemy target = EnemyFinder.FindClosestEnemyInRange((Vector2)transform.position, Range);
        if (target == null) return;

        // 2) aim the turret
        Vector2 dir  = (target.transform.position - transform.position).normalized;
        float  angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // 3) compute world‐space muzzle positions
        Vector3 worldPos1 = transform.TransformPoint(MuzzleOffset1);
        Vector3 worldPos2 = transform.TransformPoint(MuzzleOffset2);

        // 4) spawn muzzle flashes
        if (MuzzleFlashPrefab != null)
        {
            var f1 = Instantiate(MuzzleFlashPrefab, worldPos1, transform.rotation);
            Destroy(f1, MuzzleFlashDuration);

            var f2 = Instantiate(MuzzleFlashPrefab, worldPos2, transform.rotation);
            Destroy(f2, MuzzleFlashDuration);
        }

        target.TakeDamage(Damage);
        target.ApplyKnockback(dir, KnockbackForce);
        _fireCooldown = 1f / FireRate;
    }

    protected override void Die()
    {
        DualMachineGunPlacementTracker.Instance?.Decrement();
        base.Die();
    }
}
