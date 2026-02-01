using System;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public static DialogManager instance;
    
    private AudioSource m_AudioSource;
    
    [Header("Refs")]
    [SerializeField] private TextMeshProUGUI m_NarratorSubtitle;
    [SerializeField] private Image m_NarratorExpression;
    
    [Header("Expressions Face")]
    [SerializeField] private Sprite m_NeutralFace;
    [SerializeField] private Sprite m_SadFace;
    [SerializeField] private Sprite m_HappyFace;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        
        m_AudioSource = GetComponent<AudioSource>();
    }

    public void PlayDialog(DialogPartData dialog)
    {
        m_AudioSource.Stop();
        m_AudioSource.clip = dialog.clip;
        m_NarratorSubtitle.text = dialog.subtitle;
        m_NarratorExpression.sprite = GetExpressionSprite(dialog.expression);
        m_AudioSource.Play();
    }
    
    Sprite GetExpressionSprite(DialogPartData.Expression expression)
    {
        return expression switch
        {
            DialogPartData.Expression.Neutral => m_NeutralFace,
            DialogPartData.Expression.Sad => m_SadFace,
            DialogPartData.Expression.Happy => m_HappyFace,
            _ => m_NeutralFace
        };
    }
    void DisplaySubtitle()
    {
        
    }
}
