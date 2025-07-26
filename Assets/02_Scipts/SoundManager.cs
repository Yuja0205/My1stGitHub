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
    public void SoundStop()
    {
        Drop.mute = true;
        MissDrop.mute = true;
        Match.mute = true;
        Special.mute = true;
        Gameover.mute = true;
        Intro.mute = true;
    }
    public void SoundStart()
    {
        Drop.mute = false;
        MissDrop.mute = false;
        Match.mute = false;
        Special.mute = false;
        Gameover.mute = false;
        Intro.mute = false;
    }
}
