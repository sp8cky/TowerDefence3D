using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {
    public float speed = 10f; 
    private Transform target; 
    private int damage;

    public void Initialize(Transform _target, int _damage) {
        target = _target;
        damage = _damage;
    }

    public void Seek(Transform _target) {
        target = _target;
    }

    void Update() {
        // Wenn das Ziel nicht mehr existiert, zerstöre die Kugel
        if (target == null) {
            Destroy(gameObject);
            Debug.Log("Bullet destroyed.");
            return;
        }

        // calculate direction to target
        Vector3 direction = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        // damage by hit
        if (direction.magnitude <= distanceThisFrame) {
            HitTarget();
            return;
        }

        // move bullet towards target
        transform.Translate(direction.normalized * distanceThisFrame, Space.World);
    }

    void HitTarget() {
        Debug.Log("Bullet hits target: " + target.name);
        EnemyController enemyController = target.GetComponent<EnemyController>();
        if (enemyController != null) enemyController.TakeDamage(damage);
        Destroy(gameObject);
    }
}
