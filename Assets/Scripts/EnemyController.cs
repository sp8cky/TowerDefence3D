using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {
    protected GenerateCubes generateCubes;
    protected int currentWaypointIndex = 0;
    protected NavMeshAgent agent;
    protected int enemyScore = 1;
    protected int enemyHealth = 1;

    protected virtual void Start() {
        generateCubes = FindObjectOfType<GenerateCubes>(); 
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false; // Prevent NavMeshAgent from automatically rotating the agent
        SetDestinationToNextWaypoint();
    }

    protected virtual void Update() {
        // Check if the agent has reached the current waypoint
        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending) {
            // Move to the next waypoint
            currentWaypointIndex++;
            if (currentWaypointIndex < generateCubes.waypoints.Length) {
                SetDestinationToNextWaypoint();
            } else {
                // Reached the last waypoint (Base), destroy the enemy
                Debug.Log("Enemy reached Base.");
                GameManager.instance.EnemyReachedBase(enemyScore);
                Destroy(gameObject);
            }
        }
        if (currentWaypointIndex < generateCubes.waypoints.Length) RotateTowards(generateCubes.waypoints[currentWaypointIndex].position);
    
    }

    protected virtual void SetDestinationToNextWaypoint() {
        // Check if there are waypoints remaining
        if (currentWaypointIndex < generateCubes.waypoints.Length) {
            agent.SetDestination(generateCubes.waypoints[currentWaypointIndex].position);
        }
    }

    // TODO: Fix wrong look rotation of the enemy
    protected virtual void RotateTowards(Vector3 target) {
        Vector3 direction = (target - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    public void TakeDamage(int damage) {
        enemyHealth -= damage;
        if (enemyHealth <= 0) {
            Destroy(gameObject);
            Debug.Log("Enemy destroyed.");
            GameManager.instance.AddScore(enemyScore);
        }
    }
}
