using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// spawns enemies at a given interval
public class Spawner : MonoBehaviour {
    public List<GameObject> enemyPrefabs;
    public float spawnInterval = 3f; 

    void Start() {
        if (enemyPrefabs.Count == 0) {
            Debug.LogError("No enemy prefabs assigned to the spawner.");
            return;
        }
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies() {
        while (true) {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnEnemy() {
        int randomIndex = Random.Range(0, enemyPrefabs.Count);
        GameObject enemyPrefab = enemyPrefabs[randomIndex];
        Instantiate(enemyPrefab, new Vector3(transform.position.x, 1.45f ,transform.position.z), Quaternion.identity);
        Debug.Log("Spawned enemy.");
    }
}