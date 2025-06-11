using System.Collections.Generic;
using System;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;
using UnityEngine.EventSystems;

public class Boss: Enemy
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //void Start()
    //{
        
    //}
    public override void Update()
    {
        Tower[] towerArray = FindObjectsByType<Tower>(FindObjectsSortMode.None);
        List<Tower> towerList = new List<Tower>(towerArray);

        if (towerList.Count == 0)
        {
            MoveDirection = Vector2.zero;
            return;
        }

        Tower closestTower = FindClosestTarget(towerList);
        if (closestTower == null)
        {
            MoveDirection = Vector2.zero;
            return;
        }

        float distance = Vector2.Distance(transform.position, closestTower.transform.position);

        if (distance <= AttackRange && Time.time >= LastAttackTime + AttackCooldown)
        {
            Attack(closestTower.transform);
            LastAttackTime = Time.time;
            MoveDirection = Vector2.zero;
        }
        else
        {
            MoveDirection = (closestTower.transform.position - transform.position).normalized;
        }
    }

    //public Zombie() : base()
    //{

    //}
    public Tower FindClosestTarget(List<Tower> towers)
    {
        int _capacity = 1;
        (float, Tower)[] targets = new (float, Tower)[_capacity];
        int count = 0;

        foreach (Tower tower in towers)
        {
            if (count >= targets.Length)
            {
                targets = AddArrayIndex(targets);
            }
            float dist = CalculateDistance(transform.position, tower.transform.position);
            targets[count] = (dist, tower);
            count++;
        }

        MergeSort(targets, 0, count - 1);
        return targets[0].Item2; // Return the closest tower

        //targets[0].Item2.takeDamage(GetDamage());

        // Item2 means the Gameobject as targets are in form (float, GameObject)
        // The list needs both float which is sorted for distance and the Gameobject to know which is which
    }
    private void MergeSort((float, Tower)[] targets, int start, int end)
    {
        if (end <= start)
            return;

        int mid = start + (end - start) / 2;

        MergeSort(targets, start, mid);
        MergeSort(targets, mid + 1, end);

        (float, Tower)[] newArray = new (float, Tower)[end - start + 1];
        int cursor = 0;
        int left = start;
        int right = mid + 1;

        while (left < mid + 1 && right <= end)
        {
            if (targets[left].Item1 < targets[right].Item1)
            {
                newArray[cursor] = targets[left];
                left += 1;
            }
            else
            {
                newArray[cursor] = targets[right];
                right += 1;
            }
            cursor += 1;
        }

        while (left < mid + 1)
        {
            newArray[cursor] = targets[left];
            left += 1;
            cursor += 1;
        }

        while (right <= end)
        {
            newArray[cursor] = targets[right];
            right += 1;
            cursor += 1;
        }

        for (int i = 0; i < newArray.Length; i++)
        {
            targets[start + i] = newArray[i];
        }
    }
    private float CalculateDistance(Vector3 posEnemy, Vector3 posTower)
    {
        float distanceX = posEnemy.x - posTower.x;
        float distanceY = posEnemy.y - posTower.y;
        return Mathf.Sqrt(distanceX * distanceX + distanceY * distanceY);
    }
    //AddArrayIndex() is gpt code 
    private (float, Tower)[] AddArrayIndex((float, Tower)[] original)
    {
        int newSize = original.Length + 1;
        var newArray = new (float, Tower)[newSize];
        Array.Copy(original, newArray, original.Length);
        return newArray;
    }

    // Update is called once per frame

}
