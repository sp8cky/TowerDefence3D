using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour {
    public TMP_Text scoreText;
    public Text healthText;

    void Start() {
        //scoreText = GameObject.Find("ScoreText").GetComponent<TMP_Text>();
        //if (scoreText == null) Debug.LogError("UI-Textobjekt nicht gefunden.");
        
    }

    // update score text
    public void UpdateScore(int score) {
        //scoreText.text = "Score: " + score.ToString(); 
        Debug.Log("Score: " + score.ToString());
    }
    public void UpdateHealth(int health) {
        //healthText.text = "Health: " + health.ToString();
        Debug.Log("Health: " + health.ToString());
    }
}
