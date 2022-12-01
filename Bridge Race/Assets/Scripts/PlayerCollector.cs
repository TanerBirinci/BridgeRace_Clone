using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerCollector : MonoBehaviour
{
    public Transform nest;
    public List<PartController> parts;
    public ColorType colorOfThis;
    public Transform trail;
    
    
    
    private void OnTriggerEnter(Collider other)
    {
        if(GameManager.I.isGameFinished || !GameManager.I.isGameStarted) return;
       
        if (other.CompareTag("Part"))
        {
            var partController = other.gameObject.GetComponent<PartController>();
            if (colorOfThis != partController.colorOfThis) return;
            
            CollectPart(partController,other.transform);
     
        }

        else if (other.CompareTag("Ladder"))
        {
           SetLeader(other.GetComponent<LadderController>());
        }

    }

    private void CollectPart(PartController partController, Transform target)
    {
        parts.Add(partController);
        target.SetParent(nest);
        Vector3 pos = new Vector3(0f, parts.IndexOf(partController) * 0.18f, 0);

        target.DOLocalMove(pos, 0.2f).OnComplete((() =>
        {
            trail.gameObject.SetActive(true);
            trail.localPosition = parts[^1].transform.localPosition;
        }));
        target.localRotation=Quaternion.identity;

    }


    private void SetLeader(LadderController ladderController)
    {
        var lastPart = parts.LastOrDefault();
        
        if (ladderController.colorOfLadder == colorOfThis || parts.Count==0)
        {
            return;
        }


        if (ladderController.colorOfLadder != colorOfThis)
        {
            ladderController.colorOfLadder = colorOfThis;
            ladderController.ChangeLadderColor(true);
        }
        
        ladderController.GetComponent<BoxCollider>().isTrigger = false;
        parts.Remove(lastPart);
        
        if (parts.Count == 0)
        {
            trail.gameObject.SetActive(false);
        }
        else
        {
            trail.localPosition = parts[^1].transform.localPosition;
        }
        
       
            
        lastPart.transform.SetParent(ObjectPool.I.transform);
        lastPart.ResetThis();
        ladderController.OpenLadder(true);


    }
    
    
}
