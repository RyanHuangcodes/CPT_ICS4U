using UnityEngine;
//delete this soon
public class EnemySpawner : MonoBehaviour
{
    public GameObject EnemyPrefab;

    void Start()
    {
        SpawnEnemy(new Vector2(1, 0), 100, 10, 4f); 
    }

    void SpawnEnemy(Vector2 offset, int health, int damage, float speed)
    {
        //spawn position near the empty enemy spawner object
        Vector3 spawnPos = transform.position + (Vector3)offset;

        GameObject enemyGameObj = Instantiate(EnemyPrefab, spawnPos, Quaternion.identity);
        Enemy enemy = enemyGameObj.GetComponent<Enemy>();
        enemy.SetHealth(health);
        enemy.SetDamage(damage);
        enemy.SetSpeed(speed);
    }
}
