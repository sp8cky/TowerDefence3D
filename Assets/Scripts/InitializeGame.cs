using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class InitializeGame : MonoBehaviour {
    private List<GameObject> spawnList = new List<GameObject>();
    private List<GameObject> activeSpawns = new List<GameObject>();
    private GameObject baseObject;
    private Vector3 basePosition = new Vector3(0, 0.5f, 0);
    [SerializeField] private Material activeSpawnMaterial;
    [SerializeField] private Material inactiveSpawnMaterial;
    private NavMeshSurface navMeshSurface;
    public GameObject obstaclePrefab1; // Prefab für das erste Hindernis (2x2x3)
    public GameObject obstaclePrefab2; // Prefab für das zweite Hindernis (1x1x15)
    public GameObject obstaclePrefab3; // Prefab für das dritte Hindernis (1x1x20)
    private int obstacleNumber = 2; // Anzahl der Hindernisse, die gespawnt werden sollen

    private void Start() {
        baseObject = GameObject.Find("Base");
        navMeshSurface = GetComponent<NavMeshSurface>();
        FindAllSpawns();

        if (spawnList.Count == 0) Debug.LogError("No spawns found.");

        if (baseObject == null) {
            Debug.LogError("BaseObject not found.");
        } else {
            InitializeSpawnAndBase();
            //SpawnObstacles(); // TODO: check the obstacles spawn code
            BakeNavMesh();
        }
    }

    // find all spawns under the parent "Spawns"
    void FindAllSpawns() {
        GameObject spawnParent = GameObject.Find("Spawns");
        if (spawnParent == null) {
            Debug.LogError("Parent object 'Spawns' not found.");
            return;
        }

        foreach (Transform spawn in spawnParent.transform) {
            if (spawn.CompareTag("Spawn")) spawnList.Add(spawn.gameObject);
        }
        Debug.Log("Found " + spawnList.Count + " spawns.");
    }

    // set base position
    void InitializeSpawnAndBase() {
        baseObject.transform.position = basePosition;
        Debug.Log("Base and Spawns initialized");
    }

    // bakes the NavMesh
    void BakeNavMesh() {
        if (navMeshSurface == null) {
            Debug.LogError("NavMeshSurface component not found on the ground object.");
        } else {
            navMeshSurface.BuildNavMesh();
            Debug.Log("NavMesh baked.");
        }
    }
    // Activate/Deactivate spawns
    public void ActivateSpawns(int activeSpawnCount) {
        activeSpawns.Clear();

        if (spawnList.Count == 0) {
            Debug.LogError("SpawnList is empty or not assigned.");
            return;
        }

        // Deactivate all spawns
        foreach (GameObject spawn in spawnList) ToggleSpawnMaterial(spawn, false);

        // Activate random spawns
        List<int> indices = new List<int>();
        for (int i = 0; i < spawnList.Count; i++) indices.Add(i);

        for (int i = 0; i < activeSpawnCount; i++) {
            if (indices.Count == 0) break;

            int randomIndex = Random.Range(0, indices.Count);
            int spawnIndex = indices[randomIndex];
            indices.RemoveAt(randomIndex);

            ToggleSpawnMaterial(spawnList[spawnIndex], true);
            activeSpawns.Add(spawnList[spawnIndex]);
        }
    }

    // Change material of spawn
    void ToggleSpawnMaterial(GameObject spawn, bool isActive) {
        Renderer renderer = spawn.GetComponent<Renderer>();
        if (renderer != null) {
            renderer.material = isActive ? activeSpawnMaterial : inactiveSpawnMaterial;
        }
    }


    // OBSTACLE SPAWNER ///////////////////////////////////////////
    // spawns obstacles on the map
    public void SpawnObstacles() {
        int spawnedObstacles = 0;

        // Spawn Prefab 1 (3x2x2)
        for (int i = 0; i < obstacleNumber; i++) {
            Vector3 spawnPosition1 = GetRandomWholeCoordinatePosition();
            if (spawnPosition1 != Vector3.zero) {
                Quaternion spawnRotation1 = Quaternion.Euler(0f, 90f * Random.Range(0, 2), 0f); // random rotation on the Y-axis
                Instantiate(obstaclePrefab1, spawnPosition1, spawnRotation1);
                Debug.Log("Obstacle spawned at " + spawnPosition1);
                spawnedObstacles++;
            } else {
                Debug.LogWarning("No valid position found for obstacle 1.");
                break;
            }
        }

        // Spawn Prefab 2 (1x2x15)
        for (int j = 0; j < obstacleNumber; j++) {
            Vector3 spawnPosition2 = GetRandomWholeCoordinatePosition();
            if (spawnPosition2 != Vector3.zero) {
                Quaternion spawnRotation2 = Quaternion.Euler(0f, 90f * Random.Range(0, 2), 0f); // random rotation on the Y-axis
                Instantiate(obstaclePrefab2, spawnPosition2, spawnRotation2);
                Debug.Log("Obstacle spawned at " + spawnPosition2);
                spawnedObstacles++;
            } else  {
                Debug.LogWarning("No valid position found for obstacle 2.");
                break;
            }
        }

        // Spawn Prefab 3 (1x2x20)
        for (int k = 0; k < obstacleNumber; k++)  {
            Vector3 spawnPosition3 = GetRandomWholeCoordinatePosition();
            if (spawnPosition3 != Vector3.zero) {
                Quaternion spawnRotation3 = Quaternion.Euler(0f, 90f * Random.Range(0, 2), 0f); // random rotation on the Y-axis
                Instantiate(obstaclePrefab3, spawnPosition3, spawnRotation3);
                Debug.Log("Obstacle spawned at " + spawnPosition3);
                spawnedObstacles++;
            } else {
                Debug.LogWarning("No valid position found for obstacle 3.");
                break;
            }
        }

        if (spawnedObstacles < 3 * obstacleNumber) {
            Debug.LogWarning("Not enough valid positions found for obstacles. Only " + spawnedObstacles + " obstacles spawned.");
        }

        Debug.Log("Obstacles spawned.");
    }

    Vector3 GetRandomWholeCoordinatePosition() {
        Vector3 spawnPosition;
        float x, z;
        int maxAttempts = 20;
        int attempts = 0;

        do {
            x = Mathf.Round(Random.Range(-32f, 32f));
            z = Mathf.Round(Random.Range(-32f, 32f));
            spawnPosition = new Vector3(x, 1.5f, z);
            attempts++;
        } while ((IsNearBase(spawnPosition) || IsTooCloseToExistingObstacles(spawnPosition)) && attempts < maxAttempts);

        if (attempts >= maxAttempts) {
            Debug.LogWarning("Failed to find valid position after " + maxAttempts + " attempts.");
            return Vector3.zero;
        }

        return spawnPosition;
    }

    bool IsNearBase(Vector3 position) {
        Vector3 basePos = baseObject.transform.position;

        // Check if too close to base (or adjacent fields)
        if (Mathf.Abs(position.x - basePos.x) <= 1f && Mathf.Abs(position.z - basePos.z) <= 1f) return true;

        return false;
    }

    bool IsTooCloseToExistingObstacles(Vector3 newPosition) {
        foreach (GameObject obstacle in GameObject.FindGameObjectsWithTag("Obstacle")) {
            Vector3 obstaclePosition = obstacle.transform.position;
            if (Mathf.Abs(newPosition.x - obstaclePosition.x) < 2f && Mathf.Abs(newPosition.z - obstaclePosition.z) < 2f) return true;
        }
        return false;
    }

    // GETTER AND SETTER ///////////////////////////////////////////
    // get the list of spawns
    public List<GameObject> GetSpawnList() { return spawnList; }

    // Get the list of active spawns
    public List<GameObject> GetActiveSpawns() { return activeSpawns; }
}
