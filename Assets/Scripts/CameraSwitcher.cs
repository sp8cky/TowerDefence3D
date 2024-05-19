using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour {
    public Camera playerCamera; // player camera
    public Camera gameCamera; // game camera
    private bool isTopDownView = true; // check if game camera is active
    private float panSpeed = 1f;
    public float zoomSpeed = 5f;

    void Update() {
        if (Input.GetKeyDown(KeyCode.C)) SwitchView();
        // check if mouse scroll wheel is used
        if (isTopDownView && gameCamera.enabled) {
            float scrollInput = Input.GetAxis("Mouse ScrollWheel");
            ZoomCamera(scrollInput);
        }
        // Check if the mouse wheel button is being held down for moving the camera
        if (Input.GetMouseButton(2)) MoveCamera();
    }

    private void MoveCamera() {
        // Get the mouse movement
        float mouseZ = Input.GetAxis("Mouse X");
        float mouseX = Input.GetAxis("Mouse Y");

        // Calculate the new camera position
        Vector3 newPosition = gameCamera.transform.position - new Vector3(mouseX, 0, -mouseZ) * panSpeed;
        gameCamera.transform.position = newPosition; // Update the camera position
    }

    public void SwitchView() {
        isTopDownView = !isTopDownView;
        playerCamera.gameObject.SetActive(!isTopDownView);
        gameCamera.gameObject.SetActive(isTopDownView);
    }
    
    // in the game view the camera can be zoomed in and out
    private void ZoomCamera(float scrollInput) {
        // Convert the mouse position to a world point
        Ray ray = gameCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) {
            // Calculate the direction towards the hit point
            Vector3 direction = (hit.point - gameCamera.transform.position).normalized;

            // Move the camera towards the hit point, while also taking into account the scroll input and zoom speed
            Vector3 newPosition = gameCamera.transform.position + direction * scrollInput * zoomSpeed;

            // Clamp the y position to keep the camera within certain bounds
            newPosition.y = Mathf.Clamp(newPosition.y, 5f, 60f);
            gameCamera.transform.position = newPosition; // Update the camera position
        }
    }
}