using UnityEngine;
using UnityEngine.InputSystem;

namespace Timeline
{
    public class TimelineExeTest : MonoBehaviour
    {
        [SerializeField] private EV_PhaseSuccessEvent evPhaseSuccessEvent;

        private int id = 0;
    
        void Update()
        {
            if (Keyboard.current.pKey.wasPressedThisFrame)
            {
                evPhaseSuccessEvent.CallPhaseSuccess(id);
                id++;

            }
        }
    }
}