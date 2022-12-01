using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private FloatingJoystick joystick;
    [SerializeField] public Animator playerAnim;
    private bool _canMove;

    public Rigidbody rigidBody;

    [Header("Player Step Climb")] 
    [SerializeField] private GameObject stepRayUpper;
    
    [SerializeField] private GameObject stepRayLower;
    
    [SerializeField] private float stepHeight;
    
    [SerializeField] private float stepSmooth;
    
    private Vector3 _moveVector;
    private static readonly int Run1 = Animator.StringToHash("Run");

    public void Awake()
    {
        var position = stepRayUpper.transform.position;
        
        position = new Vector3(position.x, stepHeight,
            position.z);
        stepRayUpper.transform.position = position;
    }

    private void Update()
    {
        if(GameManager.I.isGameFinished || !GameManager.I.isGameStarted) return;
        
        if (Input.GetMouseButtonDown(0))
        {
            playerAnim.SetBool(Run1, true);

            _canMove = true;

        }

        if (Input.GetMouseButtonUp(0))
        {
            
            playerAnim.SetBool(Run1, false);
            _canMove = false;
        }
        
        StepClimb();
    }

    private void Run()
    {
        _moveVector = new Vector3
            (joystick.Horizontal, 0, joystick.Vertical).normalized * (2 * Time.deltaTime);
        _moveVector = Quaternion.Euler(0, 30, 0) * _moveVector;
        
        
        var position = transform.position;
        transform.LookAt(position + _moveVector);
        
        position = Vector3.Lerp(position, position +(transform.forward*movementSpeed),0.1f);
        transform.position = position;
    }


    private void FixedUpdate()
    {
        if(GameManager.I.isGameFinished || !GameManager.I.isGameStarted) return;

        if (_canMove)
        {
            Run();
        }
    }

    private void StepClimb()
    {
         RaycastHit hitLower;
        if (Physics.Raycast(stepRayLower.transform.position, transform.TransformDirection(Vector3.forward),out hitLower,0.2f))
        {
            RaycastHit hitUpper;
            if (!Physics.Raycast(stepRayUpper.transform.position,transform.TransformDirection(Vector3.forward),out hitUpper,0.2f))
            {
                 rigidBody.position-= new Vector3(0, -stepSmooth, 0f);
            }
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("FallTrigger"))
        {
            LevelFailed();
        }
    }
    
    public void LevelFinished()
    {
        playerAnim.SetBool(Run1, false);
    }
    

    private void LevelFailed()
    {
        rigidBody.isKinematic = true;
    }

    
}
