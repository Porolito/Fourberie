using UnityEngine;

[CreateAssetMenu(fileName = "BallPathScriptable", menuName = "Scriptable Objects/BallPathScriptable")]
public class BallPathScriptable : ScriptableObject
{
    public float endPosX = 30; //X pos to reach
    public float waveAmplitude = 0f;
    public float waveFrequency = 0f;
    public Vector3 spawnPosition = Vector3.zero;
}
