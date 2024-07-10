using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    private GameState currentState;
    private Spawner enemySpawner;
    private InitializeGame initializeGame;
    private int round;
    private float timer = 20f;
    private float currentTimer;
    private bool isTimerRunning = false;
    private float score;

    public enum GameState {
        PREPARATION,
        ATTACK
    }
    
    void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }


    // starts the game after button clicked
    public void Start() {
        // find objects
        enemySpawner = FindObjectOfType<Spawner>();
        initializeGame = FindObjectOfType<InitializeGame>();    
        if (enemySpawner == null || initializeGame == null) Debug.LogWarning("Scripts in GameManager not found.");

        // initialize the game
        currentTimer = timer;
        round = 1;
        currentState = GameState.PREPARATION;
        int randomSpawnCount = IncreaseSpawnCount();
        initializeGame.ActivateSpawns(randomSpawnCount);
        UIManager.instance.UpdateGameState("Preparation");
        UIManager.instance.UpdateRound(round);
        UIManager.instance.UpdateTimer(currentTimer); 
    }

    private void Update() {
        if (currentState == GameState.ATTACK && isTimerRunning) {
            currentTimer -= Time.deltaTime;
            UIManager.instance.UpdateTimer(currentTimer);
            if (currentTimer <= 0) {
                currentTimer = 0;
                UIManager.instance.UpdateTimer(currentTimer);
                isTimerRunning = false;
                ChangeGameState();
            }
        }
    }


    // switches between preparation and attack state
    public void ChangeGameState() {
        if (currentState == GameState.ATTACK) { // Next is PREP
            currentState = GameState.PREPARATION;
            isTimerRunning = false;
            round++; 
            int randomSpawnCount = IncreaseSpawnCount();
            initializeGame.ActivateSpawns(randomSpawnCount);
            enemySpawner.StopEnemySpawn();
            UIManager.instance.UpdateRound(round);
            UIManager.instance.UpdateGameState("Preparation"); 
        } else if (currentState == GameState.PREPARATION) { // Next is ATTACK
            isTimerRunning = true;
            currentTimer = timer;
            currentState = GameState.ATTACK;
            enemySpawner.StartEnemySpawn();
            UIManager.instance.UpdateGameState("Attack"); 
        }
    }

    // increases spawn count for each round
    private int IncreaseSpawnCount() { 
        if (round < initializeGame.GetSpawnList().Count) return round;
        return initializeGame.GetSpawnList().Count; 
    }

    // generates a random spawn count each round
    private int GenerateRandomSpawnCount() { return Random.Range(1, initializeGame.GetSpawnList().Count - 1); }

    // SCORES ///////////////////////////////////////////
    public void AddScore(float score) { 
        this.score += score; 
        UIManager.instance.UpdateScore(this.score);
    }


    // Getter and Setter ///////////////////////////////////////////
    public GameState GetState() { return currentState; }
    
}

