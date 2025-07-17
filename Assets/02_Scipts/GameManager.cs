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
    //public int goalScore;//������������ �ٸ���
    void Update()
    {
        
        
        if (scoreManager.score >= stageScore[currentStage - 1])//������Ű ���� �޼���.
        {
            NextStage();
            
        }
    }

    void Awake()
    {
        //DontDestroyOnLoad(this.gameObject);   //�̰� ��ư������ ����.
        
        SceneManager.sceneLoaded += OnSceneLoaded;  // ���� �ٲ� �� �ڵ� ȣ��
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;  // �޸� ���� ����
    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        boardManager = GameObject.Find("BoardManager").GetComponent<BoardManager>();
        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        countdownDisplay = GameObject.Find("TimeText").GetComponent<TextMeshProUGUI>();
        CountDown();
        // Ÿ�̸� ���� ����
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
    void NextStage()    // ���� �������� Ŭ�����ϸ�.
    {

        currentStage++;
        scoreManager.score = 0;
        if (currentStage > maxStage)    // 4 > 3�Ǹ� �ٷ� ��Ʈ�η� ����
        {
            GotoIntro();
        }
        else
            SceneManager.LoadScene("0" + currentStage + "_Stage" + currentStage);
        
    }
    void GotoIntro()    // ������ �������� Ŭ�����ϸ�.
    {

        SceneManager.LoadScene("00_Intro");
    }

    public void OnTogglePauseButton()
    {
        if (Time.timeScale == 0) //����������
        {
            Time.timeScale = 1f; //����
            
        }
        else //�����̸�
        {
            Time.timeScale = 0; //���߱�
            
        }
    }

}
