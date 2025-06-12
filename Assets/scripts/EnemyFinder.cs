using System.Collections.Generic;
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
            float distancex = enemyPos.x - origin.x;
            float distancey = enemyPos.y - origin.y;
            float distSq = distancex * distancex + distancey * distancey;

            if (distSq <= rangeSquared && distSq < smallestDistance)
            {
                smallestDistance = distSq;
                closest = e;
            }
        }

        return closest;
    }
    public static Enemy FindClosestEnemyToBaseInRange(Vector2 towerPos, float towerRange)
    {
        GameObject baseObj = GameObject.FindWithTag("Base");
        if (baseObj == null)
        {
            return null;
        }

        Vector2 basePos = baseObj.transform.position;
        Enemy[] all = Object.FindObjectsByType<Enemy>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

        float rangeSq = towerRange * towerRange;
        int count = 0;
        for (int i = 0; i < all.Length; i++)
        {
            float dsq = ((Vector2)all[i].transform.position - towerPos).sqrMagnitude;
            if (dsq <= rangeSq)
            {
                all[count] = all[i];
                count++;
            }
        }

        if (count == 0)
        {
            return null;
        }

        Enemy[] arr = new Enemy[count];
        Enemy[] tmp = new Enemy[count];
        for (int i = 0; i < count; i++)
        {
            arr[i] = all[i];
        }

        MergeSort(arr, tmp, 0, count - 1, basePos);
        return arr[0];
    }

    private static void MergeSort(Enemy[] arr, Enemy[] tmp, int left, int right, Vector2 basePos)
    {
        if (left >= right)
        {
            return;
        }

        int mid = left + (right - left) / 2;
        MergeSort(arr, tmp, left, mid, basePos);
        MergeSort(arr, tmp, mid + 1, right, basePos);

        int i = left;
        int j = mid + 1;
        int k = left;

        while (i <= mid && j <= right)
        {
            float di = ((Vector2)arr[i].transform.position - basePos).sqrMagnitude;
            float dj = ((Vector2)arr[j].transform.position - basePos).sqrMagnitude;
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

    public static Enemy FindHighestHealthTarget(Vector2 origin, float range)
    {
        float rangeSquared = range * range;

        Enemy[] allEnemies = Object.FindObjectsByType<Enemy>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

        Enemy highestHealthEnemy = null;
        float highestHealth = float.MinValue;

        for (int i = 0; i < allEnemies.Length; i++) 
        {
            Enemy enemy = allEnemies[i]; 
            Vector2 position = enemy.transform.position; 
            float distanceX = position.x - origin.x;
            float distanceY = position.y - origin.y;
            float distanceSquared = distanceX * distanceX + distanceY * distanceY; 

            if (distanceSquared <= rangeSquared) 
            {
                float health = enemy.GetHealth(); 
                if (health > highestHealth)
                {
                    highestHealth = health;
                    highestHealthEnemy = enemy;
                }
            }
        }

        return highestHealthEnemy;
    }

   
}
