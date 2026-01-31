using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    GlobalInputs m_Inputs;
    GlobalInputs.PlayerActions m_PlayerActions;
    
    private void Awake()
    {
        m_Inputs = new GlobalInputs();
        m_PlayerActions = m_Inputs.Player;
        SubInputs();
        m_Inputs.Enable();
    }

    void SubInputs()
    {
        m_PlayerActions.Move.performed += ctx => HandleMovement(ctx.ReadValue<float>());
        m_PlayerActions.Jump.started += _ => HandleJump();
    }
    
    void HandleMovement(float value)
    {
        Debug.Log(value);
    }

    void HandleJump()
    {
        
    }

    void HandleDash()
    {
        
    }

    void HandleAttack()
    {
        
    }
}
