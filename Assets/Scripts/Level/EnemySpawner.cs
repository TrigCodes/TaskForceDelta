using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs;

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
        GameObject prefabToSpawn = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

        // Spawn enemy on random enemy spawn
        Instantiate(prefabToSpawn, GetRandomSpawnPoint(), Quaternion.identity);
    }

    private Vector3 GetRandomSpawnPoint()
    {
        return LevelManager.main.spawnPoints[Random.Range(0, LevelManager.main.spawnPoints.Length)].position;
    }
}