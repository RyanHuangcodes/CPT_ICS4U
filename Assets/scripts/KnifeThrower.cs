using UnityEngine;
//gpt
public class KnifeThrower : MonoBehaviour
{
    public GameObject knifePrefab;
    public Transform throwPoint;
    public float throwForce = 10f;
    public float throwCooldown = 1f;

    private Weapon currentWeapon;
    private float lastThrowTime;

    void Start()
    {
        currentWeapon = new Weapon(25);
        lastThrowTime = -throwCooldown; // allow throwing right away
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= lastThrowTime + throwCooldown)
        {
            ThrowKnife();
            lastThrowTime = Time.time;
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
        GameObject knife = Instantiate(knifePrefab, spawnPos, Quaternion.identity);

        // Rotate knife to face direction (+45° for sprite correction)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        knife.transform.rotation = Quaternion.Euler(0f, 0f, angle - 45f);

        // Launch the knife
        knife.GetComponent<Rigidbody2D>().linearVelocity = direction * throwForce;

        // Set randomized damage
        Weapon thrownWeapon = new Weapon(currentWeapon.GetAvgDamage());
        knife.GetComponent<Knife>().damage = thrownWeapon.GetDamage();
    }
}
