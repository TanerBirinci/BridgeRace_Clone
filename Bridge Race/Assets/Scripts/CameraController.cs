using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Lean.Touch;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
  

    private void FixedUpdate()
    {
        if (GameManager.I.isGameFinished) return;
        
        transform.position=Vector3.Lerp(transform.position,player.position,0.15f);
    }
    
}
