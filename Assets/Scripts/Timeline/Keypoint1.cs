using System;
using System.Collections;
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
        
        [SerializeField] private BulletManager bulletManager;
        
        [SerializeField] private SO_DialogPart[] dialogParts;

        private Coroutine keypointCoroutine;
        private Coroutine currentPattern;

        private void Awake()
        {
            evPhaseSuccessEvent.Register(this);
        }

        public override IEnumerator ProcessKeypoint()
        {
            Debug.Log("Start Phase1");
            yield return new WaitForSeconds(0.5f);
            bulletManager.LaunchCoroutine(3f, 5, BulletManager.Pattern.Straight,3f, 2, 50);
            //Boss.Instance.StartPhase(true);
            //KP logic : bullets
        }

        public void OnPhaseSuccess(int id)
        {
            if (kpID != id) return;
            bulletManager.CancelCoroutine();
            StopCoroutine(ProcessKeypoint());
            Debug.Log("EndKeypoint");
            //KP logic : dialog
            evKPEndEvent.CallFinishedKeypoint();
        }
    }
}
