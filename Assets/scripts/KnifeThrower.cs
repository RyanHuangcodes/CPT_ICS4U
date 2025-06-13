using UnityEngine;
//gpt
public class KnifeThrower : MonoBehaviour
{
    public static KnifeThrower Instance { get; private set; }

    [Header("Throw Settings")]
    public GameObject KnifePrefab;      // Prefab must have a SpriteRenderer on its root
    public Transform  ThrowPoint;       // Optional spawn anchor
    public float      ThrowForce    = 10f;
    public float      ThrowCooldown = 1f;
    public int        BaseAvgDamage = 70;

    [Header("Upgrade Skins (Tier 0,1,2…)")]
    public Sprite[]   KnifeSkins;       // 0 = default, 1 = tier1, etc.

    private float _lastThrowTime;
    private int   _upgradeTier;
    private int   _initialBaseAvgDamage;
    private float _initialThrowCooldown;

    public int  CurrentUpgradeTier    => _upgradeTier;
    public bool MaxUpgradeLevelReached 
        => KnifeSkins != null && _upgradeTier >= KnifeSkins.Length - 1;

    private void Awake()
    {
        Instance               = this;
        _initialBaseAvgDamage  = BaseAvgDamage;
        _initialThrowCooldown  = ThrowCooldown;
        _lastThrowTime         = -ThrowCooldown;

        // start at tier 0
        SetUpgradeTier(0);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= _lastThrowTime + ThrowCooldown)
        {
            ThrowKnife();
            _lastThrowTime = Time.time;
        }
    }

    /// <summary>
    /// Advances to next tier: doubles damage, reduces cooldown, swaps skin.
    /// </summary>
    public void UpgradeWeapon()
    {
        if (MaxUpgradeLevelReached) return;
        SetUpgradeTier(_upgradeTier + 1);
    }

    /// <summary>
    /// Directly sets tier (used on load or at Awake for tier 0).
    /// </summary>
    public void SetUpgradeTier(int tier)
    {
        _upgradeTier    = tier;
        BaseAvgDamage   = _initialBaseAvgDamage * (int)Mathf.Pow(2, tier);
        ThrowCooldown   = Mathf.Max(0.1f, _initialThrowCooldown - 0.2f * tier);

        // swap the prefab’s sprite so all future knives use it
        if (KnifePrefab != null && KnifeSkins != null && KnifeSkins.Length > tier)
        {
            var sr = KnifePrefab.GetComponent<SpriteRenderer>();
            if (sr != null)
                sr.sprite = KnifeSkins[tier];
        }
    }

    private void ThrowKnife()
    {
        Vector3 mPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mPos.z = 0f;
        Vector2 dir = (mPos - transform.position).normalized;

        Vector3 spawnPos = transform.position + (Vector3)(dir * 1f);
        var knife = Instantiate(KnifePrefab, spawnPos, Quaternion.identity);

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        knife.transform.rotation = Quaternion.Euler(0, 0, angle - 45f);

        var rb = knife.GetComponent<Rigidbody2D>();
        if (rb != null) rb.linearVelocity = dir * ThrowForce;

        var kComp = knife.GetComponent<Knife>();
        if (kComp != null)
            kComp.Damage = new Weapon(BaseAvgDamage).GetDamage();
    }
}
