using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateCubes : MonoBehaviour {
    public GameObject cubePrefab; // Prefab for the cubes
    private Vector2 gridSize = new Vector2(50, 50); // Grid size
    public float height = 0.1f; // Height of the cubes
    public Material pathMaterial; // Material for cubes on the path
    public Material transparentMaterial; // Material for cubes not on the path
    public Transform[] waypoints; // Waypoints for the path

    private GameObject spawnObject;
    private GameObject baseObject;

    void Start() {
        spawnObject = GameObject.FindGameObjectWithTag("Spawn");
        baseObject = GameObject.FindGameObjectWithTag("Base");

        if (spawnObject != null && baseObject != null) {
            GenerateCubesGrid();
            AddSpawnAndBaseAsWaypoints();
            ColorPathCubes();
        } else {
            Debug.LogError("Spawn or Base object not found.");
        }
    }

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

    void AddSpawnAndBaseAsWaypoints() {
        List<Transform> updatedWaypoints = new List<Transform>(waypoints);
        updatedWaypoints.Insert(0, spawnObject.transform);
        updatedWaypoints.Add(baseObject.transform);
        waypoints = updatedWaypoints.ToArray();
    }

    public void ColorPathCubes() {
        for (int i = 0; i < waypoints.Length - 1; i++) {
            Transform currentWaypoint = waypoints[i];
            Transform nextWaypoint = waypoints[i + 1];

            Vector3 roundedPosition = RoundPosition(currentWaypoint.position);
            ColorCubeAtPosition(roundedPosition);

            Vector3 direction = nextWaypoint.position - currentWaypoint.position;
            float step = 1.0f / Mathf.Max(Mathf.Abs(direction.x), Mathf.Abs(direction.z));

            for (float t = 0; t <= 1; t += step) {
                Vector3 positionOnLine = Vector3.Lerp(currentWaypoint.position, nextWaypoint.position, t);
                Vector3 roundedPositionOnLine = RoundPosition(positionOnLine);
                ColorCubeAtPosition(roundedPositionOnLine);
            }
        }
    }

    private Vector3 RoundPosition(Vector3 position) {
        return new Vector3(Mathf.Round(position.x + 0.5f), position.y, Mathf.Round(position.z + 0.5f));
    }

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
}