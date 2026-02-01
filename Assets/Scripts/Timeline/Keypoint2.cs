using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Timeline
{
    public class Keypoint2 : AbstractKeypoints, IEV_PhaseSuccessEvent
    {
        //Pattern SO
        private int kpID = 1;

        [SerializeField] private EV_PhaseSuccessEvent evPhaseSuccessEvent;
        [SerializeField] private EV_KPEndEvent evKPEndEvent;
        
        [SerializeField] private BulletManager bulletManager;

        private Coroutine currentPattern;

        private void Awake()
        {
            evPhaseSuccessEvent.Register(this);
        }

        public override IEnumerator ProcessKeypoint()
        {
            Debug.Log("StartKeypoint");
            bulletManager.LaunchCoroutine(0.5f, 1, "Straight", 3);
            //KP logic : bullets
            yield return null;

        }

        public void OnPhaseSuccess(int id)
        {
            if (kpID != id) return;
            bulletManager.CancelCoroutine();
            Debug.Log("EndKeypoint");
            //KP logic : dialog
            evKPEndEvent.CallFinishedKeypoint();
        }
    }
}
