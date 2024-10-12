using Obvious.Soap;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public enum PlayerState { Idle, Walk, Run, Dash, Slide, Tap}
public class PlayerMovement : GameBehaviour
{
    [Header("State Machine")]
    public PlayerState state;

    [Header("Inputs")]
    [SerializeField] private Vector3Variable _movementInputs;
    [SerializeField] private BoolVariable _dashInput;
    [SerializeField] private BoolVariable _slideInput;
    [SerializeField] private FloatVariable _tapInput;

    [Header("Player Resources")]
    [SerializeField] private FloatVariable _energy;

    [Header("Speed Variables")]
    [SerializeField] private float _speedCurrent;
    [SerializeField] private FloatVariable _speedMax;
    [SerializeField] private float _speedAcceleration;
    [SerializeField] private float _speedDeceleration;
    [SerializeField] private float _speedRunTreshold;

    [Header("Tap Dash")]
    [SerializeField] private bool _isTapping = false;
    [SerializeField] private float _tapSpeedBoost;
    [SerializeField] private float _tapCost;
    [SerializeField] private float _tapDuration;
    private Coroutine tapCoroutine;

    [Header("Dash")]
    [SerializeField] private bool _isDashing = false;
    [SerializeField] private FloatVariable _dash;
    [SerializeField] private float _dashCost;
    [SerializeField] private float _dashDuration;
    [SerializeField] private Vector3 dashDirection;
    private Coroutine dashCoroutine;

    [Header("Slide")]
    [SerializeField] private bool _isSliding = false;
    [SerializeField] private FloatVariable _kineticSlideInterval;
    [SerializeField] private float _slideFriction;
    [SerializeField] private FloatVariable _slideEnergyRegen;
    [SerializeField] private float slideTimeElapsed;
    private Coroutine slideCoroutine;

    private void Start()
    {
        state = PlayerState.Idle;
    }

    private void Update()
    {
        StateSelect();
        StateMachine();
        //HandleTap();

        if (_movementInputs.Value.magnitude > 0)
        {
            // Rotate the player to face the movement direction
            Quaternion targetRotation = Quaternion.LookRotation(_movementInputs.Value);
            transform.rotation = targetRotation;
        }
    }

    void StateSelect()
    {
        
        //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        // Slide has first priority
        if (_slideInput && !_isSliding && _speedCurrent > _speedRunTreshold)
        {
            state = PlayerState.Slide;
        }
        // Dash takes the second priority
        else if (_dashInput && !_isDashing && _energy.Value >= _dashCost)
        {
            state = PlayerState.Dash;
        }
        else if (_tapInput.Value > 0 && !_isTapping && _energy.Value >= _tapCost)
        {
            state = PlayerState.Tap;
        }
        // Movement states (Run, Walk, Idle) come after
        else if (_movementInputs.Value.magnitude > 0)
        {
            if (_speedCurrent < _speedRunTreshold && !_slideInput)
            {
                state = PlayerState.Walk;

            }
            else
            {
                if(!_isDashing && !_slideInput)
                {
                    state = PlayerState.Run;
                }

            }
        }
        else
        {
            state = PlayerState.Idle;
        }
    }

    void StateMachine()
    {
        switch (state)
        {
            case PlayerState.Idle:
                HandleIdleState();
                break;

            case PlayerState.Walk:
                HandleWalkState();
                break;

            case PlayerState.Run: 
                HandleRunState();
                break;

            case PlayerState.Dash:
                HandleDashState();
                break;

            case PlayerState.Slide:
                HandleSlideState();
                break;

            case PlayerState.Tap: 
                HandleTapState();
                break;
        }
    }

    void HandleIdleState()
    {
        _speedCurrent = Mathf.Max(_speedCurrent - _speedDeceleration * Time.deltaTime, 0);
    }

    void HandleWalkState()
    {
        _speedCurrent = Mathf.Min(_speedCurrent + _speedAcceleration * Time.deltaTime, _speedMax);
        transform.position += _movementInputs.Value * _speedCurrent * Time.deltaTime;
    }

    void HandleRunState()
    {
        _speedCurrent = Mathf.Min(_speedCurrent + _speedAcceleration * Time.deltaTime, _speedMax);
        transform.position += _movementInputs.Value * _speedCurrent * Time.deltaTime;
        //run anim
        //run particles
    }

    void HandleDashState()
    {
        if (!_isDashing)
        {
            // Capture the direction when dash starts
            dashDirection = _movementInputs.Value.magnitude > 0 ? _movementInputs.Value.normalized : transform.forward;

            _isDashing = true;
            dashCoroutine = StartCoroutine(Dash());
            _energy.Value -= _dashCost;
        }

        //dash anim
        //dash particles
    }

    void HandleSlideState()
    {
        Debug.Log("Sliding called");

        if (_slideInput)
        {
            // Apply sliding friction
            _speedCurrent = Mathf.Max(_speedCurrent - _slideFriction * Time.deltaTime, 0);
            transform.position += _movementInputs.Value * _speedCurrent * Time.deltaTime;

            slideTimeElapsed += Time.deltaTime;
            if (slideTimeElapsed >= _kineticSlideInterval && _speedCurrent > 0)
            {
                _energy.Value += _slideEnergyRegen;
                slideTimeElapsed = 0f;  // Reset timer after regenerating energy
            }

            // Stop sliding when speed is 0
            if (_speedCurrent <= 0)
            {
                Debug.Log("Slide is attempting to end due to speed reaching 0");
                StopSliding();
            }
        }
        else
        {
            // Stop sliding when input is released
            Debug.Log("Slide is attempting to end due to input release");
            StopSliding();
        }
    }

    void StopSliding()
    {
        if (slideCoroutine != null)
        {
            StopCoroutine(slideCoroutine);
            slideCoroutine = null;
            Debug.Log("Slide has stopped");
        }
    }

    void HandleTapState()
    {
        if (!_isTapping)
        {
            //dashDirection = _movementInputs.Value.magnitude > 0
            //? transform.TransformDirection(_movementInputs.Value.normalized)
            //: transform.forward;
            _isTapping = true;
            tapCoroutine = StartCoroutine(Tap());
            _energy.Value -= _tapCost;
        }
    }
    void HandleTap()
    {
        if (_movementInputs.Value.magnitude > 0)
        {
            if (_tapInput.Value > 0)
            {
                if (_energy.Value >= _tapCost)
                {
                    Vector3 dashDirection = _movementInputs.Value.normalized;
                    _speedCurrent = Mathf.Min(_speedCurrent + _tapSpeedBoost, _speedMax);
                    transform.position += dashDirection * _tapSpeedBoost;
                    _energy.Value -= _tapCost;
                    _tapInput.Value = 0;
                }
            }
        }
    }

    IEnumerator Tap()
    {
        _speedCurrent += _tapSpeedBoost;
        float tapTime = 0;
        while(tapTime < _tapDuration)
        {
            // Allow new inputs during the dash
            //if (_movementInputs.Value.magnitude < 0)
            //{
            //    dashDirection = _movementInputs.Value.normalized;
            //}

            // Continue moving in the dash direction
            transform.position += transform.forward * _speedCurrent * Time.deltaTime;
            tapTime += Time.deltaTime;
            yield return null;  // Wait for the next frame
        }

        _isTapping = false;

        if (_speedCurrent > _speedMax)
        {
            _speedCurrent = Mathf.Min(_speedCurrent + _tapSpeedBoost, _speedMax);

        }
    }



    IEnumerator Dash()
    {
        Debug.Log("Dash Started");
        // Maintain initial direction but allow movement inputs
        _speedCurrent += _dash;
        float dashTime = 0;
        while (dashTime < _dashDuration)
        {
            // Allow new inputs during the dash
            if (_movementInputs.Value.magnitude < 0)
            {
                dashDirection = _movementInputs.Value.normalized;
            }

            // Continue moving in the dash direction
            transform.position += dashDirection * _speedCurrent * Time.deltaTime;
            dashTime += Time.deltaTime;
            yield return null;  // Wait for the next frame
        }

        _isDashing = false;

        // Decay speed to max speed after dashing
        if (_speedCurrent > _speedMax)
        {
            _speedCurrent = Mathf.Min(_speedCurrent + _dash, _speedMax);

        }
        Debug.Log("Dash Ended");
    }
}
