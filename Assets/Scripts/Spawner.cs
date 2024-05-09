using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spawner : MonoBehaviour {
    public GameObject enemyPrefab; // Prefab für die zu spawnende Kugel
    public float spawnInterval = 2f; // Intervall zwischen den Spawns
    public float enemySpeed = 5f; // Geschwindigkeit der Kugel

    IEnumerator Start() {
        while (true) {
            SpawnSphere();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnSphere() {
        GameObject sphere = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        Rigidbody rigidbody = sphere.GetComponent<Rigidbody>();
        if (rigidbody != null) {
            rigidbody.velocity = transform.forward * enemySpeed;
        } else {
            Debug.LogError("Rigidbody fehlt auf der Sphere.");
        }
    }

}