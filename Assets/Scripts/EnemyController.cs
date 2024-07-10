using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// enemy class with navmesh pathfinding
public class EnemyController : MonoBehaviour {
    private Transform basePoint;
    private NavMeshAgent navMeshAgent;

    void Start() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        GameObject baseObject = GameObject.Find("Base");

        // set base point
        basePoint = baseObject.transform;

        if (navMeshAgent == null) {
            Debug.LogError("NavMeshAgent component not found on this GameObject");
            return;
        }

        if (basePoint == null) {
            Debug.LogError("Base object not found in the scene.");
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
        //Debug.Log("Enemy reached the base!");
        Destroy(gameObject);
    }
}
