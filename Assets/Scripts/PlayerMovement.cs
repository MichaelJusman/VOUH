using Obvious.Soap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Vector3Variable _inputs;
    [SerializeField] private FloatVariable _tempEnergy;

    [Header("Speed Variables")]
    [SerializeField] private float currentSpeed;
    [SerializeField] private FloatVariable _speedMax;
    [SerializeField] private float acceleration;
    [SerializeField] private float decceleration;

    [Header("Speed Variables")]
    [SerializeField] private float tapSpeedBoost;
    [SerializeField] private float tapCost;

    [Header("Dash")]
    [SerializeField] private FloatVariable _dash;
    [SerializeField] private float dashDecay;
    [SerializeField] private float dashCost;

    [Header("Slide")]
    [SerializeField] private bool isSliding = false;
    [SerializeField] private float slideTreshold;
    [SerializeField] private float slideFriction;
    [SerializeField] private FloatVariable slideEnergyRegen;



    private void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        if(_inputs.Value.magnitude > 0)
        {
            if(Input.GetKeyDown(KeyCode.A) ||  Input.GetKeyDown(KeyCode.D))
            {
                if(slideEnergyRegen >= tapCost)
                {
                    currentSpeed = Mathf.Min(currentSpeed + tapSpeedBoost, _speedMax);
                    _tempEnergy.Value -= tapCost;
                }
            }

            //Speed Buildup
            currentSpeed = Mathf.Min(currentSpeed + acceleration * Time.deltaTime, _speedMax);
        }
        else if(!isSliding)
        {
            //Speed Decay
            currentSpeed = Mathf.Max(currentSpeed - decceleration * Time.deltaTime, 0);
        }

        //Apply Movement
        transform.position += _inputs.Value * currentSpeed * Time.deltaTime;
    }
}
