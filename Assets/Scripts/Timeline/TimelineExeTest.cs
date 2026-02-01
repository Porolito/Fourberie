using Timeline;
using UnityEngine;
using UnityEngine.InputSystem;

public class TimelineExeTest : MonoBehaviour
{
    [SerializeField] private EV_PhaseSuccessEvent evPhaseSuccessEvent;

    private int id = 0;
    
    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            evPhaseSuccessEvent.CallPhaseSuccess(id);
            id++;

        }
    }
}
