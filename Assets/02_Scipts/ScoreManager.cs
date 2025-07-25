using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public GameManager gamemanager;
    public Text scoreText;
    public Text bestscoreText;
    public Text endScore;

    public int score;//��������ǥ��
    string key = "bestScore"; // ��Ÿ����
    void Start()
    {
        UpdateScoreText();
        //PlayerPrefs.DeleteAll();// ������ ���� �׽�Ʈ
    }

    public void AddScore()
    {
        score++;
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        if (gamemanager.currentStage == 5)
        {
            // �ְ� ������ �ִٸ�
            if (PlayerPrefs.HasKey(key))
            {
                // �ְ� ���� ��������
                int best = PlayerPrefs.GetInt(key);
                // �ְ� ���� < ���� ����
                if (best < score)
                {
                    // ���� ������ �ְ� ������ �����Ѵ�.
                    PlayerPrefs.SetInt(key, score);
                    bestscoreText.text = score.ToString(); // ���� ������ �ְ� �����̴ϱ� time ���� �־ ��
                }
                else // �ְ� ���� > ���� ����
                {
                    bestscoreText.text = best.ToString(); // best�� ������ �ְ� �����ϱ�
                }
            }
            else // �ְ� ������ ���ٸ�
            {
                // ���� ������ �����Ѵ�.
                PlayerPrefs.SetInt(key, score);
                bestscoreText.text = score.ToString();
            }
        }
    
        
    

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
