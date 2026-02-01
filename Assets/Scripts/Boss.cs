using System;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private static readonly int HitAnimString = Animator.StringToHash("hit");
    Animator m_Animator;
    
    public bool isInDamagePhase;

    [Header("Settings")]
    [SerializeField] private int m_HealthPerPhase = 9;
    [SerializeField] private int m_HitToPush = 3;
    [SerializeField] private float m_TimeToResetPush = 3f;
    [SerializeField] private float m_PushForce = 40f;

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("PlayerAttack") || !isInDamagePhase) return;
        
        m_Animator.SetTrigger(HitAnimString);
        Vector2 pushDir = other.transform.position - transform.position;
        other.GetComponentInParent<Player>().PushAway(pushDir * m_PushForce);
    }
}
