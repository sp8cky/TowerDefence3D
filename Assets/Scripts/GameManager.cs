using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    private Spawner enemySpawner;
    private PlayerController playerController;
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
    void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    void Start() {
        Debug.ClearDeveloperConsole();
        enemySpawner = FindObjectOfType<Spawner>();
        playerController = FindObjectOfType<PlayerController>();
        if (enemySpawner == null || playerController == null) Debug.LogError("Scripte nicht gefunden.");
        
        // initialize of game variables
    }

    
}

