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

    private void Start() {
        baseObject = GameObject.Find("Base");
        navMeshSurface = GetComponent<NavMeshSurface>();
        FindAllSpawns();

        if (spawnList.Count == 0) Debug.LogError("No spawns found.");
        
        if (baseObject == null) {
            Debug.LogError("BaseObject not found.");
        } else {
            InitializeSpawnAndBase();
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



    // GETTER AND SETTER ///////////////////////////////////////////
    // get the list of spawns
    public List<GameObject> GetSpawnList() { return spawnList; }

    // Get the list of active spawns
    public List<GameObject> GetActiveSpawns() { return activeSpawns; }
}
