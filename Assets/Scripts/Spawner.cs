using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spawner : MonoBehaviour {
    public GameObject enemyPrefab; // Prefab für die zu spawnende Kugel
    public float spawnInterval = 10f; // Intervall zwischen den Spawns

    IEnumerator Start() {
        while (true) {
            SpawnSphere();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnSphere() {
        Instantiate(enemyPrefab, transform.position, Quaternion.identity);
    }

}