using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : EnemyController {
    public GameObject bulletPrefab; 
    private PlayerController playerController;
    public float attackRange = 15f;
    public int bulletDamage = 5;
    public float bulletTargetDuration = 2f; // time to seek target
    private float nextFireTime; // time for next attack
    private float attackCooldown = 10f; // cooldown between attacks
    protected override void Start() {
        playerController = FindObjectOfType<PlayerController>();
        base.Start();
        enemyScore = 10;
        enemyHealth = 5;
    }

    protected override void Update() {
        base.Update();
        
        // fire after delay is over and cooldown is passed
        if (Vector3.Distance(transform.position, playerController.transform.position) <= attackRange && Time.time >= nextFireTime) {
            Shoot();
            nextFireTime = Time.time + attackCooldown;
        }
    }

    IEnumerator KeepTargetDirection(EnemyBullet bullet) {
        yield return new WaitForSeconds(bulletTargetDuration);
        bullet.StopTargetingPlayer(); // stop targeting the player after the duration
        bullet.ChangeToStraightBullet();
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
