using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour {
    public Camera playerCamera; // player camera
    public Camera gameCamera; // game camera
    private bool isTopDownView = true; // check if game camera is active
    public float zoomSpeed = 5f;

    void Update() {
        if (Input.GetKeyDown(KeyCode.C)) SwitchView();
        // check if mouse scroll wheel is used
        if (isTopDownView && gameCamera.enabled) {
            float scrollInput = Input.GetAxis("Mouse ScrollWheel");
            ZoomCamera(scrollInput);
        }
    }

    public void SwitchView() {
        isTopDownView = !isTopDownView;
        playerCamera.gameObject.SetActive(!isTopDownView);
        gameCamera.gameObject.SetActive(isTopDownView);
    }

    // in the game view the camera can be zoomed in and out
    private void ZoomCamera(float scrollInput) { // TODO: implement zooming on mouse position
        // change camera position based on scroll input
        Vector3 newPosition = gameCamera.transform.position + gameCamera.transform.forward * scrollInput * zoomSpeed;
        newPosition.y = Mathf.Clamp(newPosition.y, 10f, 60); 
        gameCamera.transform.position = newPosition;
    }
}