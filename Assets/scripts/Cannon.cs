using UnityEngine;
//gpt
public class Cannon : Tower
{
    [Header("Firing Stats")]
    public float FireRate       = 1.1f;
    public float Range          = 7f;
    public int   Damage         = 15;
    public float SplashRadius   = 2f;
    public float KnockbackForce = 1f;

    [Header("Projectile Prefab")]
    public GameObject CannonballPrefab;

    private const float _muzzleDistance = 1.2f;
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

        // only try to fire (and rotate) when cooldown has elapsed
        if (_fireCooldown <= 0f)
        {
            Enemy target = EnemyFinder.FindClosestEnemyToBaseInRange(transform.position, Range);
            if (target != null)
            {
                // compute direction & angle at the moment of shooting
                Vector2 dir = (target.transform.position - transform.position).normalized;
                float ang = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                float dispA = ang - 90f;               // sprite points +Y by default

                // rotate the cannon just once, right before firing
                transform.rotation = Quaternion.Euler(0f, 0f, dispA);

                // spawn & launch
                SpawnAndFireCannonball(dir);

                // reset cooldown
                _fireCooldown = 1f / FireRate;
            }
        }
    }

    private void SpawnAndFireCannonball(Vector2 direction)
    {
        if (CannonballPrefab == null)
        {
            Debug.LogError($"[{name}] Missing CannonballPrefab!");
            return;
        }

        // fire from a point 1.2 units out along the barrel
        Vector3 spawnPos = transform.position + (Vector3)direction * _muzzleDistance;
        Quaternion spawnRot = transform.rotation;  // already set above

        var cbObj = Instantiate(CannonballPrefab, spawnPos, spawnRot);
        var cb    = cbObj.GetComponent<Cannonball>();
        if (cb != null)
            cb.Setup(direction, Damage, SplashRadius, KnockbackForce);
    }

    protected override void Die()
    {
        base.Die();
        CannonPlacementTracker.Instance?.Decrement();
        // in-flight projectiles remain
    }
}
