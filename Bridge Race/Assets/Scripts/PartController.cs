using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ColorType
{
    Red,Green,Default
}

public class PartController : MonoBehaviour
{
    public bool isCarrying;
    public ColorType colorOfThis;
    public Material redColor;
    public Material greenColor;
    public MeshRenderer meshRenderer;


    public void ResetThis()
    {
        if(isCarrying) return;
      
        transform.SetParent(ObjectPool.I.transform);
        gameObject.SetActive(false);
    }

    public void DropToLadder()
    {
        isCarrying = false;
        transform.SetParent(ObjectPool.I.transform);
        gameObject.SetActive(false);
    }

    public void SetThis()
    {
        if (colorOfThis==ColorType.Green)
        {
            meshRenderer.material = greenColor;
        }
        else
        {
            meshRenderer.material = redColor;
        }
    }
}
