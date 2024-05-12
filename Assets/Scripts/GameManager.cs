using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    private UIManager uiManager;
    public int baseHealth = 30; // start base-core value
    private int currentBaseHealth;
    private int playerHealth; 
    private int playerScore;
    private int playerHighScore;

    void Start() {
        Debug.ClearDeveloperConsole();
        uiManager = FindObjectOfType<UIManager>(); 
        if (uiManager == null) Debug.LogError("UIManager nicht gefunden.");
        
        // Initialisierung von Spielerwerten
        playerHealth = 50;
        playerScore = 0;
        currentBaseHealth = baseHealth;

        playerHighScore = PlayerPrefs.GetInt("HighScore", 0);

        // Aktualisiere die Anzeige des Highscores
        UpdateHighScore();
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
    void UpdateHighScore() {
        if (uiManager != null) uiManager.UpdateHighScore(playerHighScore);
    }

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

