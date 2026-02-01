using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private static readonly int IsRunningAnimString = Animator.StringToHash("isRunning");
    private static readonly int IsJumpingAnimString = Animator.StringToHash("isJumping");
    private static readonly int DashAnimString = Animator.StringToHash("dash");
    private static readonly int AttackAnimString = Animator.StringToHash("attack");
    
    GlobalInputs m_Inputs;
    GlobalInputs.PlayerActions m_PlayerActions;
    Rigidbody2D m_Rb2d;

    private float m_MoveInput;
    // Jump
    private bool m_IsJumping;
    private bool m_jumpBuffered;
    private bool m_JumpInputReleased;
    private bool m_IsMaxJumpRoutineRunning;
    // Dash
    private bool m_IsDashing;
    private float m_CurrentDashTime;
    private Vector2 m_DashVelocity;
    private bool m_CanDash = true;
    // Attack
    private bool m_CanAttack = true;
    // Invincibility
    private bool m_IsInvincible;
    
    private bool m_isGrounded => Physics2D.Raycast(transform.position, -Vector2.up, transform.localScale.y + 0.01f, m_GroundLayer);

    [SerializeField] private float m_InvincibilityDuration = 1f;
    
    [Header("Refs")]
    [SerializeField] private Animator m_Animator;
    [SerializeField] private GameObject m_Visuals;

    [Header("Movement")]
    [SerializeField] private float m_MoveSpeed = 15f;
    [SerializeField] private LayerMask m_GroundLayer;
    [SerializeField] private float m_Gravity = 9.81f;
    [SerializeField] private float m_AirGravityMultiplier = 3f;
    [SerializeField] private float m_GroundLerpVelocity = 0.5f;
    [SerializeField] private float m_AirLerpVelocity = 0.05f;
    
    [Header("Jump")]
    [SerializeField] private float m_JumpForce = 15f;
    [SerializeField] private float m_JumpMoveMult = 10f;
    [SerializeField] private float m_JumpBuffer = 0.1f;
    [SerializeField] private float m_JumpMinDuration = 0.05f;
    [SerializeField] private float m_JumpMaxDuration = 0.1f;
    
    [Header("Dash")]
    [SerializeField] private float m_DashDuration = 0.15f;
    [SerializeField] private float m_DashSpeed = 25f;
    [SerializeField] private float m_DashEndVelDiviser = 10f;

    [Header("Attack")]
    [SerializeField] private GameObject m_AttackColliders;
    [SerializeField] private float m_AttackDuration = 0.2f;
    [SerializeField] private float m_AttackCooldown = 0.2f;
    
    [Header("Event")]
    [SerializeField] private EV_MalusEvent m_MalusEvent;
    
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
        if (m_IsDashing)
            DashPlayer();
        if (m_IsJumping || (m_jumpBuffered && m_isGrounded))
            JumpPlayer();
        else
            MovePlayer();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Projectile")) return;
        if (m_IsInvincible) return;
        
        m_IsInvincible = true;
        print("HIT!");
        //EVENT HIT
        m_MalusEvent.CallMalus();
        m_Rb2d.linearVelocity /= 5f;
        Camera.main.transform.DOShakePosition(0.1f, 1f, 2);
        StartCoroutine(InvincibilityRoutine());
    }

    IEnumerator InvincibilityRoutine()
    {
        yield return new WaitForSeconds(m_InvincibilityDuration);
        m_IsInvincible = false;
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
        Vector3 scale = m_Visuals.transform.localScale;
        switch (value)
        {
            case > 0:
                m_AttackColliders.transform.localPosition = Vector2.right;
                m_Visuals.transform.localScale = new Vector3(-Mathf.Abs(scale.x), scale.y, scale.z);
                break;
            case < 0:
                m_AttackColliders.transform.localPosition = Vector2.left;
                m_Visuals.transform.localScale = new Vector3(Mathf.Abs(scale.x), scale.y, scale.z);
                break;
            default:
                break;
        }
    }

    void HandleJumpInput()
    {
        m_JumpInputReleased = false;
     
        if (m_isGrounded)
        {
            m_IsJumping = true;
            m_Animator.SetBool(IsJumpingAnimString, true);
            AudioManager.PlayOneShot(SoundType.Jump);
            m_Animator.SetBool(IsRunningAnimString, false);
        }
        else
            StartJumpBuffer();
    }

    void HandleJumpStopInput()
    {
        if (m_IsMaxJumpRoutineRunning)
        {
            StopCoroutine(MaxJumpRoutine());
            m_IsJumping = false;
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
        if (!m_CanDash || m_MoveInput == 0f || m_IsJumping) return;
        
        m_CanDash = false;
        m_IsDashing = true;
        m_DashVelocity = new Vector2(m_MoveInput, 0).normalized * m_DashSpeed;
        m_Animator.SetTrigger(DashAnimString);
        AudioManager.PlayOneShot(SoundType.Dash);
    }

    void HandleAttackInput()
    {
        AttackPlayer();
    }
    
    #endregion
    
    void MovePlayer()
    {
        Vector2 moveDir = new Vector2(m_MoveInput * m_MoveSpeed, -m_Gravity);
        
        if (m_isGrounded)
        {
            if (m_Animator.GetBool(IsJumpingAnimString))
                m_Animator.SetBool(IsJumpingAnimString, false);
            m_Animator.SetBool(IsRunningAnimString, m_MoveInput != 0f);
        }
        
        if (!m_isGrounded)
            moveDir.y *= m_AirGravityMultiplier;
        else if (!m_CanDash)
            m_CanDash = true;
        
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
            m_IsJumping = false;
        else
            StartCoroutine(MaxJumpRoutine());
    }

    IEnumerator MaxJumpRoutine()
    {
        m_IsMaxJumpRoutineRunning = true;
        yield return new WaitForSeconds(m_JumpMaxDuration);
        m_IsMaxJumpRoutineRunning = false;
        m_IsJumping = false;
    }

    void DashPlayer()
    {
        
        m_CurrentDashTime += Time.fixedDeltaTime;
            
        m_Rb2d.linearVelocity = m_DashVelocity;

        if(m_CurrentDashTime >= m_DashDuration)
        {
            m_IsDashing = false;
            m_CurrentDashTime = 0f;
            m_Rb2d.linearVelocity /= m_DashEndVelDiviser;
        }
        
    }

    void AttackPlayer()
    {
        if (!m_CanAttack || m_IsDashing) return;

        
        m_CanAttack = false;
        m_AttackColliders.SetActive(true);
        m_Animator.SetTrigger(AttackAnimString);
        AudioManager.PlayOneShot(SoundType.Attack);
        StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        yield return new WaitForSeconds(m_AttackDuration);
        m_AttackColliders.SetActive(false);
        StartCoroutine(AttackCooldownRoutine());
    }

    IEnumerator AttackCooldownRoutine()
    {
        yield return new WaitForSeconds(m_AttackCooldown);
        m_CanAttack = true;
    }

    public void PushAway(Vector2 dir)
    {
        m_Rb2d.linearVelocity = dir;
    }
}
