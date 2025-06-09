using UnityEngine;

public static class EnemyFinder
{
    //linear search algorithm
    public static Enemy FindClosestEnemyInRange(Vector2 origin, float range)
    {
        float rangeSquared = range * range;
        Enemy[] enemies = Object.FindObjectsByType<Enemy>(
            FindObjectsInactive.Exclude,
            FindObjectsSortMode.None
        );

        Enemy closest = null;
        float smallestDistance = float.MaxValue;

        foreach (Enemy e in enemies)
        {
            Vector2 enemyPos = e.transform.position;
            float dx = enemyPos.x - origin.x;
            float dy = enemyPos.y - origin.y;
            float distSq = dx * dx + dy * dy;

            if (distSq <= rangeSquared && distSq < smallestDistance)
            {
                smallestDistance = distSq;
                closest = e;
            }
        }

        return closest;
    }
    public static Enemy FindClosestEnemyToBaseInRange(float range)
    {
        GameObject baseObj = GameObject.FindWithTag("Base");
        if (baseObj == null)
        {
            return null;
        }

        Vector2 basePos = baseObj.transform.position;
        float rangeSq = range * range;

        Enemy[] all = Object.FindObjectsByType<Enemy>(
            FindObjectsInactive.Exclude,
            FindObjectsSortMode.None
        );

        int count = 0;
        for (int i = 0; i < all.Length; i++)
        {
            Vector2 pos = all[i].transform.position;
            float dx = pos.x - basePos.x;
            float dy = pos.y - basePos.y;
            float distSq = dx * dx + dy * dy;

            if (distSq <= rangeSq)
            {
                all[count] = all[i];
                count++;
            }
        }

        if (count == 0)
        {
            return null;
        }

        Enemy[] arr  = new Enemy[count];
        Enemy[] temp = new Enemy[count];
        System.Array.Copy(all, arr, count);

        void MergeSort(int left, int right)
        {
            if (left >= right)
            {
                return;
            }

            int mid = (left + right) >> 1;

            MergeSort(left, mid);
            MergeSort(mid + 1, right);

            int i = left;
            int j = mid + 1;
            int k = left;

            while (i <= mid && j <= right)
            {
                float di = ((Vector2)arr[i].transform.position - basePos).sqrMagnitude;
                float dj = ((Vector2)arr[j].transform.position - basePos).sqrMagnitude;

                if (di <= dj)
                {
                    temp[k] = arr[i];
                    k++;
                    i++;
                }
                else
                {
                    temp[k] = arr[j];
                    k++;
                    j++;
                }
            }

            while (i <= mid)
            {
                temp[k] = arr[i];
                k++;
                i++;
            }

            while (j <= right)
            {
                temp[k] = arr[j];
                k++;
                j++;
            }

            for (int t = left; t <= right; t++)
            {
                arr[t] = temp[t];
            }
        }

        MergeSort(0, count - 1);
        return arr[0];
    }
}
