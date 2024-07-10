using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// spawns enemies at a given interval
public class Spawner : MonoBehaviour {
    public List<GameObject> enemyPrefabs;
    private bool isSpawning = false;
    public float spawnInterval = 3f;
    private InitializeGame initializeGame;

    void Start() {
        initializeGame = FindObjectOfType<InitializeGame>();

        if (enemyPrefabs.Count > 0) StartCoroutine(SpawnEnemies());
        
    }

    IEnumerator SpawnEnemies() {
        while (isSpawning) {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public bool GetSpawningBool() { return isSpawning; }
    public void SetSpawningBool() { isSpawning = true; }

    void SpawnEnemy() {
        int randomIndex = Random.Range(0, enemyPrefabs.Count);
        GameObject enemyPrefab = enemyPrefabs[randomIndex];

        List<GameObject> activeSpawns = initializeGame.GetActiveSpawns();
        if (activeSpawns.Count > 0) {
            int randomSpawnIndex = Random.Range(0, activeSpawns.Count);
            GameObject spawnPoint = activeSpawns[randomSpawnIndex];
            Instantiate(enemyPrefab, spawnPoint.transform.position, Quaternion.identity);
        }
    }

    public void StartEnemySpawn() {
        isSpawning = true;
        StartCoroutine(SpawnEnemies());
    }

    public void StopEnemySpawn() {
        isSpawning = false;
        DestroyAllEnemies();
    }
    
    // Destroy all enemies when timer is over
    private void DestroyAllEnemies() {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies) Destroy(enemy);
    }
}
