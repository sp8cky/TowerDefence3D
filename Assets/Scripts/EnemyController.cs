using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {
    
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Base")) {
            Debug.Log("Collision Enemy with Base.");
            Destroy(other.gameObject);
        }
    }
}
