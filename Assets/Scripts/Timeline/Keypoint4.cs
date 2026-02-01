using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Timeline
{
    public class Keypoint4 : AbstractKeypoints, IEV_PhaseSuccessEvent
    {
        //Pattern SO
        private int kpID = 3;

        [SerializeField] private EV_PhaseSuccessEvent evPhaseSuccessEvent;
        [SerializeField] private EV_KPEndEvent evKPEndEvent;
        
        [SerializeField] private BulletManager bulletManager;
        [SerializeField] private DialogPartData[] dialogParts;

        private Coroutine currentPattern;

        private void Awake()
        {
            evPhaseSuccessEvent.Register(this);
        }

        public override IEnumerator ProcessKeypoint()
        {
            Debug.Log("Start Phase4");
            Boss.Instance.StartPhase(false);
            DialogManager.instance.PlayDialog(dialogParts[0]);
            yield return new WaitForSeconds(dialogParts[0].clip.length);
            DialogManager.instance.PlayDialog(dialogParts[1]);
            yield return new WaitForSeconds(dialogParts[1].clip.length + 0.5f);
            SpotLight_Gameplay.Instance.StartPhase(kpID);
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
