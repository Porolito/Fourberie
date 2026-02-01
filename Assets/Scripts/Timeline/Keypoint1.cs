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
        
        [SerializeField] private DialogPartData[] dialogParts;

        private Coroutine keypointCoroutine;
        private Coroutine currentPattern;

        private void Awake()
        {
            evPhaseSuccessEvent.Register(this);
        }

        public override IEnumerator ProcessKeypoint()
        {
            Debug.Log("Start Phase1");
            DialogManager.instance.PlayDialog(dialogParts[0]);
            yield return new WaitForSeconds(3f);
            DialogManager.instance.PlayDialog(dialogParts[1]);
            yield return new WaitForSeconds(dialogParts[1].clip.length+1f);
            DialogManager.instance.PlayDialog(dialogParts[2]);
            yield return new WaitForSeconds(0.5f);
            bulletManager.LaunchCoroutine(3f, 5, "Straight",3f);
            yield return new WaitForSeconds(10f);
            DialogManager.instance.PlayDialog(dialogParts[3]);
            //KP logic : bullets
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
