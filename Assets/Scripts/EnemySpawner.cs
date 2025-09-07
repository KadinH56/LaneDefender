using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //Enemies
    [SerializeField] private GameObject snake;
    [SerializeField] private GameObject snail;
    [SerializeField] private GameObject slime;

    
    [SerializeField] private Transform[] lanes; //Lane Positions
    [SerializeField] private float spawnInterval = 2f; 
    private float nextSpawnTime = 0f;

    private GameObject[] enemyTypes;

    private void Start()
    {
        // Random selection array
        enemyTypes = new GameObject[] { snake, snail, slime };
    }

    //Spawn enemies
    private void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    private void SpawnEnemy()
    {
        // Picks a random enemy 
        GameObject enemyPrefab = enemyTypes[Random.Range(0, enemyTypes.Length)];

        // Picks a lane
        Transform lane = lanes[Random.Range(0, lanes.Length)];

        // Spawm the enemy 
        Instantiate(enemyPrefab, lane.position, lane.rotation);
    }
}
