using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHard : EnemyController {
    protected override void Start() {
        base.Start();
        enemyScore = 10;
        enemyHealth = 10;
    }
}
