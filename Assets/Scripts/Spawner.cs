using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// spawns enemies at a given interval
public class Spawner : MonoBehaviour {
    private List<GameObject> enemyPrefabs = new List<GameObject>();
    private bool isSpawning = false;
    public float spawnInterval = 0.2f;
    private InitializeGame initializeGame;

    void Start() {
        initializeGame = FindObjectOfType<InitializeGame>();

        LoadEnemyPrefabsFromFolder("Assets/Prefabs/Enemies");

        if (enemyPrefabs.Count > 0) StartCoroutine(SpawnEnemies());
    }

    void LoadEnemyPrefabsFromFolder(string folderPath) {
        enemyPrefabs.Clear();

        string[] guids = AssetDatabase.FindAssets("t:Prefab", new string[] { folderPath });
        foreach (string guid in guids) {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
            if (prefab != null && prefab.CompareTag("Enemy")) {
                enemyPrefabs.Add(prefab);
            }
        }
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
        if (enemyPrefabs.Count > 0) {
            int randomIndex = Random.Range(0, enemyPrefabs.Count);
            GameObject enemyPrefab = enemyPrefabs[randomIndex];

            List<GameObject> activeSpawns = initializeGame.GetActiveSpawns();
            if (activeSpawns.Count > 0) {
                int randomSpawnIndex = Random.Range(0, activeSpawns.Count);
                GameObject spawnPoint = activeSpawns[randomSpawnIndex];
                Instantiate(enemyPrefab, spawnPoint.transform.position, Quaternion.identity);
            }
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
