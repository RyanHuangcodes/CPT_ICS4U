using UnityEngine;

public class Cannon : Tower
{
    public float FireRate = 1.1f;
    public float Range = 7.0f;
    public int Damage = 15;
    public float SplashRadius = 2.0f;
    public float KnockbackForce = 1f;
    private float _fireCooldown;

    public GameObject CannonballPrefab;
    public Transform MuzzlePoint;
  

    

  protected override void Start()
    {
        SetInitialized(true);
        _fireCooldown = 0f;
 //       _rangeSquared = Range * Range;
    }

    protected override void Update()
    {
        base.Update();
        _fireCooldown -= Time.deltaTime;

        Enemy target = EnemyFinder.FindClosestEnemyInRange(transform.position, Range);
        if (target == null) return;

        Vector2 dir = (target.transform.position - transform.position).normalized;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        if (_fireCooldown <= 0f)
        {
            SpawnAndFireCannonball(dir);
            _fireCooldown = 1f / FireRate;
        }
    } //gpt rotation

    private void SpawnAndFireCannonball(Vector2 direction)
    {
        if (CannonballPrefab == null || MuzzlePoint == null) return;

        // This is a Composition as cannon owns the cannonball
        GameObject cb = Instantiate(CannonballPrefab, MuzzlePoint.position, Quaternion.identity, transform); // instantiate spawns cannonball in
        Cannonball cannonball = cb.GetComponent<Cannonball>();
        if (cannonball != null)
        {
            cannonball.Setup(direction, Damage, SplashRadius, KnockbackForce);
        }
    }

    protected override void Die()
    {
        base.Die();
        // Aggregation, cannonballs are not destroyed when the cannon dies
    }
}
