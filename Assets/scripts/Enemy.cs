using UnityEngine;
using System.Collections.Generic;

public class Enemy : Entity
{
    private Transform player;
    public GameObject coinPrefab; // Drag coin prefab into this field

    public Enemy() : base() { }

    //commented to fix merge problems for now
    // public void AttackClosestTarget(List<GameObject> towers, List<GameObject> players)
    // {
    //     int capacity = 1;
    //     (float, GameObject)[] targets = new (float, GameObject)[capacity];
    //     int count = 0;
    //
    //     foreach (GameObject tower in towers)
    //     {
    //         if (count >= targets.Length)
    //         {
    //             targets = AddArrayIndex(targets);
    //             float dist = CalculateDistance(transform.position, tower.transform.position);
    //             targets[count++] = (dist, tower);
    //         }
    //     }
    // }
    // public float CalculateDistance(Vector3 a, Vector3 b) { }

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
