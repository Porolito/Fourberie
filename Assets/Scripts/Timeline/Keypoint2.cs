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
        
        [SerializeField] private DialogPartData[] dialogParts;

        private Coroutine currentPattern;

        private bool isHitChallengeActive = false;

        private void Awake()
        {
            evPhaseSuccessEvent.Register(this);
        }

        public override IEnumerator ProcessKeypoint()
        {
            Debug.Log("Start Phase2");
            DialogManager.instance.PlayDialog(dialogParts[0]);
            yield return new WaitForSeconds(dialogParts[0].clip.length + 1f);
            DialogManager.instance.PlayDialog(dialogParts[1]);
            yield return new WaitForSeconds(dialogParts[1].clip.length - 0.5f);
            bulletManager.LaunchCoroutine(0.5f, 1, "Straight", 15f);
            isHitChallengeActive = true;
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
