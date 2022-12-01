using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LevelEndGateController : MonoBehaviour
{
    [SerializeField] private LevelEndController levelEndController;
    public Transform gateTarget;
    
    
    private void OnTriggerEnter(Collider other)
    {
        if(GameManager.I.isGameFinished || !GameManager.I.isGameStarted) return;

        if (other.CompareTag("Player"))
        {
            transform.DOMove(gateTarget.position, 0.3f);

        }

        if (other.CompareTag("AI"))
        {
            transform.DOMove(gateTarget.position, 0.3f);
            other.GetComponent<AIController>().GoLevelEnd(levelEndController.transform.position);
        }
    }
}
