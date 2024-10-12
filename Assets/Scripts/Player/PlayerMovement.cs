using Obvious.Soap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public enum PlayerState { Idle, Walk, Run, Dash, Slide, Tap, Warp}
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
    [SerializeField] private Vector3 dashDirection;
    private Coroutine dashCoroutine;

    [Header("Slide")]
    [SerializeField] private bool isSliding = false;
    [SerializeField] private FloatVariable _kineticSlideInterval;
    [SerializeField] private float slideTreshold;
    [SerializeField] private float slideFriction;
    [SerializeField] private FloatVariable _slideEnergyRegen;
    public Coroutine slideCoroutine;
    private float slideTimeElapsed;

    private void Start()
    {
        state = PlayerState.Idle;
    }

    private void Update()
    {
        //HandleMovement();
        //HandleDash();
        //HandleSliding();

        StateSelect();
        StateMachine();
        HandleTap();
    }

    void StateSelect()
    {
        // Slide has first priority
        if (_slideInput && !isSliding && currentSpeed > slideTreshold)
        {
            state = PlayerState.Slide;
        }
        // Dash takes the second priority
        else if (_dashInput && !isDashing && _energy.Value >= dashCost)
        {
            state = PlayerState.Dash;
        }
        //// Tap action has third priority
        //else if (_tapInput.Value > 0 && _energy.Value >= tapCost)
        //{
        //    state = PlayerState.Tap;
        //}

        // Movement states (Run, Walk, Idle) come after
        else if (_movementInputs.Value.magnitude > 0)
        {
            if (currentSpeed < slideTreshold && !_slideInput)
            {
                state = PlayerState.Walk;

                
            }
            else
            {
                if(!isDashing && !_slideInput)
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

            case PlayerState.Warp: 

                break;
        }
    }

    void HandleIdleState()
    {
        currentSpeed = Mathf.Max(currentSpeed - decceleration * Time.deltaTime, 0);
    }

    void HandleWalkState()
    {
        currentSpeed = Mathf.Min(currentSpeed + acceleration * Time.deltaTime, _speedMax);
        transform.position += _movementInputs.Value * currentSpeed * Time.deltaTime;
    }

    void HandleRunState()
    {
        currentSpeed = Mathf.Min(currentSpeed + acceleration * Time.deltaTime, _speedMax);
        transform.position += _movementInputs.Value * currentSpeed * Time.deltaTime;
        //run anim
        //run particles
    }

    void HandleDashState()
    {
        if (!isDashing)
        {
            // Capture the direction when dash starts
            dashDirection = _movementInputs.Value.magnitude > 0 ? _movementInputs.Value.normalized : transform.forward;

            isDashing = true;
            dashCoroutine = StartCoroutine(Dash());
            _energy.Value -= dashCost;
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
            currentSpeed = Mathf.Max(currentSpeed - slideFriction * Time.deltaTime, 0);
            transform.position += _movementInputs.Value * currentSpeed * Time.deltaTime;

            slideTimeElapsed += Time.deltaTime;
            if (slideTimeElapsed >= _kineticSlideInterval && currentSpeed > 0)
            {
                _energy.Value += _slideEnergyRegen;
                slideTimeElapsed = 0f;  // Reset timer after regenerating energy
            }

            // Stop sliding when speed is 0
            if (currentSpeed <= 0)
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
        Vector3 dashDirection = _movementInputs.Value.normalized;
        currentSpeed = Mathf.Min(currentSpeed + tapSpeedBoost, _speedMax);
        transform.position += dashDirection * tapSpeedBoost;
        _energy.Value -= tapCost;
        _tapInput.Value = 0;
    }

    void HandleIdle()
    {
        if(!isSliding) { currentSpeed = Mathf.Max(currentSpeed - decceleration * Time.deltaTime, 0); }
    }

    void HandleWalk()
    {
        if (_movementInputs.Value.magnitude > 0)
        {
            if (_tapInput.Value > 0)
            {
                if (_energy.Value >= tapCost)
                {
                    Vector3 dashDirection = _movementInputs.Value.normalized;
                    currentSpeed = Mathf.Min(currentSpeed + tapSpeedBoost, _speedMax);
                    transform.position += dashDirection * tapSpeedBoost;
                    _energy.Value -= tapCost;
                    _tapInput.Value = 0;
                }
            }

            if (!isDashing)
            {
                currentSpeed = Mathf.Min(currentSpeed + acceleration * Time.deltaTime, _speedMax);
            }

        }
        else if (!isSliding)
        {
            //Speed Decay
            currentSpeed = Mathf.Max(currentSpeed - decceleration * Time.deltaTime, 0);
        }

        //Apply Movement
        transform.position += _movementInputs.Value * currentSpeed * Time.deltaTime;
    }

    void HandleTap()
    {
        if (_movementInputs.Value.magnitude > 0)
        {
            if (_tapInput.Value > 0)
            {
                if (_energy.Value >= tapCost)
                {
                    Vector3 dashDirection = _movementInputs.Value.normalized;
                    currentSpeed = Mathf.Min(currentSpeed + tapSpeedBoost, _speedMax);
                    transform.position += dashDirection * tapSpeedBoost;
                    _energy.Value -= tapCost;
                    _tapInput.Value = 0;
                }
            }
        }
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
        Debug.Log("Dash Started");
        // Maintain initial direction but allow movement inputs
        currentSpeed += _dash;
        float dashTime = 0;
        while (dashTime < dashDuration)
        {
            // Allow new inputs during the dash
            if (_movementInputs.Value.magnitude < 0)
            {
                dashDirection = _movementInputs.Value.normalized;
            }

            // Continue moving in the dash direction
            transform.position += dashDirection * currentSpeed * Time.deltaTime;
            dashTime += Time.deltaTime;
            yield return null;  // Wait for the next frame
        }

        isDashing = false;

        // Decay speed to max speed after dashing
        if (currentSpeed > _speedMax)
        {
            currentSpeed = Mathf.Min(currentSpeed + _dash, _speedMax);

        }
        Debug.Log("Dash Ended");
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
        while(isSliding && currentSpeed > 0)
        {
            _energy.Value += _slideEnergyRegen;

            yield return new WaitForSeconds(_kineticSlideInterval);
        }
    }
}
