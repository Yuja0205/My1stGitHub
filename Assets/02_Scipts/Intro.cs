using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
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
