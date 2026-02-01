using UnityEngine;

[CreateAssetMenu(menuName = "Fourberies/Dialog/Dialog Part Data", fileName = "New Dialog Part")]
public class DialogPartData : ScriptableObject
{
    public AudioClip clip;
    [TextArea] public string subtitle;
    public Expression expression;
    
    public enum Expression
    {
        Neutral,
        Sad,
        Happy,
    }
}
