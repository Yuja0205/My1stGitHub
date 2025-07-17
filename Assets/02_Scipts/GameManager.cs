using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int currentStage = 2;
    public int maxStage = 3;

    public BoardManager boardManager;
    public ScoreManager scoreManager;

    public int[] stageScore = { 5, 10, 20, 100 };
    public int[] stageTime = { 60, 120, 180 };
    
    int countMin;
    int countSec;
    public TextMeshProUGUI countdownDisplay;
    //public panel
    //public int goalScore;//스테이지마다 다르게
    void Update()
    {
        
        
        if (scoreManager.score >= stageScore[currentStage - 1])//스테이키 점수 달성시.
        {
            NextStage();
            
        }
    }

    void Awake()
    {
        //DontDestroyOnLoad(this.gameObject);   //이거 버튼때문에 못씀.
        
        SceneManager.sceneLoaded += OnSceneLoaded;  // 씬이 바뀔 때 자동 호출
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;  // 메모리 누수 방지
    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        boardManager = GameObject.Find("BoardManager").GetComponent<BoardManager>();
        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        countdownDisplay = GameObject.Find("TimeText").GetComponent<TextMeshProUGUI>();
        CountDown();
        // 타이머 새로 시작
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

    public void OnTogglePauseButton()
    {
        if (Time.timeScale == 0) //멈춰있으면
        {
            Time.timeScale = 1f; //시작
            
        }
        else //움직이면
        {
            Time.timeScale = 0; //멈추기
            
        }
    }

}
