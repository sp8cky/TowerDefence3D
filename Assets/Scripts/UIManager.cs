using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour {
    public static UIManager instance;
    private GameObject buildUI;
    private GameObject playerUI;
    public GameObject towerBasic;
    public TMP_Text scoreText;
    public TMP_Text highScoreText;
    public TMP_Text playerHealthText;
    public TMP_Text baseHealthText;
    public TMP_Text roundText;
    public TMP_Text stateText;
    public TMP_Text timerText;
    private float currentTimer = 10f;
    private bool isTimerRunning = false; 
    private bool isBuildUIOpen = false;
    private bool isPlayerUIOpen = true;
    private bool isPlacementMode = false;
    private GameObject placementPreviewCube;
    public Material placementPreviewMaterial;

    void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }
    
    private void Start() {
        // UI settings
        buildUI = GameObject.Find("BuildUI");
        playerUI = GameObject.Find("PlayerUI");
        if (buildUI == null || playerUI == null) Debug.Log("UIs not found");
        buildUI.SetActive(isBuildUIOpen);
        playerUI.SetActive(isPlayerUIOpen);

        // initialize UI texts
        scoreText.text = "Score: " + "0".ToString();
        baseHealthText.text = "Base-Health: " + "100".ToString();
        playerHealthText.text = "Player-Health: " + "100".ToString();
        roundText.text = "Round: " + "1".ToString();
        stateText.text = "- Preparation".ToString();
    }   
    private void Update() {
        if (isTimerRunning) {
            currentTimer -= Time.deltaTime;
            UpdateTimerText(currentTimer);
        }
        if (isPlacementMode) {
            UpdatePlacementPreview();
        }
    }

    public void UpdateScore(int score) { scoreText.text = "Score: " + score.ToString(); }
    public void UpdateHighScore(int highScore) { highScoreText.text = "Highscore: " + highScore.ToString(); }
    public void UpdateBaseHealth(int baseHealth) { baseHealthText.text = "Base-Health: " + baseHealth.ToString(); }
    public void UpdatePlayerHealth(int playerHealth) { playerHealthText.text = "Player-Health: " + playerHealth.ToString(); }
    public void UpdateRound(int round) { roundText.text = "Round: " + round.ToString(); }
    public void UpdateGameState(string state) { stateText.text = "- " + state; }

    // Timer aktualisieren und überprüfen, ob er abgelaufen ist
    private void UpdateTimerText(float time) {
        if (time <= 0) {
            timerText.text = "Timer: 0";
            isTimerRunning = false;
            GameManager.instance.StartNextRound(); // start next round
        } else {
            timerText.text = "Timer: " + Mathf.RoundToInt(time).ToString();
        }
    }

    // start timer
    public void StartTimer(float duration) {
        currentTimer = duration;
        isTimerRunning = true;
    }
    public void SelectTowerBasic() {
        Debug.Log("Selected Tower: TowerBasic");
        StartPlacementPreview();
    }

    // toggles both UIs
    public void ToggleUIs() {
        if (isBuildUIOpen) {
            isBuildUIOpen = false;
            isPlayerUIOpen = true;
            buildUI.SetActive(false);
            ToggleUIComponents("Infos", true);
            StopPlacementPreview(); // Stop placement preview when closing build UI
        } else if (isPlayerUIOpen) {
            isBuildUIOpen = true;
            isPlayerUIOpen = false;
            buildUI.SetActive(true);
            ToggleUIComponents("Infos", false);
        }
    }

    // helps to just toggle specific UI components
    void ToggleUIComponents(string name, bool show) {
        Transform objectsToHide = playerUI.transform.Find(name);
        if (objectsToHide != null) {
            foreach (Transform child in objectsToHide) child.gameObject.SetActive(show); 
        } else {
            Debug.LogError("objectsToHide nicht gefunden!");
        }
    }
    public void StartPlacementPreview() {
        isPlacementMode = true;
        buildUI.SetActive(false);
        ToggleCursorVisibility(true);
    }

    public void StopPlacementPreview() {
        isPlacementMode = false;
        buildUI.SetActive(true);
        Destroy(placementPreviewCube);
        //ToggleCursorVisibility(false);
    }

    // TODO: fix exact placement of the preview cube
    void UpdatePlacementPreview() {
        // Mausposition in der Game View erhalten
        Vector3 mousePosition = Input.mousePosition;
        
        // Mausposition auf den Boden der Game View projizieren
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;
        if (groundPlane.Raycast(ray, out rayDistance)) {
            Vector3 groundPoint = ray.GetPoint(rayDistance);
            
            // Einen Raycast senkrecht zum Boden von der projizierten Mausposition aus starten
            Ray groundRay = new Ray(groundPoint + Vector3.up * 100f, Vector3.down);
            
            // Cubes färben basierend auf dem getroffenen Punkt
            RaycastHit hit;
            if (Physics.Raycast(groundRay, out hit, Mathf.Infinity)) {
                if (hit.collider.CompareTag("Ground")) {
                    Vector3Int gridPosition = Vector3Int.RoundToInt(hit.point);
                    if (placementPreviewCube == null) {
                        placementPreviewCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        placementPreviewCube.GetComponent<Renderer>().material = placementPreviewMaterial;
                    }
                    placementPreviewCube.transform.position = gridPosition;
                }
            }
        }
    }

    void ToggleCursorVisibility(bool isVisible) {
        Cursor.visible = isVisible;
        Cursor.lockState = isVisible ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
