using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {
    
    protected GenerateCubes generateCubes; // Reference to the GenerateCubes script
    protected int currentWaypointIndex = 0;
    protected NavMeshAgent agent;
    protected GameManager gameManager;
    protected int enemyScore = 1;
    protected int enemyHealth = 1;

    protected virtual void Start() {
        generateCubes = GameObject.FindObjectOfType<GenerateCubes>(); // Find the GenerateCubes script
        gameManager = FindObjectOfType<GameManager>();
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
                if (gameManager != null) gameManager.EnemyReachedBase(enemyScore);
                Destroy(gameObject);
            }
        }
    }

    protected virtual void SetDestinationToNextWaypoint() {
        // Check if there are waypoints remaining
        if (currentWaypointIndex < generateCubes.waypoints.Length) agent.SetDestination(generateCubes.waypoints[currentWaypointIndex].position);
    }
    public void TakeDamage(int damageAmount) {
        enemyHealth -= damageAmount;
        if (enemyHealth <= 0) {
            Destroy(gameObject);
            Debug.Log("Enemy destroyed.");
            if (gameManager != null) gameManager.AddScore(enemyScore);
        }
    }
}
