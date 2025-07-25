using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public GameManager gamemanager;
    public Text scoreText;
    public Text endScore;

    public int score;

    void Start()
    {
        UpdateScoreText();
    }

    public void AddScore()
    {
        score++;
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        //if (scoreText != null)
        //{
        //    scoreText.text = $"{score}";
        //}
        if (gamemanager.currentStage == 5)
        {
            scoreText.text = $"{score}";
            endScore.text = $"{score}";

        }
        else if (gamemanager.currentStage == 99)
        {
            scoreText.text = $"{score}";
            endScore.text = $"{score}";
        }
        else
        {
            scoreText.text = $"{score} / {gamemanager.stageScore[gamemanager.currentStage - 1]}";
            endScore.text = $"{score} / {gamemanager.stageScore[gamemanager.currentStage - 1]}";
        }
    }
    
}
