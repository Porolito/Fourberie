using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private static readonly int HitAnimString = Animator.StringToHash("hit");
    Animator m_Animator;
    
    private bool m_CanTakeDamages;
    private int m_CurrentHealth;
    private int m_CurrentHitToPush;
    private bool m_PushCooldownStarted;

    [Header("Settings")]
    [SerializeField] private int m_HealthPerPhase = 9;
    [SerializeField] private int m_HitToPush = 3;
    [SerializeField] private float m_TimeToResetPush = 2f;
    [SerializeField] private float m_PushForce = 40f;

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
        StartPhase(true);
    }

    public void StartPhase(bool canTakeDamages)
    {
        m_CurrentHealth = m_HealthPerPhase;
        m_CurrentHitToPush = m_HitToPush;
        m_CanTakeDamages = canTakeDamages;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("PlayerAttack") || !m_CanTakeDamages) return;
        
        Hurt(other.GetComponentInParent<Player>());
    }

    void Hurt(Player player)
    {
        m_Animator.SetTrigger(HitAnimString);
        
        m_CurrentHealth--;
        if (m_CurrentHealth <= 0) EndPhase(player);

        m_CurrentHitToPush--;
        if (!m_PushCooldownStarted) StartCoroutine(PushCooldown());
        if (m_CurrentHitToPush <= 0) PushPlayerAway(player);
    }

    IEnumerator PushCooldown()
    {
        print("start cd");
        m_PushCooldownStarted = true;
        yield return new WaitForSeconds(m_TimeToResetPush);
        print("cd end: " + m_CurrentHitToPush);
        m_PushCooldownStarted = false;
        m_CurrentHitToPush = m_HitToPush;
    } 

    void PushPlayerAway(Player player)
    {
        m_CurrentHitToPush = m_HitToPush;
        Vector2 pushDir = (Vector2.left + Vector2.up) * m_PushForce;
        player.PushAway(pushDir);
        StopCoroutine(PushCooldown());
    }

    void EndPhase(Player player)
    {
        //TODO appel du timeline pour next
        m_CanTakeDamages = false;
        PushPlayerAway(player);
        print("END OF PHASE");
    }
}
