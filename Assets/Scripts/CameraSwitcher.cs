using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour {
    public Camera playerCamera; // player camera
    public Camera gameCamera; // game camera
    private bool isTopDownView = true; // check if game camera is active

    void Update() {
        if (Input.GetKeyDown(KeyCode.C)) SwitchView();
    }

    public void SwitchView() {
        isTopDownView = !isTopDownView;
        playerCamera.gameObject.SetActive(!isTopDownView);
        gameCamera.gameObject.SetActive(isTopDownView);
    }
}