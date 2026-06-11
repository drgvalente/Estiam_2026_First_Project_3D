using System.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Enemy Spawning Settings")]
    public GameObject enemyPrefab; // the enemy prefab to be instantiated
    public Transform[] spawnPoints; // Array with the places where they can spawn
    public float timeBetweenSpawns = 5f; // seconds between spawnings
    public int maxEnemies = 20; // max enemies in level

    private int currentEnemyCount = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentEnemyCount = FindObjectsByType<Enemy>().Length;
        StartCoroutine(SpawnRoutine());
    }

    // Coroutine: function that "pauses" it's execution for some time
    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            // wait (pauses) the time in timeBetweenSpawns
            yield return new WaitForSeconds(timeBetweenSpawns);

            // Instantiate (create) an enemy IF the maxEnemies is not reached
            if (currentEnemyCount < maxEnemies)
            {
                SpawnEnemy();
            }
        }
    }

    private void SpawnEnemy()
    {
        // 1. Choose a random spawn point in the array
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform chosenPoint = spawnPoints[randomIndex];

        // 2. Instantiate the enemy at chosen position and rotation
        Instantiate(enemyPrefab, chosenPoint.position, chosenPoint.rotation);

        // 3. Increments the counter
        currentEnemyCount++;
    }

    public void EnemyDied()
    {
        currentEnemyCount--;
    }

}
