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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
