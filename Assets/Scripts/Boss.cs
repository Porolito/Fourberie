using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Timeline;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private static readonly int HitAnimString = Animator.StringToHash("hit");
    Animator m_Animator;
    
    private bool m_CanTakeDamages;
    private int m_CurrentHealth;
    float m_StartPosY;
    
    [Header("Events")]
    [SerializeField] private SO_GameEvent m_ChallengeSuccessGE;
    [SerializeField] private EV_BonusEvent m_BonusEvent;

    [Header("Settings")]
    [SerializeField] private int m_HealthPerPhase = 9;
    [SerializeField] private float m_MoveDuration = 4f;
    [SerializeField] private float m_MoveDistance = 10f;

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
        m_StartPosY = transform.position.y;
        transform.position = new Vector2(transform.position.x, transform.position.y + m_MoveDistance);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("PlayerAttack") || !m_CanTakeDamages) return;
        Hurt();
    }
    
    void Hurt()
    {
        m_Animator.SetTrigger(HitAnimString);
        m_CurrentHealth--;
        if (m_CurrentHealth <= 0) EndPhase();
    }

    public void StartPhase()
    {
        transform.DOMoveY(m_StartPosY, m_MoveDuration).SetEase(Ease.OutCubic);
        m_CurrentHealth = m_HealthPerPhase;
        m_CanTakeDamages = true;
    }

    void EndPhase()
    {
        transform.DOMoveY(m_MoveDistance, m_MoveDuration).SetEase(Ease.InCubic);
        m_CanTakeDamages = false;
        m_BonusEvent.CallBonus();
        m_ChallengeSuccessGE.Trigger();
    }
}
