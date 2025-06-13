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
    public Missile MissilePrefab;   
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
        if (_fireCooldown > 0f) return;

        Enemy target = EnemyFinder.FindHighestHealthTarget((Vector2)transform.position, Range);
        if (target == null) return;

        Vector2 dir = (target.transform.position - transform.position).normalized;
        float   ang = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0f, 0f, ang);

        SpawnAndLaunchMissile(dir);
        _fireCooldown = 1f / FireRate;
    }

    private void SpawnAndLaunchMissile(Vector2 direction)
    {
        if (MissilePrefab == null)
        {
            return;
        }

        Vector3 spawnPos = MuzzlePoint != null
            ? MuzzlePoint.position
            : transform.position + (Vector3)direction * 1.2f;
        Quaternion spawnRot = transform.rotation;

        Missile m = Instantiate(MissilePrefab, spawnPos, spawnRot, transform);
        m.Setup(direction, Damage, ExplosionRadius, KnockbackForce);
    }

    protected override void Die()
    {
        base.Die();
    }
}
