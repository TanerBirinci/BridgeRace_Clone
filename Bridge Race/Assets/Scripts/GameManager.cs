using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager I;

    public GameObject winPanel;
    public GameObject losePanel;
    
    
    public bool isGameStarted;
    public bool isGameFinished;

    [SerializeField] private PlayerMovement pm;
    [SerializeField] private AIController ai;

    private void Awake()
    {
        I = this;
        
    }
    


    public void StartGame()
    {
        isGameStarted = true;
        isGameFinished = false;
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(0);
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
    
    public void FinishGame(bool isPlayerFinished)
    {
        isGameFinished = true;
        
        if (isPlayerFinished)
        {
            winPanel.SetActive(true);
        }
        else
        {
            losePanel.SetActive(true);
        }
        pm.LevelFinished();
        ai.LevelFinished();
    }
}
