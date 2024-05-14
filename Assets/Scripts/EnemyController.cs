using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Parent class for all enemies with waypoints, movementand taking damage
public class EnemyController : MonoBehaviour {
    protected GenerateCubes generateCubes;
    protected int currentWaypointIndex = 0;
    protected NavMeshAgent agent;
    protected int enemyScore = 1;
    protected int enemyHealth = 1;

    protected virtual void Start() {
        generateCubes = FindObjectOfType<GenerateCubes>(); 
        agent = GetComponent<NavMeshAgent>();
        SetDestinationToNextWaypoint();
    }

    protected virtual void Update() {
        // Check if the agent has reached the current waypoint
        if (agent.remainingDistance <= agent.stoppingDistance) {
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
    }

    protected virtual void SetDestinationToNextWaypoint() {
        // Check if there are waypoints remaining
        if (currentWaypointIndex < generateCubes.waypoints.Length) agent.SetDestination(generateCubes.waypoints[currentWaypointIndex].position);
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
