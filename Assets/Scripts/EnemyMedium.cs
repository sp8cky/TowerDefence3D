using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Child class for the medium enemy 
public class EnemyMedium : EnemyController {
    protected override void Start() {
        base.Start();
        enemyScore = 5;
        enemyHealth = 5;
    }
}
