using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
    public GameObject text;
    //void Awake()
    //{
    //    text.SetActive(true);

        
    //}
    public void GoClassic()
    {
        SceneManager.LoadScene("00_SampleScene");
    }
    public void GoStage1()
    {
        SceneManager.LoadScene("01_Stage1");

    }
    public void GoStage2()
    {
        SceneManager.LoadScene("02_Stage2");

    }
    public void GoStage3()
    {
        SceneManager.LoadScene("03_Stage3");

    }
}
