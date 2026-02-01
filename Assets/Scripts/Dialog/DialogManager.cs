using System;
using System.Collections;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DialogManager : MonoBehaviour
{
    public static DialogManager instance;
    
    private AudioSource m_AudioSource;

    [Header("Test")]
    [SerializeField] private bool m_TestAtAwake;
    [SerializeField] private DialogPartData m_DialogTest;
    
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
        
        m_AudioSource = GetComponent<AudioSource>();
        
        if (m_TestAtAwake)
            PlayDialog(m_DialogTest);
    }

    public void PlayDialog(DialogPartData dialog)
    {
        m_AudioSource.Stop();
        m_NarratorSubtitle.text = "";
        
        m_NarratorExpression.sprite = GetExpressionSprite(dialog.expression);
        m_AudioSource.clip = dialog.clip;
        m_AudioSource.Play();
        StartCoroutine(DisplaySubtitleRoutine(dialog.subtitle));
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
    IEnumerator DisplaySubtitleRoutine(string subtitle)
    {
        foreach (char letter in subtitle)
        {
            float randPause = Random.Range(m_MinLetterTime, m_MaxLetterTime);
            yield return new WaitForSeconds(randPause);
            m_NarratorSubtitle.text += letter;
        }
    }
}
