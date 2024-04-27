using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private Wave[] waves;
    [SerializeField] private GameObject[] enemyPrefab;
    [SerializeField] private float countdown = 2f;
    [SerializeField] private GameObject[] spawnPoint;
    [SerializeField] private Text waveCountdownText;

    public static int EnemiesAlive = 0;
    private int waveNumber = 0;

    [SerializeField] private float timeBetweenRounds = 5.3f;
    // function: Update
    // purpose: run every frame
    void Update()
    {
        // If there are still enemy from a wave, return
        if (EnemiesAlive > 0)
        {
            return;
        }
        if (countdown <= 0f)
        {
            SpawnWave();
            countdown = timeBetweenRounds;
        }
        countdown -= Time.deltaTime;
        waveCountdownText.text = Mathf.Round(countdown).ToString();
    }
    // function: SpawnWave
    // purpose: spawn all enemies at in a wave
    void SpawnWave()
    {
        if (waveNumber < waves.Length)
        {
            Wave wave = waves[waveNumber];
            for (int i = 0; i < wave.locations.Length; i++)
            {
                Location loc = wave.locations[i];
                StartCoroutine(AtLocation(loc));
            }
            waveNumber++;
        }
    }
    // function: AtLocation
    // purpose: Loop through all cluster, and and spawn enemies based on number of enemies of each cluster.
    IEnumerator AtLocation(Location loc)
    {
        for (int i = 0; i < loc.clusters.Length; i++)
        {
            for (int j = 0; j < loc.clusters[i].numberOfEnemies; j++)
            {
                SpawnEnemy(loc.location, loc.clusters[i].enemyType);
                yield return new WaitForSeconds(0.3f);
            }
        }
    }
    // function: SpawnEnemy
    // purpose: spawn enemy at a location.
    void SpawnEnemy(GameObject spawnLocation, GameObject enemyType)
    {
        Instantiate(enemyType, spawnLocation.transform.position, Quaternion.identity);
        EnemiesAlive++;
    }
}
