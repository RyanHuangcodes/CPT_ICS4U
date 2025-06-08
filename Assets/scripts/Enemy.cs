using UnityEngine;
using System.Collections.Generic;
using System;

public class Enemy : Entity
{
    private Transform _player;
    // public GameObject coinPrefab;
    private Weapon _weapon;

    public Enemy() : base() {
        _weapon = new Weapon(20);
    }


    public void AttackClosestTarget(List<GameObject> towers, List<GameObject> players)
    {
        int _capacity = 1;
        (float, GameObject)[] _targets = new (float, GameObject)[_capacity];
        int _count = 0;

        foreach (GameObject tower in towers)
        {
            if (_count >= _targets.Length)
            {
                _targets = AddArrayIndex(_targets);
            }
            float dist = CalculateDistance(transform.position, tower.transform.position);
            _targets[_count] = (dist, tower);
            _count++;
        }
        if (_count == 0)
        {
            return;
        }
        MergeSort(_targets, 0, _count - 1);
        _weapon.Use(_targets[0].Item2);
        // Item2 means the Gameobject as targets are in form (float, GameObject)
        // The list needs both float which is sorted for distance and the Gameobject to know which is which
    }
    private void MergeSort((float, GameObject)[] targets, int start, int end)
    {
        if (end <= start)
            return;

        int _mid = start + (end - start) / 2;

        MergeSort(targets, start, _mid);
        MergeSort(targets, _mid + 1, end);

        (float, GameObject)[] newArray = new (float, GameObject)[end - start + 1];
        int _cursor = 0;
        int _left = start;
        int _right = _mid + 1;

        while (_left < _mid + 1 && _right <= end)
        {
            if (targets[_left].Item1 < targets[_right].Item1)
            {
                newArray[_cursor] = targets[_left];
                _left += 1;
            }
            else
            {
                newArray[_cursor] = targets[_right];
                _right += 1;
            }
            _cursor += 1;
        }

        while (_left < _mid + 1)
        {
            newArray[_cursor] = targets[_left];
            _left += 1;
            _cursor += 1;
        }

        while (_right <= end)
        {
            newArray[_cursor] = targets[_right];
            _right += 1;
            _cursor += 1;
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
    //AddArrayIndex() is gpt code 
    private (float, GameObject)[] AddArrayIndex((float, GameObject)[] original)
    {
        int _newSize = original.Length + 1;
        var _newArray = new (float, GameObject)[_newSize];
        Array.Copy(original, _newArray, original.Length);
        return _newArray;
    }
    
    private void Start()
    {
        _player = GameObject.FindWithTag("Player")?.transform;
    }

    private void Update()
    {
        if (_player != null)
        {
            // enemy moves towards player
            Vector2 direction = (_player.position - transform.position).normalized;
            transform.position += (Vector3)(direction * GetSpeed() * Time.deltaTime);
        }
    }

    // private void GiveCoinsToPlayer()
    // {
    //     if (CoinManager.Instance != null)
    //         CoinManager.Instance.AddCoins(5); // Add 5 coins
    // }

    protected override void Die()
    {
        // GiveCoinsToPlayer();

        Debug.Log("died");
        base.Die();
    }
}
