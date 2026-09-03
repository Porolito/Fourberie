using System;
using UnityEngine;
using UnityEngine.Timeline;

[CreateAssetMenu(menuName = "Fourberies/Sequence", fileName = "SEQ_NewSequence")]
public class SO_Sequence : ScriptableObject
{
    public TimelineAsset timeline;
    public SO_DialogPart[] dialogs;
    public ChallengeType challenge;
    public BulletPattern[] bulletPatterns;
    
    public enum ChallengeType
    {
        None,
        BossAttack,
        PlayerHit
    }

    [Serializable]
    public struct BulletPattern
    {
        public SO_BulletInfo[] bulletInfos;
    }
}
