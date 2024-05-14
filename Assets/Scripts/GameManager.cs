using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    private Spawner enemySpawner;
    private TowerController towerController;
    private PlayerController playerController;
    private CameraSwitcher cameraSwitcher;
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
        towerController = FindObjectOfType<TowerController>();
        playerController = FindObjectOfType<PlayerController>();
        cameraSwitcher = FindObjectOfType<CameraSwitcher>(); 
        if (enemySpawner == null || towerController == null || playerController == null || cameraSwitcher == null) Debug.LogError("Scripte nicht gefunden.");
        
        // initialize of game variables
        playerHealth = 100;
        baseHealth = 100;
        playerScore = 0;
        currentBaseHealth = baseHealth;
        playerHighScore = PlayerPrefs.GetInt("HighScore", 0);
        // update highscore
        UpdateHighScore();

        // first state is Preparation
        Debug.Log("Set game state to Preparation and freeze player");
        currentState = GameState.Preparation;
        UIManager.instance.UpdateGameState("Preparation");
        playerController.FreezePlayer();
    }

    public int GetPlayerHealth() { return playerHealth; }
    public int GetPlayerScore() { return playerScore; }

    public void AddScore(int score) {
        playerScore += score;
        UIManager.instance.UpdateScore(playerScore);
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
        UIManager.instance.UpdateBaseHealth(currentBaseHealth);
        if (currentBaseHealth <= 0) EndGame();
    }

    void EndGame() {
        Debug.Log("End game, load scene: Game Over");
        SceneManager.LoadScene("GameOver");
    }

    public void StartNextRound() { // TODO: space triggers at the beginning the ready button in unity
        Debug.Log("Start next round");
        
        if (currentState == GameState.Preparation) { // ATTACK
            currentState = GameState.Attack;
            UIManager.instance.UpdateGameState("Attack");

            // start timer, enemy spawn and tower 
            UIManager.instance.StartTimer(10f); 
            enemySpawner.StartEnemySpawn();
            towerController.Update();
            cameraSwitcher.SwitchView();
            playerController.UnfreezePlayer();
        } else if (currentState == GameState.Attack) { // PREP
            currentState = GameState.Preparation;
            UIManager.instance.UpdateGameState("Preparation"); 
            enemySpawner.StopEnemySpawn();
            currentRound++; 
            UIManager.instance.UpdateRound(currentRound);
            cameraSwitcher.SwitchView();
            playerController.FreezePlayer();
        }
        
    }

    void UpdateHighScore() { UIManager.instance.UpdateHighScore(playerHighScore); }

    public void UpdatePlayerHealth(int damage) {
        playerHealth -= damage;
        UIManager.instance.UpdatePlayerHealth(playerHealth);
        if (playerHealth <= 0) {
            Debug.Log("Freeze Player");
            if (playerController != null) playerController.FreezePlayer();
        }
    }
}

