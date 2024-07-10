using UnityEngine;

// controls camera views
public class CameraController : MonoBehaviour {
    private Camera playerCamera;
    private Camera overviewCamera;
    public KeyCode switchKey = KeyCode.O; // Taste zum Wechseln der Kameras
    private PlayerController playerController;

    void Start() {
        // find overview camera and deactivate it
        playerCamera = GameObject.Find("PlayerCamera").GetComponent<Camera>();
        overviewCamera = GameObject.Find("OverviewCamera").GetComponent<Camera>();
        playerController = FindObjectOfType<PlayerController>();
        
        if (playerController == null) Debug.LogError("PlayerController not found.");
        if (playerCamera == null || overviewCamera == null) Debug.LogError("Cameras not found.");
        overviewCamera.gameObject.SetActive(false);
    }

    void Update() {
        // change cameras
        if (Input.GetKeyDown(switchKey)) {
            SwitchCameras();
        }
    }

    void SwitchCameras() {
        if (overviewCamera.gameObject.activeInHierarchy) { // change back to player camera
            overviewCamera.gameObject.SetActive(false);
            playerCamera.gameObject.SetActive(true);
            playerController.UnfreezePlayer();
        } else { // change to overview camera
            overviewCamera.gameObject.SetActive(true);
            playerCamera.gameObject.SetActive(false);
            playerController.FreezePlayer();
        }
    }
}
