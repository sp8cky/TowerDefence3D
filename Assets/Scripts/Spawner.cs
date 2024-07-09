using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// spawns enemies at a given interval
public class Spawner : MonoBehaviour {
    public List<GameObject> enemyPrefabs;
    public bool isActivated = false; // Initially deactivated
    private bool isSpawning = false;
    public float spawnInterval = 3f;
    [SerializeField] private Material activeMaterial;
    [SerializeField] private Material inactiveMaterial;
    private static List<Spawner> allSpawners = new List<Spawner>();

    void Awake() {
        allSpawners.Add(this); // Add this spawner to the list of all spawners
    }

    void OnDestroy() {
        allSpawners.Remove(this); // Remove this spawner when it's destroyed
    }

    void Start() {
        ActivateRandomSpawners(2);
        ToggleSpawnMaterial(isActivated);
        if (isActivated && enemyPrefabs.Count > 0) {
            StartCoroutine(SpawnEnemies());
        }
    }

    IEnumerator SpawnEnemies() {
        while (isSpawning) {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public bool GetSpawningBool() { return isSpawning;}
    public void SetSpawningBool() { isSpawning = true;}

    void SpawnEnemy() {
        int randomIndex = Random.Range(0, enemyPrefabs.Count);
        GameObject enemyPrefab = enemyPrefabs[randomIndex];
        Instantiate(enemyPrefab, new Vector3(transform.position.x, 1.45f, transform.position.z), Quaternion.identity);
    }
    public static void FindAllSpawners() {
        allSpawners.Clear(); // Clear the list to avoid duplicates if called multiple times
        GameObject[] spawnerObjects = GameObject.FindGameObjectsWithTag("Spawn");
        foreach (GameObject spawnerObject in spawnerObjects) {
            Spawner spawner = spawnerObject.GetComponent<Spawner>();
            if (spawner != null) {
                allSpawners.Add(spawner);
            }
        }
    }

    void ToggleSpawnMaterial(bool isActive) {
        GetComponent<Renderer>().material = isActive ? activeMaterial : inactiveMaterial;
    }

    public static void ActivateRandomSpawners(int activeCount) {
        FindAllSpawners();

        // Deactivate all spawners first
        foreach (var spawner in allSpawners) {
            spawner.isActivated = false;
            spawner.ToggleSpawnMaterial(false);
        }

        // Shuffle the list and activate the first 'activeCount' spawners
        List<Spawner> shuffledSpawners = new List<Spawner>(allSpawners);
        for (int i = 0; i < shuffledSpawners.Count; i++) {
            Spawner temp = shuffledSpawners[i];
            int randomIndex = Random.Range(i, shuffledSpawners.Count);
            shuffledSpawners[i] = shuffledSpawners[randomIndex];
            shuffledSpawners[randomIndex] = temp;
        }

        for (int i = 0; i < activeCount && i < shuffledSpawners.Count; i++) {
            shuffledSpawners[i].isActivated = true;
            shuffledSpawners[i].ToggleSpawnMaterial(true);
            shuffledSpawners[i].StartCoroutine(shuffledSpawners[i].SpawnEnemies()); // Start spawning enemies
        }
    }
    public void StartEnemySpawn() {
        isSpawning = true;
        StartCoroutine(SpawnEnemies());
    }

    public void StopEnemySpawn() {
        isSpawning = false;
        DestroyAllEnemies();
    }
    
    // Destroy all enemies when timer is over
    private void DestroyAllEnemies() {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies) Destroy(enemy);
    }


}