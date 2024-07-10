using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour {
    public float attackRadius = 30f; 
    public float attackSpeed = 1f; // attacks per second
    public float damage = 10f; 
    public GameObject bulletPrefab;
    private Transform target; 
    private float attackCooldown = 0f; 
    

    void Update() {
        if (attackCooldown <= 0f) {
            FindNearestTarget();
            if (target != null) {
                StartCoroutine(Attack());
            }
        } else {
            attackCooldown -= Time.deltaTime;
        }
    }

    IEnumerator Attack() {
        attackCooldown = 1f / attackSpeed;
        GameObject bullet = Instantiate(bulletPrefab, transform.position + new Vector3(0, 2, 0), Quaternion.identity);
        TowerBulletController bulletController = bullet.GetComponent<TowerBulletController>();
        if (bulletController != null) {
            bulletController.SetTarget(target);
        }
        yield return null; // This line is not strictly necessary but can be used to ensure the coroutine properly yields.
    }

    void FindNearestTarget() {
        Collider[] colliders = Physics.OverlapSphere(transform.position, attackRadius);
        float shortestDistance = Mathf.Infinity;
        Transform nearestTarget = null;

        foreach (Collider collider in colliders) {
            if (collider.CompareTag("Enemy")) {
                float distanceToTarget = Vector3.Distance(transform.position, collider.transform.position);
                if (distanceToTarget < shortestDistance) {
                    shortestDistance = distanceToTarget;
                    nearestTarget = collider.transform;
                }
            }
        }

        target = nearestTarget;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}