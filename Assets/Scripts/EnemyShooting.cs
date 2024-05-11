using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        // stop targeting the player after the duration
        bullet.StopTargetingPlayer();
    }

    void Shoot() {
        Vector3 directionToPlayer = (playerController.transform.position - transform.position).normalized;

        // Erzeuge die Kugel am Gegner und übergebe die Richtung zum Spieler
        GameObject bulletGO = Instantiate(bulletPrefab, transform.position, Quaternion.LookRotation(directionToPlayer));
        EnemyBullet bullet = bulletGO.GetComponent<EnemyBullet>();

        if (bullet != null) {
            bullet.Initialize(playerController.transform, bulletDamage);
            StartCoroutine(KeepTargetDirection(bullet));
        }
    }
}
