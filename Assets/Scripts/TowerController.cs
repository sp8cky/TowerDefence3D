using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Parent class for all towers with attack radius, cooldown and target enemy
public class TowerController : MonoBehaviour {
    public float attackRadius = 5f; 
    public float attackCooldown = 1f; // cooldown between attacks
    public LayerMask enemyLayer; // enemy layer
    private Transform targetEnemy; // current target enemy
    private float lastAttackTime; // last attack time
    public GameObject bulletPrefab;
    public int damage = 1;

    void Update() {
        // find enemy in radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, attackRadius, enemyLayer);

        // choose next enemy
        targetEnemy = FindNearestEnemy(colliders);

        // attack enemy
        if (targetEnemy != null && Time.time - lastAttackTime >= attackCooldown) {
            AttackEnemy(targetEnemy);
            lastAttackTime = Time.time;
        }
    }

    void AttackEnemy(Transform enemy) {
        //Debug.Log("Tower attacks enemy: " + enemy.name);
        GameObject bulletGO = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        TowerBulletController bullet = bulletGO.GetComponent<TowerBulletController>();

        if (bullet != null) bullet.Initialize(enemy, damage);
    }

    Transform FindNearestEnemy(Collider[] colliders) {
        Transform nearestEnemy = null;
        float shortestDistance = Mathf.Infinity;

        foreach (Collider collider in colliders) {
            float distanceToEnemy = Vector3.Distance(transform.position, collider.transform.position);
            if (distanceToEnemy < shortestDistance) {
                shortestDistance = distanceToEnemy;
                nearestEnemy = collider.transform;
            }
        }
        return nearestEnemy;
    }

    // show attack radius in editor
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}