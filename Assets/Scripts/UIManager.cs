using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour {
    public TMP_Text scoreText;
    public TMP_Text highScoreText;
    public TMP_Text playerHealthText;
    public TMP_Text baseHealthText;
    

    public void UpdateScore(int score) {
        scoreText.text = "Score: " + score.ToString(); 
        Debug.Log("Score: " + score.ToString());
    }
    public void UpdateHighScore(int highScore) {
        highScoreText.text = "Highscore: " + highScore.ToString(); 
        Debug.Log("Highscore: " + highScore.ToString());
    }
    public void UpdateBaseHealth(int baseHealth) {
        baseHealthText.text = "Base-Health: " + baseHealth.ToString();
        Debug.Log("Base-Health: " + baseHealth.ToString());
    }
    public void UpdatePlayerHealth(int playerHealth) {
        playerHealthText.text = "Player-Health: " + playerHealth.ToString();
        Debug.Log("Player-Health: " + playerHealth.ToString());
    }
}
