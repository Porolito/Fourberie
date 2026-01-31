using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    GlobalInputs m_Inputs;
    GlobalInputs.PlayerActions m_PlayerActions;
    Rigidbody2D m_Rb2d;

    private float m_MoveInput;
    private bool m_isJumping;
    private bool m_jumpBuffered;
    private bool m_JumpInputReleased;
    private bool m_IsMaxJumpRoutineRunning;
    
    private bool m_isGrounded => Physics2D.Raycast(transform.position, -Vector2.up, 1.05f, m_GroundLayer);

    [Header("Base")]
    [SerializeField] private float m_MoveSpeed = 50f;
    [SerializeField] private float m_JumpForce = 10f;
    [SerializeField] private float m_JumpMoveMult = 1.3f;
    [SerializeField] private float m_JumpBuffer = 0.1f;
    [SerializeField] private float m_JumpMinDuration = 0.1f;
    [SerializeField] private float m_JumpMaxDuration = 0.5f;
    
    [Header("Physics")]
    [SerializeField] private LayerMask m_GroundLayer;
    [SerializeField] private float m_Gravity = 9.81f;
    [SerializeField] private float m_AirGravityMultiplier = 3f;
    [SerializeField] private float m_GroundLerpVelocity = .5f;
    [SerializeField] private float m_AirLerpVelocity = .05f;
    
    private void Awake()
    {
        m_Inputs = new GlobalInputs();
        m_PlayerActions = m_Inputs.Player;
        SubInputs();
        m_Inputs.Enable();
        
        m_Rb2d = GetComponent<Rigidbody2D>();
    }

    private void OnDestroy()
    {
        UnsubInputs();
    }

    private void FixedUpdate()
    {
        if (m_isJumping || (m_jumpBuffered && m_isGrounded))
            JumpPlayer();
        else
            MovePlayer();
    }
    
    #region Inputs

    void SubInputs()
    {
        m_PlayerActions.Move.performed += ctx => HandleMovementInput(ctx.ReadValue<float>());
        m_PlayerActions.Move.canceled += ctx => HandleMovementInput(ctx.ReadValue<float>());
        m_PlayerActions.Jump.started += _ => HandleJumpInput();
        m_PlayerActions.Jump.canceled += _ => HandleJumpStopInput();
        m_PlayerActions.Dash.started += _ => HandleDashInput();
        m_PlayerActions.Attack.started += _ => HandleAttackInput();
    }

    void UnsubInputs()
    {
        m_PlayerActions.Move.performed -= ctx => HandleMovementInput(ctx.ReadValue<float>());
        m_PlayerActions.Move.canceled -= ctx => HandleMovementInput(ctx.ReadValue<float>());
        m_PlayerActions.Jump.started -= _ => HandleJumpInput();
        m_PlayerActions.Jump.canceled += _ => HandleJumpStopInput();
        m_PlayerActions.Dash.started -= _ => HandleDashInput();
        m_PlayerActions.Attack.started -= _ => HandleAttackInput();
    }
    
    void HandleMovementInput(float value)
    {
        m_MoveInput = value;
    }

    void HandleJumpInput()
    {
        m_JumpInputReleased = false;
     
        if (m_isGrounded)
            m_isJumping = true;
        else
            StartJumpBuffer();
    }

    void HandleJumpStopInput()
    {
        if (m_IsMaxJumpRoutineRunning)
        {
            StopCoroutine(MaxJumpRoutine());
            m_isJumping = false;
        }
        m_JumpInputReleased = true;
    }
    
    private void StartJumpBuffer()
    {
        m_jumpBuffered = false;
        StopCoroutine(JumpBufferRoutine());
        StartCoroutine(JumpBufferRoutine());
    }

    private IEnumerator JumpBufferRoutine()
    {
        m_jumpBuffered = true;
        yield return new WaitForSeconds(m_JumpBuffer);
        m_jumpBuffered = false;
    }

    void HandleDashInput()
    {
        DashPlayer();
    }

    void HandleAttackInput()
    {
        AttackPlayer();
    }
    
    #endregion
    
    void MovePlayer()
    {
        Vector2 moveDir = new Vector2(m_MoveInput * m_MoveSpeed, -m_Gravity);
        if (!m_isGrounded)
            moveDir.y *= m_AirGravityMultiplier;
        
        m_Rb2d.linearVelocity = Vector2.Lerp(m_Rb2d.linearVelocity, moveDir, m_isGrounded ? m_GroundLerpVelocity : m_AirLerpVelocity);
    }

    void JumpPlayer()
    {
        m_Rb2d.linearVelocity = new Vector2(m_MoveInput * m_JumpMoveMult, m_JumpForce);
        StartCoroutine(MinJumpRoutine());
    }

    IEnumerator MinJumpRoutine()
    {
        yield return new WaitForSeconds(m_JumpMinDuration);
        if (m_JumpInputReleased)
            m_isJumping = false;
        else
            StartCoroutine(MaxJumpRoutine());
    }

    IEnumerator MaxJumpRoutine()
    {
        m_IsMaxJumpRoutineRunning = true;
        yield return new WaitForSeconds(m_JumpMaxDuration);
        m_IsMaxJumpRoutineRunning = false;
        m_isJumping = false;
    }

    void DashPlayer()
    {
        
    }

    void AttackPlayer()
    {
        
    }
}
