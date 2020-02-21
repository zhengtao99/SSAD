using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour
{
    public Text scoreText;

    void OnEnable() {
        // += to subscribe event in other C# script
        PlayerController.OnScoreUpdate += OnScoreUpdate;
    }

    void OnDisable() {
        // -= to unsubscribe event in other C# script
        PlayerController.OnScoreUpdate -= OnScoreUpdate;
    }
    void OnScoreUpdate(int score) {
        scoreText.text = "Coin: " + score;
    }

    // Start is called before the first frame update
    void Start()
    {
        scoreText = GetComponent<Text>();
        scoreText.text = "Coin: 0";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
