using UnityEngine;
using System.Collections.Generic;
using System;

public class Enemy : Entity
{
    private Transform _player;
    // public GameObject coinPrefab;

    public Enemy() : base() {

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
