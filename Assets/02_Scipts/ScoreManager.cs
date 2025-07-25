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

    public int score;//점수숫자표시
    string key = "bestScore"; // 오타방지
    void Start()
    {
        UpdateScoreText();
        //PlayerPrefs.DeleteAll();// 데이터 삭제 테스트
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
            // 최고 점수가 있다면
            if (PlayerPrefs.HasKey(key))
            {
                // 최고 점수 가져오기
                int best = PlayerPrefs.GetInt(key);
                // 최고 점수 < 현재 점수
                if (best < score)
                {
                    // 현재 점수를 최고 점수에 저장한다.
                    PlayerPrefs.SetInt(key, score);
                    bestscoreText.text = score.ToString(); // 현재 점수가 최고 점수이니까 time 값을 넣어도 됨
                }
                else // 최고 점수 > 현재 점수
                {
                    bestscoreText.text = best.ToString(); // best가 여전히 최고 점수니까
                }
            }
            else // 최고 점수가 없다면
            {
                // 현재 점수를 저장한다.
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
