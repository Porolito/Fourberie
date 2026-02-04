using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Fourberies/Dialog Part", fileName = "DP_NewDialogPart")]
[System.Serializable]
public class SO_DialogPart : ScriptableObject
{
    public AudioClip clip;
    [TextArea] public string subtitle;//TODO: transformer en smart strings pour la localization
    public Expression expression;
    
    public enum Expression
    {
        Neutral,
        Sad,
        Happy,
    }
}
