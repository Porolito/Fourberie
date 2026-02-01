using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Timeline
{
    public class Keypoint5 : AbstractKeypoints, IEV_PhaseSuccessEvent
    {
        //Pattern SO
        private int kpID = 4;

        [SerializeField] private EV_PhaseSuccessEvent evPhaseSuccessEvent;
        [SerializeField] private EV_KPEndEvent evKPEndEvent;
        
        [SerializeField] private BulletManager bulletManager;

        private Coroutine currentPattern;

        private void Awake()
        {
            evPhaseSuccessEvent.Register(this);
        }

        public override void ProcessKeypoint()
        {
            Debug.Log("StartKeypoint");
            currentPattern = StartCoroutine(bulletManager.SpawnerPattern(0.5f, 30, "Straight"));
            //KP logic : bullets
        }

        public void OnPhaseSuccess(int id)
        {
            if (kpID != id) return;
            StopCoroutine(currentPattern);
            Debug.Log("EndKeypoint");
            //KP logic : dialog
            evKPEndEvent.CallFinishedKeypoint();
        }
    }
}
