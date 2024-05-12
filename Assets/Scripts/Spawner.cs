using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    public List<GameObject> enemyPrefabs; 
    public float spawnInterval = 10f; // interval between spawns
    private bool spawningEnabled = false;

    private void DestroyAllEnemies() {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies) Destroy(enemy);
    }

    // Coroutine zum Spawnen von Gegnern in regelmäßigen Intervallen
    IEnumerator SpawnEnemies() {
        while (spawningEnabled) {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnEnemy() {
        int randomIndex = Random.Range(0, enemyPrefabs.Count);
        GameObject randomPrefab = enemyPrefabs[randomIndex];
        Instantiate(randomPrefab, transform.position + new Vector3(0f, 1.0f, 0f), Quaternion.identity);
    }

    public void StartEnemySpawn() {
        spawningEnabled = true;
        StartCoroutine(SpawnEnemies());
    }

    public void StopEnemySpawn() {
        spawningEnabled = false;
        DestroyAllEnemies();
    }

}