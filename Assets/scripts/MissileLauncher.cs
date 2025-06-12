using UnityEngine;

public class MissileLauncher : Tower
{
    [Header("Launcher Stats")]
    public float FireRate = 0.5f;
    public float Range = 8f;
    public int Damage = 25;
    public float ExplosionRadius = 2f;
    public float KnockbackForce = 1f;

    [Header("Prefab & Muzzle")]
    [Tooltip("Drag in your Missile prefab (must have a Missile component)")]
    public Missile missilePrefab;

    [Tooltip("Optional: where on the launcher the missile spawns")]
    public Transform MuzzlePoint;

    private float _fireCooldown;

    protected override void Start()
    {
        base.Start();
        SetInitialized(true);
        _fireCooldown = 0f;
    }

    protected override void Update()
    {
        base.Update();
        _fireCooldown -= Time.deltaTime;
        if (_fireCooldown > 0f)
        {
            return;
        }

        Enemy target = EnemyFinder.FindClosestEnemyInRange((Vector2)transform.position, Range);
        if (target == null)
        {
            return;
        }

        Vector2 dir = (target.transform.position - transform.position).normalized;
        float ang = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0f, 0f, ang);

        SpawnAndLaunchMissile(dir);
        _fireCooldown = 1f / FireRate;
    }

    private void SpawnAndLaunchMissile(Vector2 direction)
    {
        if (missilePrefab == null)
        {
            Debug.LogError($"[{name}] Missile prefab not set!");
            return;
        }

        Vector3 spawnPos = MuzzlePoint != null
            ? MuzzlePoint.position
            : transform.position + (Vector3)direction * 1.2f;
        Quaternion spawnRot = transform.rotation;

        // instantiate via composition: returns a Missile, not a GameObject
        Missile m = Instantiate(missilePrefab, spawnPos, spawnRot, transform);
        m.Setup(direction, Damage, ExplosionRadius, KnockbackForce);
    }

    protected override void Die()
    {
        base.Die();
        // because missiles are children, they’ll be destroyed with this launcher
    }
}