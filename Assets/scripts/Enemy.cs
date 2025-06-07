using UnityEngine;
using System.Collections.Generic;
using System;

public class Enemy : Entity
{
    private Transform player;
    public GameObject coinPrefab; // Drag coin prefab into this field
    private Weapon _weapon;

    public Enemy() : base() {
        _weapon = new Weapon();
    }

   
    public void AttackClosestTarget(List<GameObject> towers, List<GameObject> players)
    {
        int capacity = 1;
        (float, GameObject)[] targets = new (float, GameObject)[capacity];
        int count = 0;

        foreach (GameObject tower in towers)
        {
            if (count >= targets.Length)
            {
                targets = AddArrayIndex(targets);
            }
            float dist = CalculateDistance(transform.position, tower.transform.position);
            targets[count] = (dist, tower);
            count++;
        }
        if (count == 0)
        {
            return;
        }
        MergeSort(targets, 0, count - 1);
        _weapon.Use(targets[0].Item2);
        // Item2 means the Gameobject as targets are in form (float, GameObject)
        // The list needs both float which is sorted for distance and the Gameobject to know which is which
    }
    private void MergeSort((float, GameObject)[] targets, int start, int end)
    {
        if (end <= start)
            return;

        int mid = start + (end - start) / 2;

        MergeSort(targets, start, mid);
        MergeSort(targets, mid + 1, end);

        (float, GameObject)[] newArray = new (float, GameObject)[end - start + 1];
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
        float _distanceX = posEnemy.x - posTower.x;
        float _distanceY = posEnemy.y - posTower.y;
        return Mathf.Sqrt(_distanceX * _distanceX + _distanceY * _distanceY);
    }
    private (float, GameObject)[] AddArrayIndex((float, GameObject)[] original)
    {
        int _newSize = original.Length + 1;
        var _newArray = new (float, GameObject)[_newSize];
        Array.Copy(original, _newArray, original.Length);
        return _newArray;
    }
    //AddArrayIndex() is gpt code 
    private void Start()
    {
        player = GameObject.FindWithTag("Player")?.transform;
    }

    private void Update()
    {
        if (player != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position += (Vector3)(direction * GetSpeed() * Time.deltaTime);
        }
    }

    private void GiveCoinsToPlayer()
    {
        if (CoinManager.Instance != null)
            CoinManager.Instance.AddCoins(5); // Add 5 coins
    }

    protected override void Die()
    {
        GiveCoinsToPlayer();

        if (coinPrefab != null)
            Instantiate(coinPrefab, transform.position, Quaternion.identity);

        Debug.Log("died");
        base.Die();
    }
}
