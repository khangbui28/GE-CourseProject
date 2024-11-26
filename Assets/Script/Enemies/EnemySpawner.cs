using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject oldEnemyPrefab;

    public float spawnInterval = 1f;
    public List<Wave> waves;

    private bool isSpawning = false;
    private int currentWaveIndex = 0;
    private int enemiesToSpawn;
    private int enemiesSpawned = 0;
    private int enemiesRemaining;

    public delegate void WaveStarted(int waveIndex);
    public static event WaveStarted OnWaveStarted;

    public delegate void WaveCompleted(int waveIndex);
    public static event WaveCompleted OnWaveCompleted;

    private void Start()
    {
        StartWave();
        UIManager.Instance.UpdateWave(currentWaveIndex);
    }

    public void StartWave()
    {
        if (currentWaveIndex < waves.Count)
        {
            Wave wave = waves[currentWaveIndex];
            enemiesToSpawn = wave.enemyCount;
            enemiesRemaining = wave.enemyCount;

            OnWaveStarted?.Invoke(currentWaveIndex);

            if (!isSpawning)
            {
                StartCoroutine(SpawnEnemies(wave));
            }
        }
        else
        {
            Debug.Log("All waves completed!");
        }
    }

    private IEnumerator SpawnEnemies(Wave wave)
    {
        isSpawning = true;

        while (enemiesSpawned < wave.enemyCount)
        {
            // Check if this is an old or new enemy
            if (wave.useNewSystem)
            {
                EnemyData enemyData = wave.enemiesToSpawn[enemiesSpawned % wave.enemiesToSpawn.Count];
                SpawnNewEnemy(enemyData);
            }
            else
            {
                SpawnOldEnemy();
            }

            enemiesSpawned++;
            yield return new WaitForSeconds(spawnInterval);
        }

        isSpawning = false;
    }

    private void SpawnNewEnemy(EnemyData enemyData)
    {
        Vector3 spawnPosition = GridManager.Instance.GetTileWorldPosition(0, 0);

        if (enemyData != null && enemyData.enemyPrefab != null)
        {
            GameObject enemy = EnemyPool.Instance.GetEnemy(enemyData, spawnPosition);

            if (enemy != null)
            {
                NewEnemy enemyScript = enemy.GetComponent<NewEnemy>();
                if (enemyScript != null)
                {
                    enemyScript.data = enemyData; // Assign shared data
                }
                else
                {
                    Debug.LogError($"The prefab for {enemyData.name} does not have a NewEnemy component.");
                }
            }
        }
    }

    private void SpawnOldEnemy()
    {
        Vector3 spawnPosition = GridManager.Instance.GetTileWorldPosition(0, 0);

        if (oldEnemyPrefab != null)
        {
            Instantiate(oldEnemyPrefab, spawnPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Old enemy prefab not assigned.");
        }
    }

    private void HandleEnemyDestroyed(NewEnemy enemy)
    {
        if (enemiesRemaining <= 0) return;

        enemiesRemaining--;
        UIManager.Instance.AddMoney(50);

        if (enemiesRemaining <= 0 && enemiesSpawned == enemiesToSpawn)
        {
            OnWaveCompleted?.Invoke(currentWaveIndex);
            currentWaveIndex++;
            StartWave();
        }
    }

    private void HandleEnemyReachedEnd(NewEnemy enemy)
    {
        UIManager.Instance.ReducePlayerHealth(40);
    }
    private void HandleOldEnemyReachedEnd(OldEnemy enemy)
    {
        UIManager.Instance.ReducePlayerHealth(50);
    }

    private void OnEnable()
    {
        NewEnemy.OnEnemyDestroyed += HandleEnemyDestroyed;
        NewEnemy.OnEnemyReachedEnd += HandleEnemyReachedEnd;
        //OldEnemy.OnEnemyReachedEnd2 += HandleOldEnemyReachedEnd;
    }

    private void OnDisable()
    {
        NewEnemy.OnEnemyDestroyed -= HandleEnemyDestroyed;
        NewEnemy.OnEnemyReachedEnd -= HandleEnemyReachedEnd;
        //OldEnemy.OnEnemyReachedEnd2 += HandleOldEnemyReachedEnd;
    }
}

[System.Serializable]
public class Wave
{
    public bool useNewSystem;
    public List<EnemyData> enemiesToSpawn; 
    public int enemyCount; 
}