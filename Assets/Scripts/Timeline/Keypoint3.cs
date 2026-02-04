using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Timeline
{
    public class Keypoint3 : AbstractKeypoints, IEV_PhaseSuccessEvent
    {
        //Pattern SO
        private int kpID = 2;

        [SerializeField] private EV_PhaseSuccessEvent evPhaseSuccessEvent;
        [SerializeField] private EV_KPEndEvent evKPEndEvent;
        
        [SerializeField] private BulletManager bulletManager;
        
        [SerializeField] private SO_DialogPart[] dialogParts;

        private Coroutine currentPattern;

        private void Awake()
        {
            evPhaseSuccessEvent.Register(this);
        }

        public override IEnumerator ProcessKeypoint()
        {
            Debug.Log("Start Phase3");
            //Boss.Instance.StartPhase(true);
            bulletManager.LaunchCoroutine(3f, 10, BulletManager.Pattern.Sine, 3);
            //KP logic : bullets
            yield return null;
        }

        public void OnPhaseSuccess(int id)
        {
            if (kpID != id) return;
            print("CALLED");
            bulletManager.CancelCoroutine();
            StopCoroutine(ProcessKeypoint());
            Debug.Log("EndKeypoint");
            //KP logic : dialog
            evKPEndEvent.CallFinishedKeypoint();
        }
    }
}
