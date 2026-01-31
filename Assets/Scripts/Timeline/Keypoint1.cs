using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Timeline
{
    public class Keypoint1 : AbstractKeypoints, IEV_PhaseSuccessEvent
    {
        //Pattern SO
        private int kpID = 0;

        [SerializeField] private EV_PhaseSuccessEvent evPhaseSuccessEvent;
        [SerializeField] private EV_KPEndEvent evKPEndEvent;

        private void Awake()
        {
            evPhaseSuccessEvent.Register(this);
        }

        public override void ProcessKeypoint()
        {
            Debug.Log("StartKeypoint");
            //KP logic : bullets
        }

        public void OnPhaseSuccess(int id)
        {
            if (kpID != id) return;
            Debug.Log("EndKeypoint");
            //KP logic : dialog
            evKPEndEvent.CallFinishedKeypoint();
        }
    }
}
