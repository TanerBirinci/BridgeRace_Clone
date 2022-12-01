using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GateController : MonoBehaviour
{
    public Transform gateTarget;
    public SpawnManager spawnGround2;
    public SpawnManager spawnGround1;
   
    

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            spawnGround2.gameObject.SetActive(true);
            transform.DOMove(gateTarget.position, 0.3f);
        }

        if (other.CompareTag("AI"))
        {
            spawnGround2.gameObject.SetActive(true);
            spawnGround1.firstLevelDone = true;
            spawnGround2.firstGround = false;
            spawnGround1.ResetParts(other.GetComponent<AIController>().colorOfThis,true);
            
            transform.DOMove(gateTarget.position, .3f);
        }
    }
}
