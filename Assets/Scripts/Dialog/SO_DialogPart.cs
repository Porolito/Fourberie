using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Fourberies/Dialog/Dialog Part Data", fileName = "New Dialog Part")]
[System.Serializable]
public class SO_DialogPart : ScriptableObject
{
    public AudioClip clip;
    [TextArea] public string subtitle;//TODO: transformer en sart strings pour la localization
    public Expression expression;
    
    public enum Expression
    {
        Neutral,
        Sad,
        Happy,
    }
}
