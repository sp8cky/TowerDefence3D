using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    private UIManager uiManager;
    public int baseScore = 30; // start base-core value
    private int currentBaseScore;
    public int playerHealth = 100; // player health
    private int currentPlayerHealth; 
    public int playerScore; // current player score value
    private int currentPlayerScore;
    void Start() {
        uiManager = FindObjectOfType<UIManager>(); 
        if (uiManager == null) Debug.LogError("UIManager nicht gefunden.");
        currentBaseScore = baseScore;
        currentPlayerHealth = playerHealth;
        currentPlayerScore = playerScore;
    }

    public void AddScore(int score) {
        currentPlayerScore += score;
        if (uiManager != null) uiManager.UpdateScore(currentPlayerScore);
    }

    public void EnemyReachedBase(int enemyScore) {
        currentBaseScore -= enemyScore;
        if (currentBaseScore <= 0) EndGame();
    }
    
    void EndGame() {
        Debug.Log("End game, load scene: Game Over");
        SceneManager.LoadScene("GameOver");
    }

    public void UpdatePlayerHealth(int health) {
        currentPlayerHealth -= health;
        if (uiManager != null) uiManager.UpdateHealth(currentPlayerHealth);
        if (currentPlayerHealth <= 0) EndGame();
    }
}

