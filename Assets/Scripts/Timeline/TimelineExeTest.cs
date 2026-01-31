using Timeline;
using UnityEngine;
using UnityEngine.InputSystem;

public class TimelineExeTest : MonoBehaviour
{
    [SerializeField] private EV_PhaseSuccessEvent evPhaseSuccessEvent;
    
    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            evPhaseSuccessEvent.CallPhaseSuccess(0);
        }
    }
}
