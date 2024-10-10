using Obvious.Soap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Inputs")]
    [SerializeField] private Vector3Variable _movementInputs;
    [SerializeField] private BoolVariable _dashInput;
    [SerializeField] private BoolVariable _slideInput;
    [SerializeField] private FloatVariable _tapInput;

    [Header("Player Resources")]
    [SerializeField] private FloatVariable _energy;

    [Header("Speed Variables")]
    [SerializeField] private float currentSpeed;
    [SerializeField] private FloatVariable _speedMax;
    [SerializeField] private float acceleration;
    [SerializeField] private float decceleration;

    [Header("Speed Variables")]
    [SerializeField] private float tapSpeedBoost;
    [SerializeField] private float tapCost;

    [Header("Dash")]
    [SerializeField] private bool isDashing = false;
    [SerializeField] private FloatVariable _dash;
    [SerializeField] private float dashDecay;
    [SerializeField] private float dashCost;
    [SerializeField] private float dashDuration;
    private Coroutine dashCoroutine;

    [Header("Slide")]
    [SerializeField] private bool isSliding = false;
    [SerializeField] private FloatVariable _kineticSlideInterval;
    [SerializeField] private float slideTreshold;
    [SerializeField] private float slideFriction;
    [SerializeField] private FloatVariable _slideEnergyRegen;
    private Coroutine slideCoroutine;



    private void Update()
    {
        HandleMovement();
        HandleDash();
        HandleSliding();
    }

    void HandleMovement()
    {
        if(_movementInputs.Value.magnitude > 0)
        {
            if(_tapInput.Value > 0)
            {
                if(_energy.Value >= tapCost)
                {
                    Vector3 dashDirection = _movementInputs.Value.normalized;
                    currentSpeed = Mathf.Min(currentSpeed + tapSpeedBoost, _speedMax);
                    transform.position += dashDirection * tapSpeedBoost;
                    _energy.Value -= tapCost;
                    _tapInput.Value = 0;
                }
            }

            if(!isDashing)
            {
                currentSpeed = Mathf.Min(currentSpeed + acceleration * Time.deltaTime, _speedMax);
            }
            
        }
        else if(!isSliding)
        {
            //Speed Decay
            currentSpeed = Mathf.Max(currentSpeed - decceleration * Time.deltaTime, 0);
        }

        //Apply Movement
        transform.position += _movementInputs.Value * currentSpeed * Time.deltaTime;
    }

    void HandleDash()
    {
        if(_dashInput && !isDashing && _energy.Value >= dashCost)
        {
            dashCoroutine = StartCoroutine(Dash());
            _energy.Value -= dashCost;
        }
    }

    IEnumerator Dash()
    {
        isDashing = true;

        currentSpeed += _dash;

        yield return new WaitForSeconds(dashDuration);

        if(currentSpeed > _speedMax)
        {
            currentSpeed = _speedMax;
        }

        isDashing = false;
    }

    void HandleSliding()
    {
        if(currentSpeed > slideTreshold && _slideInput && !isSliding)
        {
            isSliding = true;
            slideCoroutine = StartCoroutine(KineticSlide());
        }

        if(isSliding)
        {
            currentSpeed = Mathf.Max(currentSpeed - slideFriction * Time.deltaTime, 0);

            if (currentSpeed <= 0) // Stop sliding when speed is 0
            {
                isSliding = false;
                StopCoroutine(slideCoroutine);
            }

            if (!_slideInput) // Stop sliding when the key is released
            {
                isSliding = false;
                StopCoroutine(slideCoroutine);
            }
        }
    }

    IEnumerator KineticSlide()
    {
        while(isSliding)
        {
            if(_movementInputs.Value.magnitude > 0)
            {
                _energy.Value += _slideEnergyRegen;
            }

            yield return new WaitForSeconds(_kineticSlideInterval);
        }
    }
}
