using UnityEngine;

[CreateAssetMenu(menuName = "Fourberies/BulletInfo", fileName = "BI_NewBulletInfo")]
public class SO_BulletInfo : ScriptableObject
{
    public float spawnFrequency;
    public int spawnQuantity;
    public float waveCooldown;
    [Tooltip("Leave at 0 for strait ball")] public float sineFrequency;
    public float sineAmplitude;
    public float travelTime;
    public SpawnHeight spawnHeight;
    public bool spawnAtLeft;
        
    public enum SpawnHeight
    {
        Low,
        Medium,
        High
    }
}
