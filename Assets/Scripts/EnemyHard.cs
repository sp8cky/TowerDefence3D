using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Child class for the hard enemy 
public class EnemyHard : EnemyController {
    protected override void Start() {
        base.Start();
        enemyScore = 10;
        enemyHealth = 10;
    }
}
