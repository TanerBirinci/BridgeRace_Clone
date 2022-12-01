using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndController : MonoBehaviour
{

    public ParticleSystem[] confettiParticles;

    private void OnTriggerEnter(Collider other)
    {
        if(GameManager.I.isGameFinished || !GameManager.I.isGameStarted) return;

        if (other.CompareTag("Player"))
        {
            GameManager.I.FinishGame(true); 
            LevelFinished();
        }

        if (other.CompareTag("AI"))
        {
            GameManager.I.FinishGame(false); 

        }
    }

    private void LevelFinished()
    {
        for (int i = 0; i < confettiParticles.Length; i++)
        {
            confettiParticles[i].Play();
        }
    }
}
