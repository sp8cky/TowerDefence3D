using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// class for the enemy bullets with initialization, seeking and hitting target
public class EnemyBullet : MonoBehaviour {
    public float speed = 10f; 
    public int damage = 5; 
    private Transform target; 
    private bool isTargetingPlayer = true; // check if bullet is targeting player
    private float targetingTimer = 0f; // timer for targeting duration
    private float targetingDuration = 2f; // time to target the player

    public void Initialize(Transform _target, int _damage) {
        target = _target;
        damage = _damage;
    }

    public void Seek(Transform _target) {
        target = _target;
    }

    void Update() {
        if (target == null) {
            Destroy(gameObject);
            return;
        }

        // Check if the bullet should still target the player
        if (isTargetingPlayer) {
            // Move the bullet towards the target
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            
            targetingTimer += Time.deltaTime; // Update targeting timer
            if (targetingTimer >= targetingDuration) StopTargetingPlayer();
        }
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Player")) {
            //Debug.Log("EnemyBullet hits target: " + target.name);
            target.GetComponent<PlayerController>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    public void StopTargetingPlayer() {
        isTargetingPlayer = false;
    }
}