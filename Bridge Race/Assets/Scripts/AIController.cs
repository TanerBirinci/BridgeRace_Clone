using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using DG.Tweening;

public class AIController : MonoBehaviour
{
    public ColorType colorOfThis;
    public GameObject targetsParent;
    public List<GameObject> targets = new List<GameObject>();
    public List<GameObject> ladders;
    public List<PartController> stackGreen;

    public LayerMask partLayerMask;

    public float radius;
    public int openedLadder;
    
    public Transform nestAI;
    
    public int countAIStack;
    public int ladderCount;
    public int goLadder;
    
    
    public Animator animator;
    public NavMeshAgent agent;

    private Vector3 _targetTransform;

    public bool haveTarget;
    public bool ladderDone;

    public Transform trail;

    private bool _aiFinished;
    private static readonly int Run = Animator.StringToHash("Run");


    public void SetTarget()
    {
        for (int i = 0; i < targetsParent.transform.childCount; i++)
        {
            targets.Add(targetsParent.transform.GetChild(i).gameObject);
        }
    }

    private void FixedUpdate()
    {
        if(GameManager.I.isGameFinished || !GameManager.I.isGameStarted || _aiFinished) return;

        if (goLadder>=9)
        {
            _targetTransform = ladders[openedLadder-1].transform.position;
            agent.SetDestination(_targetTransform);
            if (stackGreen.Count==0)
            {
                haveTarget = false;
                goLadder = 0;
            }

            if (ladderCount==17 && ladderDone==false)
            {
                haveTarget = false;
                ladderDone = true;
            }
        }
        
        if (!haveTarget && targets.Count>0)
        {
            goLadder = countAIStack;
            ChooseTarget();
        }
    }

    private void ChooseTarget()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius,partLayerMask);
        List<Vector3> ourColors = new List<Vector3>();
        
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].GetComponent<PartController>().colorOfThis==colorOfThis)
            {
                if (ourColors.Contains(hitColliders[i].transform.position)) return;
                ourColors.Add(hitColliders[i].transform.position); 
            }
        }

        if (ourColors.Count>0)
        {
            _targetTransform = ourColors[0];
        }
        else
        {
            int random = Random.Range(0, targets.Count);
            _targetTransform = targets[random].transform.position;
        }
        agent.SetDestination(_targetTransform);
        if (!animator.GetBool(Run))
        {
            animator.SetBool(Run,true);
        }

        haveTarget = true;
        
    }

    public void GoLevelEnd(Vector3 targetPosition)
    {
        _aiFinished = true;
        agent.SetDestination(targetPosition);
    }

    public void LevelFinished()
    {
        animator.SetBool(Run, false);
    }

    private void OnTriggerEnter(Collider target)
    {
        if(GameManager.I.isGameFinished || !GameManager.I.isGameStarted || _aiFinished) return;

        if (target.gameObject.CompareTag("Part"))
        {
            var partController = target.gameObject.GetComponent<PartController>();
            if (colorOfThis!= partController.colorOfThis || partController.isCarrying ) return;
         
            CollectPart(partController,target.transform);
            
        }
        
        if (target.gameObject.CompareTag("Ladder"))
        {
            
            var ladderController = target.gameObject.GetComponent<LadderController>();
            
            if (ladderController.colorOfLadder == colorOfThis || stackGreen.Count<=0) return;
            
            PlaceLadder(ladderController);
        }

    }

    private void CollectPart(PartController partController, Transform targetTransform)
    {
        partController.isCarrying = true;
        stackGreen.Add(partController);
        targets.Add(targetTransform.gameObject);
        
        partController.transform.SetParent(nestAI);
        
        var pos = new Vector3(0f, 0.20f*countAIStack, 0);

        targetTransform.DOLocalMove(pos, 0.2f).OnComplete((() =>
        {
            trail.gameObject.SetActive(true);
            trail.localPosition = stackGreen[^1].transform.localPosition;
        }));
        
        
        targetTransform.localRotation=Quaternion.identity;
        targets.Remove(targetTransform.gameObject);
        
        haveTarget = false;
        countAIStack++;
        goLadder++;
            
        if (openedLadder<ladders.Count)
        {
            openedLadder++;
        }
    }

    private void PlaceLadder(LadderController ladderController)
    {
        if (ladderController.colorOfLadder != colorOfThis)
        {
            ladderController.colorOfLadder = colorOfThis;
            ladderController.ChangeLadderColor(false);
        }
            
        ladderController.OpenLadder(false);

        ladderController.GetComponent<Collider>().isTrigger = true;
            
        var lastPart = stackGreen.LastOrDefault();
        lastPart.transform.SetParent(ObjectPool.I.transform);
            
        lastPart.DropToLadder(); 
                    
        
        countAIStack--;
        ladderCount++;
        stackGreen.Remove(lastPart);
            
        if (stackGreen.Count == 0)
        {
            trail.gameObject.SetActive(false);
        }
        else
        {
            trail.localPosition = stackGreen[^1].transform.localPosition;
        }
    }
}
