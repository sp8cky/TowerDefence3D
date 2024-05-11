using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateCubes : MonoBehaviour {
    public GameObject cubePrefab; // Prefab for the cubes
    private Vector2 gridSize = new Vector2(50, 50); // Grid size
    public float height = 0.1f; // Height of the cubes
    public Material pathMaterial; // Material for cubes on the path
    public Material transparentMaterial; // Material for cubes not on the path
    public Transform[] waypoints; // Waypoints for the path, included in scene
    private GameObject spawnObject;
    private GameObject baseObject;

    void Start() {
        spawnObject = GameObject.FindGameObjectWithTag("Spawn");
        baseObject = GameObject.FindGameObjectWithTag("Base");
        //GenerateRandomPath(2, 0.0f, 49.0f);
        if (spawnObject != null && baseObject != null) {
            GenerateCubesGrid();

            // Definiere manuelle Positionen
            Vector3[] manualPositions = new Vector3[5];
            manualPositions[0] = new Vector3(7.5f, 1.5f, 2.5f);
            manualPositions[1] = new Vector3(7.5f, 1.5f, 13.5f);
            manualPositions[2] = new Vector3(20.5f, 1.5f, 13.5f);
            manualPositions[3] = new Vector3(20.5f, 1.5f, 40.5f);
            manualPositions[4] = new Vector3(30.5f, 1.5f, 40.5f);

            // Erstelle das Array mit den Wegpunkt-Positionen
            CreatePathPositionsArray(manualPositions);

            // color all path cubes
            CalculatePathCubes();
        } else {
            Debug.LogError("Spawn or Base object not found.");
        }
    }

    // adds spawn and base to waypoints
    void AddSpawnAndBaseAsWaypoints() {
        List<Transform> updatedWaypoints = new List<Transform>(waypoints);
        updatedWaypoints.Insert(0, spawnObject.transform);
        updatedWaypoints.Add(baseObject.transform);
        waypoints = updatedWaypoints.ToArray();
    }

    // calculates the whole path between spawn and base
    public void CalculatePathCubes() {
        for (int i = 0; i < waypoints.Length - 1; i++) {
            Transform currentWaypoint = waypoints[i]; // get current and nect waypoint
            Transform nextWaypoint = waypoints[i + 1];

            Vector3 roundedPosition = RoundPosition(currentWaypoint.position);
            ColorCubeAtPosition(roundedPosition); // color the current cube

            Vector3 direction = nextWaypoint.position - currentWaypoint.position; //calculate the direction to next waypoint
            float step = 1.0f / Mathf.Max(Mathf.Abs(direction.x), Mathf.Abs(direction.z)); // step size

            for (float t = 0; t <= 1; t += step) { // color every cube on the line between the waypoints
                Vector3 positionOnLine = Vector3.Lerp(currentWaypoint.position, nextWaypoint.position, t);
                Vector3 roundedPositionOnLine = RoundPosition(positionOnLine);
                ColorCubeAtPosition(roundedPositionOnLine);
            }
        }
    }
    
    // colors the current cube at the given position in the path
    private void ColorCubeAtPosition(Vector3 position) {
        // Check if the position is within the grid bounds
        if (position.x >= 0 && position.x < gridSize.x && position.z >= 0 && position.z < gridSize.y) {
            string cubeName = "Cube " + position.x.ToString("00") + "-" + position.z.ToString("00");
            GameObject cube = GameObject.Find(cubeName);
            if (cube != null) {
                cube.tag = "Path";
                Renderer renderer = cube.GetComponent<Renderer>();
                if (renderer != null) renderer.material = pathMaterial;
            }
        }
    }

    // creates an array of path positions from manual positions
    public void CreatePathPositionsArray(Vector3[] manualPositions) {
        GameObject waypointsParent = new GameObject("Waypoints"); //parent
        List<Transform> updatedWaypoints = new List<Transform>(waypoints);

        for (int i = 0; i < manualPositions.Length; i++) {
            GameObject waypointObject = new GameObject("WP " + (i + 1).ToString("00"));
            waypointObject.transform.position = manualPositions[i];
            
            waypointObject.transform.parent = waypointsParent.transform;
            updatedWaypoints.Add(waypointObject.transform);
        }
        waypoints = updatedWaypoints.ToArray();
        AddSpawnAndBaseAsWaypoints();

        Debug.Log("Anzahl der Waypoints: " + waypoints.Length);
    }

    // generates a grid of X x Y cubes with tag "Ground" on the ground, organized in rows
    void GenerateCubesGrid() {
        for (float x = 0.5f; x < gridSize.x; x++) {
            // Create an empty GameObject as a parent for the current row
            GameObject rowParent = new GameObject("Row " + (x + 0.5f));
            rowParent.transform.parent = transform; // Set the ground object as the parent

            for (float z = 0.5f; z < gridSize.y; z++) {
                Vector3 position = new Vector3(x, 1f + (height / 2), z);
                GameObject cube = Instantiate(cubePrefab, position, Quaternion.identity);
                cube.name = "Cube " + (x + 0.5f).ToString("00") + "-" + (z + 0.5f).ToString("00");
                Renderer renderer = cube.GetComponent<Renderer>();
                if (renderer != null) renderer.material = transparentMaterial;
                cube.tag = "Ground";
                cube.transform.parent = rowParent.transform;
            }
        }
        Debug.Log("Cubes successfully generated");
    }

    // generates rabdom path TODO: fix me
    public void GenerateRandomPath(int numWaypoints, float minBounds, float maxBounds) {
        List<Transform> updatedWaypoints = new List<Transform>();

        // Generate random spawn position on the first row
        float randomZSpawn = Random.Range(minBounds + 0.5f, maxBounds + 0.5f);
        Vector3 randomSpawnPosition = new Vector3(0.5f, 1.5f, randomZSpawn);
        spawnObject.transform.position = randomSpawnPosition;
        updatedWaypoints.Add(spawnObject.transform); // Add spawn as the first waypoint


        // Generate random base position on the last row
        float randomZBase = Random.Range(minBounds + 0.5f, maxBounds + 0.5f);
        Vector3 randomBasePosition = new Vector3(25.5f, 1.5f, randomZBase);
        baseObject.transform.position = randomBasePosition;
        updatedWaypoints.Add(baseObject.transform); // Add base as the last waypoint

        // Update waypoints array
        waypoints = updatedWaypoints.ToArray();
        Debug.Log("Anzahl der Waypoints: " + waypoints.Length);
    }
    
    private Vector3 RoundPosition(Vector3 position) {
        return new Vector3(Mathf.Round(position.x + 0.5f), position.y, Mathf.Round(position.z + 0.5f));
    }
}