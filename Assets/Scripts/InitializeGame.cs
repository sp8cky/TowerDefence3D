using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

// initialize the game with the spawn and base positions
public class InitializeGame : MonoBehaviour {
    private GameObject spawnObject;
    private GameObject baseObject;
    private Vector3 spawnPosition = new Vector3(-37, 0.5f, -37);
    private Vector3 basePosition = new Vector3(0, 0.5f, 0);
    private NavMeshSurface navMeshSurface;
    public GameObject obstaclePrefab;
    private int numberOfObstacles = 3;
    public Vector2 obstacleSizeRange = new Vector2(1, 5);

    private void Start() {
        spawnObject = GameObject.Find("Spawn");
        baseObject = GameObject.Find("Base");
        navMeshSurface = GetComponent<NavMeshSurface>();
        if (spawnObject == null || baseObject == null) {
            Debug.LogError("SpawnObject or BaseObject not found.");
        } else {
            InitializeSpawnAndBase();
            SpawnObstacles();
            BakeNavMesh();
        }
    }

    // spawn and base get set to their initial positions
    void InitializeSpawnAndBase() {
        spawnObject.transform.position = spawnPosition;
        baseObject.transform.position = basePosition;
        Debug.Log("Spawn and Base initialized.");
    }

    // Spawns obstacles randomly on the ground
    void SpawnObstacles() {
        List<Vector3> obstaclePositions = new List<Vector3>();

        for (int i = 0; i < numberOfObstacles; i++) {
            Vector3 randomPosition;
            bool positionValid;

            do {
                positionValid = true;
                float randomX = Random.Range(-35, 35);
                float randomZ = Random.Range(-35, 35);
                randomPosition = new Vector3(randomX, 0.5f, randomZ);

                // Check if the position is too close to the spawn or base
                if (Vector3.Distance(randomPosition, spawnPosition) < 2 || Vector3.Distance(randomPosition, basePosition) < 2) {
                    positionValid = false;
                    continue;
                }

                // Check if the position is too close to other obstacles
                foreach (Vector3 pos in obstaclePositions) {
                    if (Vector3.Distance(randomPosition, pos) < 2) {
                        positionValid = false;
                        break;
                    }
                }

            } while (!positionValid);

            obstaclePositions.Add(randomPosition);

            // Spawn the obstacle with random rotation
            Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0, 2) * 90, 0);
            GameObject obstacle = Instantiate(obstaclePrefab, randomPosition, randomRotation);

            // Set obstacle size
            obstacle.transform.localScale = new Vector3(1, 2, 1);
        }
        Debug.Log("Obstacles spawned.");
    }

    // Bakes the NavMesh
    void BakeNavMesh() {
        if (navMeshSurface == null) {
            Debug.LogError("NavMeshSurface component not found on the ground object.");
        } else {
            navMeshSurface.BuildNavMesh();
            Debug.Log("NavMesh baked.");
        }
    }
}
