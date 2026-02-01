using Timeline;
using UnityEngine;

public class HappynessManager : MonoBehaviour, IEV_MalusEvent
{
    private int malusCount;
    
    public void OnMalusReceived()
    {
        malusCount++;
    }
}
