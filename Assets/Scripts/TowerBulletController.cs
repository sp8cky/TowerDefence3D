using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBulletController : MonoBehaviour {
    public float speed = 0.5f;
    public float damage = 10f;
    private Transform target;

    public void SetTarget(Transform _target) {
        target = _target;
    }

    void Update() {
        if (target == null) {
            Destroy(gameObject); // Destroy bullet if target is null
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        // Check if bullet hit the target
        if (dir.magnitude <= distanceThisFrame) {
            HitTarget();
            return;
        }

        // Move bullet towards the target
        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.LookAt(target);
    }

    void HitTarget() {
        EnemyController enemy = target.GetComponent<EnemyController>();
        enemy.TakeDamage(damage);
        Debug.Log("Hit target!");
        Destroy(gameObject); // Destroy bullet after hitting the target
    }
}