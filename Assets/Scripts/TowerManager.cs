using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour {
    public List<GameObject> towerPrefabs; // list with tower prefabs
    private List<GameObject> towers = new List<GameObject>(); // list of placed towers

    // choose tower by index
    public void SelectTower(int index) {
        if (index >= 0 && index < towerPrefabs.Count) {
            GameObject selectedTowerPrefab = towerPrefabs[index];
            // place tower at current position
            GameObject newTower = Instantiate(selectedTowerPrefab, transform.position, Quaternion.identity);
            towers.Add(newTower); 
        } else {
            Debug.LogWarning("Index out of range for tower selection!");
        }
    }

    // start attacks
    public void StartAttackPhase() {
        StartCoroutine(AttackPhaseCoroutine());
    }

    // Coroutine for attacks
    IEnumerator AttackPhaseCoroutine() {
        // Endlosschleife für die Angriffsphase
        while (true) {
            // Aktualisiere jeden Turm
            foreach (GameObject tower in towers) {
                TowerController towerController = tower.GetComponent<TowerController>();
                if (towerController != null) {
                    towerController.Update(); // Rufe die Update-Methode des Turms auf
                }
            }
            yield return null; // Warte einen Frame, bevor der nächste Durchlauf beginnt
        }
    }
}