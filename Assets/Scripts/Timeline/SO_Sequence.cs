using System;
using UnityEngine;
using UnityEngine.Timeline;

[CreateAssetMenu(menuName = "Fourberies/Sequence", fileName = "SEQ_NewSequence")]
public class SO_Sequence : ScriptableObject
{
    public TimelineAsset timeline;
    public SO_DialogPart[] dialogs;
    public ChallengeType challenge;
    public BulletInfo[] bulletPatterns;
    
    public enum ChallengeType
    {
        None,
        BossAttack,
        PlayerHit
    }
    
    [Serializable]
    public struct BulletInfo
    {
        public float spawnFrequency;
        public int spawnQuantity;
        public BulletManager.Pattern pattern;
        public float timeBeforeNewWave;
    }
}
