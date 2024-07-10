using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour {
    public static UIManager instance;

    public TMP_Text timerText;
    public TMP_Text stateText;
    public TMP_Text roundText;
    public TMP_Text scoreText;
    public Button readyButton;

    void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }
    
    public void UpdateRound(int round) { roundText.text = "Round: " + round.ToString(); }
    public void UpdateGameState(string state) { stateText.text = "State: " + state; }
    public void UpdateTimer(float time) {timerText.text = "Timer: " + Mathf.RoundToInt(time).ToString(); }
    public void UpdateScore(float score) { scoreText.text = "Score: " + score.ToString(); }
    public void StartAttackPhase() { GameManager.instance.ChangeGameState(); }
    
}


    