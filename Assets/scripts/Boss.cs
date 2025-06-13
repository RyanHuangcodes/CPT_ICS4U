using UnityEngine;

public class Boss : Enemy
{

    private float _bossAttackRange    = 3f;
    private float _bossAttackCooldown = 1.5f;
    public GameObject CoinPrefab;

    protected override void Start()
    {
        AttackRange = _bossAttackRange;
        AttackCooldown = _bossAttackCooldown;
    }
    // Accessor methods or getters for accessing private attributes
    public float GetBossAttackRange()
    {
        return _bossAttackRange;
    }

    public float GetBossAttackCooldown()
    {
        return _bossAttackCooldown;
    }
    protected override Transform AcquireTarget()
    {
        Tower[] all = Object.FindObjectsByType<Tower>(FindObjectsInactive.Exclude,FindObjectsSortMode.None); //gpt line to get array of all towers in the scene
        if (all.Length == 0) // if there are no twoers
        {
            return base.AcquireTarget();
        }

        Vector2 origin = transform.position; // boss position
        Tower[] tmp = new Tower[all.Length]; //temporary array  for sorting
        MergeSort(all, tmp, origin, 0, all.Length - 1); // sort array
        return all[0].transform;
    }

    private void MergeSort(Tower[] towers, Tower[] temp, Vector2 origin, int start, int end)
    {
        // base case
        if (end <= start)
        {
            return;
        }

        int mid = start + (end - start) / 2;

        MergeSort(towers, temp, origin, start, mid);
        MergeSort(towers, temp, origin, mid + 1, end);

        int cursor = start;
        int left = start;
        int right = mid + 1;

        while (left < mid + 1 && right <= end)
        {
            // calculate squared distance from origin to each tower
            float distanceLeft = ((Vector2)towers[left].transform.position - origin).sqrMagnitude; // finds distance from origin to left tower
            float distanceRight = ((Vector2)towers[right].transform.position - origin).sqrMagnitude;// distance to right tower

            if (distanceLeft <= distanceRight)
            {
                temp[cursor] = towers[left];
                left++;
            }
            else
            {
                temp[cursor] = towers[right];
                right++;
            }
            cursor++;
        }

        // if left side has leftover add remaining, otherwise it will check right
        while (left <= mid)
        {
            temp[cursor] = towers[left];
            left++;
            cursor++;
        }

        //if right side has leftover add the rest
        while (right <= end)
        {
            temp[cursor] = towers[right];
            right++;
            cursor++;
        }

        // shove temp array back into towers
        for (int i = start; i <= end; i++)
        {
            towers[i] = temp[i];
        }
    }



    public override void ApplyKnockback(Vector2 distanceIrection, float force)
    {
        //override enemy.cs so there is no knockback
    }

    protected override void Die()
    {
        if (CoinPrefab != null)
        {
            Instantiate(CoinPrefab, transform.position, Quaternion.identity);
        }
        base.Die();
    }

    private new void Awake()
    {
        base.Awake();  
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = Color.white;
        }
    }
}
