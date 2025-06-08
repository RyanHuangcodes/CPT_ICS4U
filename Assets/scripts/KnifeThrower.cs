using UnityEngine;
//gpt
public class KnifeThrower : MonoBehaviour
{
    public GameObject KnifePrefab;
    public Transform ThrowPoint;
    public float ThrowForce = 10f;
    public float ThrowCooldown = 1f;

    private Weapon _currentWeapon;
    private float _lastThrowTime;

    void Start()
    {
        _currentWeapon = new Weapon(70);
        _lastThrowTime = -ThrowCooldown; // allow throwing right away
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= _lastThrowTime + ThrowCooldown)
        {
            ThrowKnife();
            _lastThrowTime = Time.time;
        }
    }

    void ThrowKnife()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        // Direction from player to mouse
        Vector2 direction = (mousePos - transform.position).normalized;

        // Calculate spawn position offset by 0.5 in that direction
        Vector3 spawnPos = transform.position + (Vector3)(direction * 1f);

        // Instantiate knife at the calculated position
        GameObject knife = Instantiate(KnifePrefab, spawnPos, Quaternion.identity);

        // ✅ Ignore collision between knife and player
        Collider2D knifeCollider = knife.GetComponent<Collider2D>();
        Collider2D playerCollider = GetComponent<Collider2D>();

        if (knifeCollider != null && playerCollider != null)
        {
            Physics2D.IgnoreCollision(knifeCollider, playerCollider);
        }

        // Rotate knife to face direction (+45° for sprite correction)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        knife.transform.rotation = Quaternion.Euler(0f, 0f, angle - 45f);

        // Launch the knife
        knife.GetComponent<Rigidbody2D>().linearVelocity = direction * ThrowForce;

        // Set randomized damage
        Weapon thrownWeapon = new Weapon(_currentWeapon.GetAvgDamage());
        knife.GetComponent<Knife>().Damage = thrownWeapon.GetDamage();
    }
}
