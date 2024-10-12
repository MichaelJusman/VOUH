using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Obvious.Soap;
using Unity.VisualScripting;

public class PlayerInputs : GameBehaviour
{
    [Header("Movement")]
    [SerializeField] private Vector3Variable _movementInput;
    [SerializeField] private BoolVariable _dashInput;
    [SerializeField] private BoolVariable _slideInput;

    [Header("Tap")]
    [SerializeField] private FloatVariable _tapInput;
    [SerializeField] private float tapTreshold;
    [SerializeField] private float movementKeyTimer = 0;
    [SerializeField] private bool isMovementKeyHeld = false;


    private void Update()
    {
        // Movement inputs
        _movementInput.Value = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        // Tap detection
        if (_movementInput.Value.magnitude > 0) //If detects any movement input
        {
            if (!isMovementKeyHeld)
            {
                isMovementKeyHeld = true;
                movementKeyTimer = 0;
            }
            else { movementKeyTimer += Time.deltaTime; } //Start timer
        }
        else if(isMovementKeyHeld) //Release
        {
            if(movementKeyTimer <= tapTreshold)
            {
                _tapInput.Value = 1;

                ExecuteAfterFrames(3, ()=> _tapInput.Value = 0);
                
            }
            else
            {
                _tapInput.Value = 0;
            }
            isMovementKeyHeld = false;

        }

        //Dash input
        _dashInput.Value = Input.GetKeyDown(KeyCode.Space);

        //Slide input
        _slideInput.Value = Input.GetKey(KeyCode.LeftShift);
        
    }
}
