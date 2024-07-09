using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    private GameState currentState;
    private Spawner enemySpawner;
    private int round;
    private float timer = 10f;
    private float currentTimer;
    private bool isTimerRunning = false;

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
        if (enemySpawner == null) Debug.LogWarning("Scripts in GameManager not found.");


        // initialize the game
        currentTimer = timer;
        round = 1;
        currentState = GameState.PREPARATION;
        UIManager.instance.UpdateGameState("Preparation");
        UIManager.instance.UpdateRound(round);
        UIManager.instance.UpdateTimerText(currentTimer); 
    }

    private void Update() {
        if (currentState == GameState.ATTACK && isTimerRunning) {
            currentTimer -= Time.deltaTime;
            UIManager.instance.UpdateTimerText(currentTimer);
            if (currentTimer <= 0) {
                currentTimer = 0;
                UIManager.instance.UpdateTimerText(currentTimer);
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


    // Getter and Setter ///////////////////////////////////////////
    public GameState GetState() { return currentState; }
    
}

