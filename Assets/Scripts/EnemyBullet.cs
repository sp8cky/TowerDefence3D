using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour {
    public float bulletSpeed = 8f; 
    public int damage; 
    private Transform target; 
    private bool isTargetingPlayer = true; // check if bullet is targeting player
    private float targetingTimer = 0f; // timer for targeting duration
    private float targetingDuration = 2f; // time to target the player
    private bool isStraightBullet = false; // check if bullet should continue straight after targeting player

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

        if (isTargetingPlayer) {
            // Move the bullet towards the target
            transform.position = Vector3.MoveTowards(transform.position, target.position, bulletSpeed * Time.deltaTime);
            
            targetingTimer += Time.deltaTime; // Update targeting timer
            if (targetingTimer >= targetingDuration) StopTargetingPlayer();
        } else if (isStraightBullet) {
            // Move the bullet forward without targeting player
            transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);
        }
    }
    
    public void ChangeToStraightBullet() {
        isStraightBullet = true;
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Player")) {
            target.GetComponent<PlayerController>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    public void StopTargetingPlayer() {
        isTargetingPlayer = false;
    }
}
