using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {
    private Transform spawnPoint;
    private Transform basePoint;
    private NavMeshAgent navMeshAgent;

    void Start() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        GameObject spawnObject = GameObject.Find("Spawn");
        GameObject baseObject = GameObject.Find("Base");
        
        // set positions
        spawnPoint = spawnObject.transform;
        basePoint = baseObject.transform;

        if (navMeshAgent == null) {
            Debug.LogError("NavMeshAgent component not found on this GameObject");
            return;
        }

        if (spawnPoint == null || basePoint == null) {
            Debug.LogError("Spawn or Base object not found in the scene.");
            return;
        }

        if (navMeshAgent == null) {
            Debug.LogError("NavMeshAgent component not found on this GameObject.");
            return;
        }

        // set destination
        SetDestination(basePoint.position);
    }


    void Update() {
        // Check if the agent has reached the base
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance) {
            // Enemy has reached the base
            OnReachBase();
        }
    }
    
    // set destination for the enemy
    private void SetDestination(Vector3 destination) {
        navMeshAgent.SetDestination(destination);
    }

    // enemy reached the base
    private void OnReachBase() {
        Debug.Log("Enemy reached the base!");
        Destroy(gameObject);
    }
    
}