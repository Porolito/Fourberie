using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Timeline
{
    public class TimelineHandler : MonoBehaviour, IEV_KPEndEvent
    {
        public static TimelineHandler instance;
        
        private PlayableDirector m_TimelineDirector;
        
        //[SerializeField] private List<AbstractKeypoints> keypoints;
        [SerializeField] private EV_KPEndEvent evKPEndEvent;
        
        [Header("References")]
        [SerializeField] private DialogManager m_DialogManager;
        [SerializeField] private Menus m_Menus;
        
        [Space]
        [SerializeField] [Tooltip("Order is important!")] private TimelineAsset[] m_Timelines;
        
        public int currentSequenceIndex { get; private set; }

        private void Awake()
        {
            if (instance == null)
                instance = this;
            
            evKPEndEvent.Register(this);
            currentSequenceIndex = 0;

            m_TimelineDirector = GetComponent<PlayableDirector>();
        }

        public void StartSequence()
        {
            Debug.Log($"Start sequence {m_Timelines[currentSequenceIndex].name}");
            m_TimelineDirector.playableAsset = m_Timelines[currentSequenceIndex];
            m_TimelineDirector.Play();
        }

        public void OnKeypointFinished()
        {
            EndSequence();
        }

        private void EndSequence()
        {
            print($"End sequence {currentSequenceIndex}");
            m_DialogManager.EndSequence();
            currentSequenceIndex++;

            if (currentSequenceIndex >= m_Timelines.Length)
                DisplayEndScreen();
            else
                StartSequence();
        }

        private void DisplayEndScreen()
        {
            //TODO: faire la fin
            Debug.Log($"End game gg");
        }

        
        #region Signals Callbacks // Called by signals in timelines

        public void SC_OnEndSequence()
        {
            EndSequence();
        }
        
        public void SC_OnDisplayNextSubtitle()
        {
            m_DialogManager.DisplayNextSubtitle(currentSequenceIndex);
        }

        public void SC_OnOpenEntranceCurtains()
        {
            m_Menus.OpenCurtains();
        }

        #endregion
    }
}
