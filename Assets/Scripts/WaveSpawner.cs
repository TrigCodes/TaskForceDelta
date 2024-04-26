using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefab;
    [SerializeField] private float timeBetweenWaves = 5.3f;
    [SerializeField] private float countdown = 2f;
    [SerializeField] private GameObject[] spawnPoint;
    [SerializeField] private Text waveCountdownText;

    private int waveNumber = 0;

    void Update() {
        if (countdown <= 0f) {
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
        }
        countdown -= Time.deltaTime;
        waveCountdownText.text = Mathf.Round(countdown).ToString();
    }

    IEnumerator SpawnWave() {
        waveNumber++;
        for (int i = 0; i < waveNumber; i++) {
            SpawnEnemy();
            yield return new WaitForSeconds(0.5f);
        }
    }
    void SpawnEnemy() {
        Transform location = spawnPoint[Random.Range(0, spawnPoint.Length)].transform;
        Transform enemy = enemyPrefab[Random.Range(0,enemyPrefab.Length)].transform;
        Instantiate(enemy, location.position, location.rotation);
    }
}
