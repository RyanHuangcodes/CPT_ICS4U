using UnityEngine;

public class Boss : Enemy
{
    //override boss
    [SerializeField] private float _bossAttackRange    = 3f;
    [SerializeField] private float _bossAttackCooldown = 1.5f;

    private void Start()
    {
        AttackRange = _bossAttackRange;
        AttackCooldown = _bossAttackCooldown;
    }

    protected override Transform AcquireTarget()
    {
        Tower[] all = Object.FindObjectsByType<Tower>(
            FindObjectsInactive.Exclude,
            FindObjectsSortMode.None
        );
        if (all.Length == 0)
        {
            return base.AcquireTarget();
        }

        Vector2 origin = transform.position;
        Tower[] tmp = new Tower[all.Length];
        MergeSort(all, tmp, origin, 0, all.Length - 1);
        return all[0].transform;
    }

    private void MergeSort(Tower[] arr, Tower[] tmp, Vector2 origin, int left, int right)
    {
        if (left >= right)
        {
            return;
        }

        int mid = left + (right - left) / 2;
        MergeSort(arr, tmp, origin, left, mid);
        MergeSort(arr, tmp, origin, mid + 1, right);

        int i = left;
        int j = mid + 1;
        int k = left;

        while (i <= mid && j <= right)
        {
            float di = ((Vector2)arr[i].transform.position - origin).sqrMagnitude;
            float dj = ((Vector2)arr[j].transform.position - origin).sqrMagnitude;
            if (di <= dj)
            {
                tmp[k] = arr[i];
                i++;
            }
            else
            {
                tmp[k] = arr[j];
                j++;
            }
            k++;
        }

        while (i <= mid)
        {
            tmp[k] = arr[i];
            i++;
            k++;
        }

        while (j <= right)
        {
            tmp[k] = arr[j];
            j++;
            k++;
        }

        for (int t = left; t <= right; t++)
        {
            arr[t] = tmp[t];
        }
    }

    public override void ApplyKnockback(Vector2 direction, float force)
    {
    }
}
