using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Child class for the shooting enemy with bullets targeting the player
public class EnemyShooting : EnemyController {
    public GameObject bulletPrefab; 
    private PlayerController playerController;
    public float attackSpeed = 1f; 
    public float attackRange = 20f;
    public int bulletDamage = 5;
    public float bulletTargetDuration = 2f; // time to seek target
    private float nextFireTime; 
    protected override void Start() {
        playerController = FindObjectOfType<PlayerController>();
        base.Start();
        enemyScore = 10;
        enemyHealth = 5;
    }

    protected override void Update() {
        base.Update();
        // fire after delay is over
        if (Vector3.Distance(transform.position, playerController.transform.position) <= attackRange && Time.time >= nextFireTime) {
            Shoot();
            nextFireTime = Time.time + 1f / attackSpeed;
        }
    }
    IEnumerator KeepTargetDirection(EnemyBullet bullet) {
        yield return new WaitForSeconds(bulletTargetDuration);
        bullet.StopTargetingPlayer(); // stop targeting the player after the duration
    }

    void Shoot() {
        Vector3 directionToPlayer = (playerController.transform.position - transform.position).normalized;
        
        // create bullet at enemy and pass direction to player
        GameObject bulletGO = Instantiate(bulletPrefab, transform.position, Quaternion.LookRotation(directionToPlayer));
        EnemyBullet bullet = bulletGO.GetComponent<EnemyBullet>();

        if (bullet != null) {
            bullet.Initialize(playerController.transform, bulletDamage);
            StartCoroutine(KeepTargetDirection(bullet));
        }
    }
}
