using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource Intro;
    public AudioSource Gameover;
    public AudioSource Drop;
    public AudioSource MissDrop;
    public AudioSource Match;
    public AudioSource Special;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public void DropSound()
    {
        Drop.Play();
    }
    public void MissDropSound()
    {
        MissDrop.Play();
    }
    public void MatchSound()
    {
        Match.Play();
    }
    public void SpecialSound()
    {
        Special.Play();
    }
    public void GameoverSound()
    {
        Gameover.Play();
    }
    public void IntroSound()
    {
        Intro.Play();
    }
}
