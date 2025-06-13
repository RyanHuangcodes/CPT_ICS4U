using UnityEngine;

public class PiercingCannonTower : Tower
{
    [Header("Tower Stats")]
    public float FireRate = 0.5f;
    public float Range = 8f;

    [Header("Bullet Stats")]
    public int Damage = 50;
    public float BulletSpeed = 15f;
    public float BulletRange = 12f;
    public LayerMask EnemyLayer;

    [Header("Prefab & Muzzle")]
    public BigBullet BigBulletPrefab;
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

        Enemy target = EnemyFinder.FindFurthestEnemyInRange((Vector2)transform.position, Range);
        if (target == null) return;

        Vector2 dir = (target.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        FireBigBullet(dir);
        _fireCooldown = 1f / FireRate;
    }

    private void FireBigBullet(Vector2 direction)
    {
        if (BigBulletPrefab == null) return;

        Vector3 spawnPos = MuzzlePoint != null
            ? MuzzlePoint.position
            : transform.position;

        var bullet = Instantiate(
            BigBulletPrefab,
            spawnPos,
            transform.rotation,
            transform
        );

        bullet.Setup(direction, Damage, BulletSpeed, BulletRange, EnemyLayer);
    }

    protected override void Die()
    {
        base.Die();
    }
}