using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool I;
    private int amountStack = 200;
    
    private List<PartController> pooledObjects = new List<PartController>();

    [SerializeField] private PartController partPrefab;

    private void Awake()
    {
        if (I==null)
        {
            I = this;
        }

        StartSpawn();
    }

    private void StartSpawn()
    {
        for (int i = 0; i < amountStack; i++)
        {
            var part = Instantiate(partPrefab, transform, true);

            part.gameObject.SetActive(false);
            pooledObjects.Add(part);
            
            if (i%2==0)
            {
                part.colorOfThis = ColorType.Red;
            }
            else
            {
                part.colorOfThis = ColorType.Green;
            }
            part.SetThis();
        }
    }



    public PartController GetObjectFromPool()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].gameObject.activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }

        return null;
    }
    
}
