using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class LadderController : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private GameObject blocker;
    [SerializeField] private Material playerMaterial;
    [SerializeField] private Material aiMaterial;

    public ColorType colorOfLadder;
 
    
    public void OpenLadder(bool isPlayer)
    {
        ChangeLadderColor(isPlayer);

        blocker.SetActive(!isPlayer);

        meshRenderer.enabled = true;
    }

    public void ChangeLadderColor(bool isPlayer)
    {
        meshRenderer.material = isPlayer ? playerMaterial : aiMaterial;
    }
}
