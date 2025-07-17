using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int currentStage = 1;
    public int maxStage = 3;

    public BoardManager boardManager;
    public ScoreManager scoreManager;

    public int[] stageScore = { 5, 10, 20, 100 };
    public int[] stageTime = { 60, 120, 180 };
    public int countdownTime;
    int countMin;
    int countSec;
    public TextMeshProUGUI countdownDisplay;
    //public panel
    //public int goalScore;//스테이지마다 다르게
    void Update()
    {
        boardManager = GameObject.Find("BoardManager").GetComponent<BoardManager>();
        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        //countdownDisplay = 
        if (scoreManager.score >= stageScore[currentStage - 1])
        {
            NextStage();
            stageTime[currentStage - 1] = stageTime[currentStage - 1 + 1];
        }
    }

    void Awake()
    {
        CountDown();
        //boardManager = GameObject.Find("BoardManager").GetComponent<BoardManager>();
        //scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        DontDestroyOnLoad(this.gameObject); // 씬 전환해도 사라지지 않게
    }

    public void CountDown()
    {
        StartCoroutine(CountdownStart());
    }

    IEnumerator CountdownStart()
    {

        while (stageTime[currentStage - 1] > 0)
        {
            countMin = stageTime[currentStage - 1] / 60;
            countSec = stageTime[currentStage - 1] % 60;
            string minText = countMin.ToString("D2");
            string secText = countSec.ToString("D2");

            countdownDisplay.text = $"{minText}:{secText}";
            yield return new WaitForSeconds(1f);

            stageTime[currentStage - 1]--;
        }
        //GameOver
    }
    void NextStage()    // 현재 스테이지 클리어하면.
    {

        currentStage++;
        scoreManager.score = 0;
        if (currentStage > maxStage)    // 4 > 3되면 바로 인트로로 꺼져
        {
            GotoIntro();
        }
        else
            SceneManager.LoadScene("0" + currentStage + "_Stage" + currentStage);

    }
    void GotoIntro()    // 마지막 스테이지 클리어하면.
    {

        SceneManager.LoadScene("00_Intro");
    }



}
