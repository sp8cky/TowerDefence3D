using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    private UIManager uiManager;
    private Spawner enemySpawner;
    private TowerController towerController;
    public GameState currentState = GameState.Preparation;
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
        if (uiManager == null || enemySpawner == null || towerController == null) Debug.LogError("UIManager, EnemySpawner oder TowerManager nicht gefunden.");
        
        // Initialisierung von Spielerwerten
        playerHealth = 100;
        baseHealth = 100;
        playerScore = 0;
        currentBaseHealth = baseHealth;
        playerHighScore = PlayerPrefs.GetInt("HighScore", 0);
        // Aktualisiere die Anzeige des Highscores
        UpdateHighScore();

        // Setze den Spielzustand auf Vorbereitung (Preparation)
        currentState = GameState.Preparation;
        uiManager.UpdateGameState("Preparation");
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
        if (currentState == GameState.Preparation) {
            currentState = GameState.Attack;
            uiManager.UpdateGameState("Attack"); // Aktualisiere den UI-Text, um den neuen Zustand anzuzeigen

            // start timer, enemy spawn and tower 
            uiManager.StartTimer(10f); 
            enemySpawner.StartEnemySpawn();
            towerController.Update();

        } else if (currentState == GameState.Attack) {
            currentState = GameState.Preparation;
            uiManager.UpdateGameState("Preparation"); 
            enemySpawner.StopEnemySpawn();
            currentRound++; 
            uiManager.UpdateRound(currentRound);
        }
    }

    void UpdateHighScore() { if (uiManager != null) uiManager.UpdateHighScore(playerHighScore); }

    public void UpdatePlayerHealth(int damage) {
        playerHealth -= damage;
        if (uiManager != null) uiManager.UpdatePlayerHealth(playerHealth);
        if (playerHealth <= 0) {
            Debug.Log("Freeze Player");
            PlayerController player = FindObjectOfType<PlayerController>();
            if (player != null) player.FreezePlayer();  
        }
    }
}

