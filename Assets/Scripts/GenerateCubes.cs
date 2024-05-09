using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GenerateCubes : MonoBehaviour {
    public GameObject cubePrefab; // Prefab for the cubes
    private Vector2 gridSize = new Vector2(50, 50); // Grid size
    public float height = 0.1f; // Height of the cubes
    public Material pathMaterial; // Material for cubes on the path
    public Material transparentMaterial; // Material for cubes not on the path

    private GameObject spawnObject;
    private GameObject baseObject;

    void Start() {
        spawnObject = GameObject.FindGameObjectWithTag("Spawn");
        baseObject = GameObject.FindGameObjectWithTag("Base");

        if (spawnObject != null && baseObject != null) {
            GenerateCubesGrid();
            GeneratePath();
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
        Debug.Log("Cubes erfolgreich erzeugt");
    }

    void GeneratePath() {
        Vector3 spawnPosition = spawnObject.transform.position;
        Vector3 basePosition = baseObject.transform.position;

        // Check if the spawn and base positions are on the same row or column
        if (Mathf.Approximately(spawnPosition.x, basePosition.x) || Mathf.Approximately(spawnPosition.z, basePosition.z)) {
            float startX = Mathf.Min(spawnPosition.x, basePosition.x);
            float endX = Mathf.Max(spawnPosition.x, basePosition.x);
            float startZ = Mathf.Min(spawnPosition.z, basePosition.z);
            float endZ = Mathf.Max(spawnPosition.z, basePosition.z);

            // Loop through each cube and check if it lies on the path
            for (float x = startX; x <= endX; x++) {
                for (float z = startZ; z <= endZ; z++){
                    if (IsOnPath(x, z)) {
                        // Calculate the name of the cube
                        string cubeName = "Cube " + x.ToString("00") + "-" + z.ToString("00");

                        // Check if the cube exists
                        GameObject cube = GameObject.Find(cubeName);
                        if (cube != null) {
                            cube.tag = "Path";
                            // Set the material
                            Renderer renderer = cube.GetComponent<Renderer>();
                            if (renderer != null) renderer.material = pathMaterial;
                        }
                    }
                }
            }
        } else {
            Debug.LogError("Spawn and Base objects are not aligned horizontally or vertically.");
        }
    }

    bool IsOnPath(float x, float z) {
        Vector3 spawnPosition = spawnObject.transform.position;
        Vector3 basePosition = baseObject.transform.position;

        // Check if the spawn and base positions are on the same row or column
        if (Mathf.Approximately(spawnPosition.x, basePosition.x)) {
            // Check if the cube is between the spawn and base positions on the z-axis
            return Mathf.Min(spawnPosition.z, basePosition.z) <= z && z <= Mathf.Max(spawnPosition.z, basePosition.z);
        } else if (Mathf.Approximately(spawnPosition.z, basePosition.z)) {
            // Check if the cube is between the spawn and base positions on the x-axis
            return Mathf.Min(spawnPosition.x, basePosition.x) <= x && x <= Mathf.Max(spawnPosition.x, basePosition.x);
        } else {
            Debug.LogError("Spawn and Base objects are not aligned horizontally or vertically.");
            return false;
        }
    }
}