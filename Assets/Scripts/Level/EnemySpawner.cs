using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawning Attributes")]
    [SerializeField] private float spawnRate = 0.25f; // Enemies per second

    private float timeSinceLastSpawn;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;
        
        if (timeSinceLastSpawn >= (1f / spawnRate))
        {
            SpawnEnemy();
            timeSinceLastSpawn = 0;
        }
    }

    private void SpawnEnemy()
    {
        // Choose random enemy type
        GameObject prefabToSpawn = LevelManager.main.enemyPrefabs[
            Random.Range(0, LevelManager.main.enemyPrefabs.Length)
        ];

        // Spawn enemy on random enemy spawn
        Instantiate(prefabToSpawn, GetRandomSpawnPoint(), Quaternion.identity);
    }

    private Vector3 GetRandomSpawnPoint()
    {
        return LevelManager.main.enemySpawnPoints[
            Random.Range(0, LevelManager.main.enemySpawnPoints.Length)
        ].position;
    }
}