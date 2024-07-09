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

    private void Start() {
        spawnObject = GameObject.Find("Spawn");
        baseObject = GameObject.Find("Base");
        navMeshSurface = GetComponent<NavMeshSurface>();
        if (spawnObject == null || baseObject == null) {
            Debug.LogError("SpawnObject or BaseObject not found.");
        } else {
            InitializeSpawnAndBase();
            BakeNavMesh();
        }
    }

    // spawn and base get set to their initial positions
    void InitializeSpawnAndBase() {
        spawnObject.transform.position = spawnPosition;
        baseObject.transform.position = basePosition;
        Debug.Log("Spawn and Base initialized.");
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
