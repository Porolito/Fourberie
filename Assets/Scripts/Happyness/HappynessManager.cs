using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Timeline;
using UnityEngine;

public class HappynessManager : MonoBehaviour, IEV_MalusEvent, IEV_BonusEvent
{
    public int malusCount { get; private set; }

    private List<SpriteRenderer> _publicMasks = new ();
    private AudioSource m_AudioSource;
    
    [Header("Audio")]
    [SerializeField] private AudioClip m_AngryClip;
    [SerializeField] private AudioClip m_HappyClip;
    
    [Header("Events")]
    [SerializeField] private EV_MalusEvent m_MalusEvent;
    [SerializeField] private EV_BonusEvent m_bonusEvent;
    
    [Header("Sprites")]
    [SerializeField] private Sprite m_HappyMask;
    [SerializeField] private Sprite m_AngryMask;
    
    void Start()
    {
        var masksGO = GameObject.FindGameObjectsWithTag("PublicMask");
        foreach (var go in masksGO)
        {
            _publicMasks.Add(go.GetComponent<SpriteRenderer>());
        }
        m_MalusEvent.Unregister();
        m_bonusEvent.Unregister();
        m_MalusEvent.Register(this);
        m_bonusEvent.Register(this);
        
        m_AudioSource = GetComponent<AudioSource>();
    }
    
    public void OnMalusReceived()
    {
        malusCount++;
        if (!m_AudioSource.isPlaying)
        {
            m_AudioSource.clip = m_AngryClip;
            m_AudioSource.Play();
        }
        StopCoroutine(nameof(ChangeMaskRoutine));
        ChangePublicMask(m_AngryMask);
    }
   
    public void OnBonusReceived()
    {
        malusCount--;
        if (!m_AudioSource.isPlaying)
        {
            m_AudioSource.clip = m_HappyClip;
            m_AudioSource.Play();
        }
        StopCoroutine(nameof(ChangeMaskRoutine));
        ChangePublicMask(m_HappyMask, false);
    }

    private void ChangePublicMask(Sprite mask, bool withRoutine = true)
    {
        foreach (var go in _publicMasks)
        {
            go.sprite = mask;
        }
        
        if (withRoutine)
            StartCoroutine(ChangeMaskRoutine());
    }

    IEnumerator ChangeMaskRoutine()
    {
        yield return new WaitForSeconds(3f);
        ChangePublicMask(m_HappyMask, false);
    }

}
