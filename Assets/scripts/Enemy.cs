using UnityEngine;

public class Enemy : Entity
{
    private Transform player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player")?.transform;
    }

    private void Update()
    {
        if (player != null)
        {
            //follow player
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position += (Vector3)(direction * GetSpeed() * Time.deltaTime);
        }
    }

    protected override void Die()
    {
        Debug.Log("died");
        base.Die();
    }
}
