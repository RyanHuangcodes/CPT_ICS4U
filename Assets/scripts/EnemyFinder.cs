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
            float distancex = pos.x - basePos.x;
            float distancey = pos.y - basePos.y;
            float distSq = distancex * distancex + distancey * distancey;

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

        Enemy[] arr = new Enemy[count];
        Enemy[] tmp = new Enemy[count];
        for (int i = 0; i < count; i++)
        {
            arr[i] = all[i];
        }

        void MergeSort(int left, int right)
        {
            if (left >= right)
            {
                return;
            }

            int mid = left + (right - left) / 2;

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
                    tmp[k] = arr[i];
                    k++;
                    i++;
                }
                else
                {
                    tmp[k] = arr[j];
                    k++;
                    j++;
                }
            }

            while (i <= mid)
            {
                tmp[k] = arr[i];
                k++;
                i++;
            }

            while (j <= right)
            {
                tmp[k] = arr[j];
                k++;
                j++;
            }

            for (int t = left; t <= right; t++)
            {
                arr[t] = tmp[t];
            }
        }

        MergeSort(0, count - 1);
        return arr[0];
    }
    public static Enemy FindHighestHealthTarget(Vector2 origin, float range)
    {
        float rangeSquared = range * range;

        Enemy[] allEnemies = Object.FindObjectsByType<Enemy>(
            FindObjectsInactive.Exclude,
            FindObjectsSortMode.None
        );

        Enemy highestHealthEnemy = null;
        float highestHealth = float.MinValue;

        for (int i = 0; i < allEnemies.Length; i++) // Linear search through all the enemies for highest health
        {
            Enemy enemy = allEnemies[i]; // current enemy that it looks through
            Vector2 position = enemy.transform.position; // gets position of enemy
            float distanceX = position.x - origin.x;
            float distanceY = position.y - origin.y;
            float distanceSquared = distanceX * distanceX + distanceY * distanceY; //distance

            if (distanceSquared <= rangeSquared) // only consider enemies within range
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
