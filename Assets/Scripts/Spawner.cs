using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spawner : MonoBehaviour {
    public List<GameObject> enemyPrefabs; 
    public float spawnInterval = 10f; // interval between spawns

    IEnumerator Start() {
        while (true) {
            SpawnSphere();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnSphere() {
        int randomIndex = Random.Range(0, enemyPrefabs.Count);
        GameObject randomPrefab = enemyPrefabs[randomIndex];
        Instantiate(randomPrefab, transform.position + new Vector3(0f, 1.0f, 0f), Quaternion.identity);
    }

}