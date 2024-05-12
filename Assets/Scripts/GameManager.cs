using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    private UIManager uiManager;
    private Spawner enemySpawner;
    private TowerController towerController;
    private PlayerController playerController;
    private CameraSwitcher cameraSwitcher;
    public GameState currentState = GameState.Preparation;
    private bool spacePressed = false; // check if space key is pressed for not to trigger the button 
    public int baseHealth; 
    private int currentBaseHealth;
    private int playerHealth; 
    private int playerScore;
    private int playerHighScore;
    private int currentRound = 1;
    public enum GameState {
        Preparation,
        Attack
    }

    void Start() {
        Debug.ClearDeveloperConsole();
        uiManager = FindObjectOfType<UIManager>();
        enemySpawner = FindObjectOfType<Spawner>();
        towerController = FindObjectOfType<TowerController>();
        uiManager = FindObjectOfType<UIManager>();
        playerController = FindObjectOfType<PlayerController>();
        cameraSwitcher = FindObjectOfType<CameraSwitcher>(); 
        if (uiManager == null || enemySpawner == null || towerController == null || playerController == null || cameraSwitcher == null) Debug.LogError("Scripte nicht gefunden.");
        
        // Initialisierung von Spielerwerten
        playerHealth = 100;
        baseHealth = 100;
        playerScore = 0;
        currentBaseHealth = baseHealth;
        playerHighScore = PlayerPrefs.GetInt("HighScore", 0);
        // Aktualisiere die Anzeige des Highscores
        UpdateHighScore();

        // Setze den Spielzustand auf Vorbereitung (Preparation)
        Debug.Log("Set game state to Preparation and freeze player");
        currentState = GameState.Preparation;
        uiManager.UpdateGameState("Preparation");
        playerController.FreezePlayer();
    }
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            spacePressed = true;
        } else if (Input.GetKeyUp(KeyCode.Space)) {
            spacePressed = false;
        }
    }

    public int GetPlayerHealth() { return playerHealth; }
    public int GetPlayerScore() { return playerScore; }

    public void AddScore(int score) {
        playerScore += score;
        if (uiManager != null) uiManager.UpdateScore(playerScore);
        // check if new highscore
        if (playerScore > playerHighScore) {
            playerHighScore = playerScore;
            // safe new highscore
            PlayerPrefs.SetInt("HighScore", playerHighScore);
            UpdateHighScore();
        }
    }

    public void EnemyReachedBase(int enemyScore) {
        currentBaseHealth -= enemyScore;
        if (uiManager != null) uiManager.UpdateBaseHealth(currentBaseHealth);
        if (currentBaseHealth <= 0) EndGame();
    }

    void EndGame() {
        Debug.Log("End game, load scene: Game Over");
        SceneManager.LoadScene("GameOver");
    }

    public void StartNextRound() {
        Debug.Log("Start next round");
        if (!spacePressed) { // TODO: fix space trigger
            if (currentState == GameState.Preparation) { // ATTACK
                currentState = GameState.Attack;
                uiManager.UpdateGameState("Attack"); // Aktualisiere den UI-Text, um den neuen Zustand anzuzeigen

                // start timer, enemy spawn and tower 
                uiManager.StartTimer(10f); 
                enemySpawner.StartEnemySpawn();
                towerController.Update();
                cameraSwitcher.SwitchView();
                playerController.UnfreezePlayer();

            } else if (currentState == GameState.Attack) { // PREP
                currentState = GameState.Preparation;
                uiManager.UpdateGameState("Preparation"); 
                enemySpawner.StopEnemySpawn();
                currentRound++; 
                uiManager.UpdateRound(currentRound);
                cameraSwitcher.SwitchView();
                playerController.FreezePlayer();
            }
        }
    }

    void UpdateHighScore() { if (uiManager != null) uiManager.UpdateHighScore(playerHighScore); }

    public void UpdatePlayerHealth(int damage) {
        playerHealth -= damage;
        if (uiManager != null) uiManager.UpdatePlayerHealth(playerHealth);
        if (playerHealth <= 0) {
            Debug.Log("Freeze Player");
            if (playerController != null) playerController.FreezePlayer();
        }
    }
}

