using System;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(menuName = "Fourberies/Dialog Part", fileName = "DP_NewDialogPart")]
[System.Serializable]
public class SO_DialogPart : ScriptableObject
{
    public AudioClip clip;
    [SerializeField] private LocalizedString subtitle;
    public Expression expression;

    public string GetLocalizedSubtitle()
    {
        return subtitle.GetLocalizedString();
    }
    
    public enum Expression
    {
        Neutral,
        Sad,
        Happy,
    }
}
