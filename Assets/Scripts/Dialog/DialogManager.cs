using System;
using System.Collections;
using JetBrains.Annotations;
using Timeline;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DialogManager : MonoBehaviour
{
    public static DialogManager instance;
    
    private int m_DialogIndex;
    
    [Header("Refs")]
    [SerializeField] private TextMeshProUGUI m_NarratorSubtitle;
    [SerializeField] private Image m_NarratorExpression;
    
    [Header("Expressions Face")]
    [SerializeField] private Sprite m_NeutralFace;
    [SerializeField] private Sprite m_SadFace;
    [SerializeField] private Sprite m_HappyFace;

    [Header("Subtitle letter apparition")]
    [SerializeField] private float m_MinLetterTime = 0.05f;
    [SerializeField] private float m_MaxLetterTime = 0.1f;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    
    Sprite GetExpressionSprite(SO_DialogPart.Expression expression)
    {
        return expression switch
        {
            SO_DialogPart.Expression.Neutral => m_NeutralFace,
            SO_DialogPart.Expression.Sad => m_SadFace,
            SO_DialogPart.Expression.Happy => m_HappyFace,
            _ => m_NeutralFace
        };
    }
    
    IEnumerator DisplaySubtitleRoutine(string subtitle)
    {
        foreach (char letter in subtitle)
        {
            float randPause = Random.Range(m_MinLetterTime, m_MaxLetterTime);
            yield return new WaitForSeconds(randPause);
            m_NarratorSubtitle.text += letter;
        }
    }

    public void DisplayNextSubtitle(SO_DialogPart[] dialogs)
    {
        SO_DialogPart dialogPart = dialogs[m_DialogIndex];
        
        m_NarratorSubtitle.text = "";
        m_NarratorExpression.sprite = GetExpressionSprite(dialogPart.expression);
        
        StopCoroutine(nameof(DisplaySubtitleRoutine));
        StartCoroutine(DisplaySubtitleRoutine(dialogPart.subtitle));
        m_DialogIndex++;
    }

    public void EndSequence()
    {
        m_DialogIndex = 0;
    }
}
