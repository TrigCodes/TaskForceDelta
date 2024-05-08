/***************************************************************
*file: EnemySpawner.cs
*author: Samin Hossain, An Le, Otto Del Cid, Luis Navarrete, Luis Salazar, Sebastian Cursaro
*class: CS 4700 - Game Development
*assignment: Final Program
*date last modified: 5/6/2024
*
*purpose: This class provide general behavior spawning enemies
*
****************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // inner class that handle enemies in a wave and its spawn rate
    [System.Serializable]
    public class Wave
    {
        public int[] enemyCount;
        public float spawnRate;
    }

    [Header("Wave Attributes")]
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private Wave[] waves = new Wave[]
    {
        // Assuming index 0: BasicEnemy, index 1: RangerEnemy, index 2: StealthEnemy
        // Wave 1: Starting easy, more basic enemies
        new Wave { enemyCount = new int[] { 5, 2, 1 }, spawnRate = 1f },
        // Wave 2: Introduce a few more rangers
        new Wave { enemyCount = new int[] { 5, 4, 2 }, spawnRate = 1f },
        // Wave 3: Increase stealth enemies slightly
        new Wave { enemyCount = new int[] { 6, 4, 3 }, spawnRate = 1.2f },
        // Wave 4: Balanced increase
        new Wave { enemyCount = new int[] { 8, 5, 4 }, spawnRate = 1.3f },
        // Wave 5: Mid-game challenge
        new Wave { enemyCount = new int[] { 10, 5, 5 }, spawnRate = 1.5f },
        // Wave 6: Slight bump in difficulty
        new Wave { enemyCount = new int[] { 12, 7, 6 }, spawnRate = 1.7f },
        // Wave 7: More rangers
        new Wave { enemyCount = new int[] { 12, 10, 6 }, spawnRate = 1.8f },
        // Wave 8: Higher stealth challenge
        new Wave { enemyCount = new int[] { 12, 10, 8 }, spawnRate = 2.0f },
        // Wave 9: Pre-final challenge, high tension
        new Wave { enemyCount = new int[] { 14, 12, 10 }, spawnRate = 2.2f },
        // Wave 10: Final wave, maximum difficulty
        new Wave { enemyCount = new int[] { 15, 15, 12 }, spawnRate = 2.5f }
    };
    [SerializeField] private float pauseDuration = 10.0f;
    private int currentWaveIndex = 0;
    private Wave currentWave;
    private int totalEnemiesToSpawn;
    private int enemiesSpawned;
    private float timeSinceLastSpawn;
    private float pauseTimer;
    private bool isPausing = false;
    private int activeEnemies;

    // function: Start
    // purpose: Called before the first frame update to set gameObject necessary info.
    void Start()
    {
        isPausing = true;  // Start the game with a pause
        pauseTimer = pauseDuration;  // Initialize the pause timer
    }

    // function: Update
    // purpose: handle spawning wave
    void Update()
    {
        if (isPausing)
        {
            PauseBetweenWaves();
            return;
        }

        timeSinceLastSpawn += Time.deltaTime;

        // Ensure we only attempt to spawn more enemies if there are still enemies to be spawned for the current wave
        if (timeSinceLastSpawn >= (1f / currentWave.spawnRate) && enemiesSpawned < totalEnemiesToSpawn)
        {
            SpawnEnemy();
            timeSinceLastSpawn = 0;
        }
        else if (enemiesSpawned >= totalEnemiesToSpawn && activeEnemies == 0)
        {
            // Ensure to only transition if all enemies of the current wave are dealt with
            TransitionToNextWave();
        }
    }
    // function: PauseBetweenWaves
    // purpose: pause for a time before the next wave start
    void PauseBetweenWaves()
    {
        pauseTimer -= Time.deltaTime;
        if (pauseTimer <= 0)
        {
            isPausing = false;
            StartWave();
        }
    }
    // function: TransitionToNextWave
    // purpose: ready for the next wave
    void TransitionToNextWave()
    {
        currentWaveIndex++;
        if (currentWaveIndex < waves.Length)
        {
            isPausing = true;
            pauseTimer = pauseDuration;
        }
        else
        {
            // All waves are completed, trigger the win condition
            LevelManager.main.WinLevel();
        }
    }
    // function: StartWave
    // purpose: start the wave
    void StartWave()
    {
        if (currentWaveIndex >= waves.Length)
        {
            return; // All waves complete
        }

        currentWave = waves[currentWaveIndex];
        totalEnemiesToSpawn = 0;
        foreach (int count in currentWave.enemyCount)
        {
            totalEnemiesToSpawn += count;
        }
        enemiesSpawned = 0;
        activeEnemies = totalEnemiesToSpawn;
        timeSinceLastSpawn = 0;

        LevelManager.main.UI.GetComponent<TopHUD>().UpdateWaveDisplay(
            currentWaveIndex+1,
            waves.Length
        );

        // Send wave alert
        FindObjectOfType<Alert>().DisplayAlert($"Wave {currentWaveIndex+1}");
    }
    // function: SpawnEnemy
    // purpose: spawn enemies
    void SpawnEnemy()
    {
        int prefabIndex = Random.Range(0, enemyPrefabs.Length);
        if (currentWave.enemyCount[prefabIndex] > 0)
        {
            Instantiate(enemyPrefabs[prefabIndex], GetRandomSpawnPoint(), Quaternion.identity);
            currentWave.enemyCount[prefabIndex]--;
            enemiesSpawned++;
        }
    }
    // function: EnemyDestroyed
    // purpose: call this method from the Enemy class when an enemy is destroyed
    public void EnemyDestroyed()
    {
        activeEnemies--;
    }
    // function: GetRandomSpawnPoint
    // purpose: randomly get spawn location
    private Vector3 GetRandomSpawnPoint()
    {
        return LevelManager.main.enemySpawnPoints[
            Random.Range(0, LevelManager.main.enemySpawnPoints.Length)
        ].position;
    }
}
