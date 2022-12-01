using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    
    public float xValue;
    public float zValue;
    public float yValue;
    public int horizontalCount;
    public int verticalCount;

    public GameObject spawnTransform;
    public List<Transform> spawnPosList;
    public Transform aiTarget;
    public List<PartController> spawnedStacksFirstFloor;
    public List<PartController> spawnedStacksSecondFloor;

    public bool firstLevelDone;

    public AIController aIController;

    public bool firstGround;


    private void Start()
    {
        Spawner();
        aIController.SetTarget();
    }

   

    private void Spawner()
    {
        for (int i = 0; i < verticalCount; i++)
        {
            zValue -= 2;
            xValue = -6;
            for (int j = 0; j < horizontalCount; j++)
            {
                GameObject go = Instantiate(spawnTransform);
                go.transform.SetParent(aiTarget);
                go.transform.position = new Vector3(xValue,yValue , zValue);
                xValue += 2;
                spawnPosList.Add(go.transform);
            }
        }
        StartCoroutine(CollectableSpawner());
    }


    private IEnumerator CollectableSpawner()
    {
        List<Transform> tempList = new List<Transform>();
        
        while (true)
        {
            tempList.Clear();
            foreach (var pos in spawnPosList.Where(x=>x.childCount==0))
            {
                tempList.Add(pos);
            }

            foreach (var spawnPos in tempList)
            {
                var go = ObjectPool.I.GetObjectFromPool();
                if (firstLevelDone)
                {
                    if (go.colorOfThis==ColorType.Red)
                    {
                        go.gameObject.SetActive(true);
                        
                        if (firstGround)
                        {
                            spawnedStacksFirstFloor.Add(go);
                        }
                        
                        go.transform.SetParent(spawnPos);
                        go.transform.localPosition=Vector3.zero;
                    }
                }
                else
                {
                    if (firstGround)
                    {
                        spawnedStacksFirstFloor.Add(go);
                    }
                    else
                    {
                        spawnedStacksSecondFloor.Add(go);
                    }
                    
                    go.gameObject.SetActive(true);
                    Transform transform1;
                    (transform1 = go.transform).SetParent(spawnPos);
                    transform1.localPosition = Vector3.zero;
                    transform1.localEulerAngles = Vector3.zero;
                }
                
            }
            

            yield return new WaitForSeconds(10);

        }
    }

    public void ResetParts(ColorType colorOfThis, bool resetFirstGround)
    {
        if (resetFirstGround)
        {
            for (int i = 0; i < spawnedStacksFirstFloor.Count; i++)
            {
                if (colorOfThis==spawnedStacksFirstFloor[i].colorOfThis)
                {
                    spawnedStacksFirstFloor[i].ResetThis();
                }
            }  
        }
        else
        {
            for (int i = 0; i < spawnedStacksSecondFloor.Count; i++)
            {
                if (colorOfThis == spawnedStacksSecondFloor[i].colorOfThis)
                {
                    spawnedStacksSecondFloor[i].ResetThis();
                }
            }  
        }
      
    }
    
    
    
}
