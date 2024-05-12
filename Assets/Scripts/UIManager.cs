using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour {
    private GameManager gameManager;
    public TMP_Text scoreText;
    public TMP_Text highScoreText;
    public TMP_Text playerHealthText;
    public TMP_Text baseHealthText;
    public TMP_Text roundText;
    public TMP_Text stateText;
    public TMP_Text timerText;
    private float currentTimer = 10f;
    private bool isTimerRunning = false; 

    
    private void Start() {
        gameManager = FindObjectOfType<GameManager>();
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
            gameManager.StartNextRound(); // start next round
        } else {
            timerText.text = "Timer: " + Mathf.RoundToInt(time).ToString();
        }
    }

    // start timer
    public void StartTimer(float duration) {
        currentTimer = duration;
        isTimerRunning = true;
    }
}
