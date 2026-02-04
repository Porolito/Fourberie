using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Timeline
{
    public class TimelineHandler : MonoBehaviour
    {
        public static TimelineHandler instance;
        
        private PlayableDirector m_TimelineDirector;
        
        private int m_CurrentSequenceIndex;
        
        [Header("References")]
        [SerializeField] private DialogManager m_DialogManager;
        [SerializeField] private Menus m_Menus;
        [SerializeField] private Boss m_Boss;
        [SerializeField] private SO_GameEvent m_ChallengeSuccessGE;
        
        [Space]
        [SerializeField] [Tooltip("Order is important!")] private SO_Sequence[] m_Sequences;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            
            m_CurrentSequenceIndex = 0;

            m_TimelineDirector = GetComponent<PlayableDirector>();
            m_ChallengeSuccessGE.Bind(OnChallengeSuccess);
        }

        public void StartSequence()
        {
            Debug.Log($"Start sequence {m_Sequences[m_CurrentSequenceIndex].name}");
            m_TimelineDirector.playableAsset = m_Sequences[m_CurrentSequenceIndex].timeline;
            m_TimelineDirector.Play();
        }

        private void EndSequence()
        {
            print($"End sequence {m_Sequences[m_CurrentSequenceIndex].name}");
            m_DialogManager.EndSequence();
            m_CurrentSequenceIndex++;

            if (m_CurrentSequenceIndex >= m_Sequences.Length)
                DisplayEndScreen();
            else
                StartSequence();
        }

        private void DisplayEndScreen()
        {
            //TODO: faire la fin
            Debug.Log($"End game gg");
        }

        private void StartChallenge(SO_Sequence.ChallengeType challenge)
        {
            switch (challenge)
            {
                case SO_Sequence.ChallengeType.BossAttack:
                    m_Boss.StartPhase();
                    break;
                case SO_Sequence.ChallengeType.PlayerHit:
                    //TODO Faire le player hit challenge
                    print("Player hit challenge");
                    break;
                default:
                    Debug.LogWarning($"[{m_Sequences[m_CurrentSequenceIndex].name}] Unknown challenge");
                    break;
            }
        }

        private void OnChallengeSuccess(object args)
        {
            EndSequence();
        }

        
        #region Signals Callbacks // Called by signals in timelines

        public void SC_OnEndSequence()
        {
            EndSequence();
        }
        
        public void SC_OnDisplayNextSubtitle()
        {
            m_DialogManager.DisplayNextSubtitle(m_Sequences[m_CurrentSequenceIndex].dialogs);
        }

        public void SC_OnOpenEntranceCurtains()
        {
            m_Menus.OpenCurtains();
        }

        public void SC_OnEnableChallenge()
        {
            StartChallenge(m_Sequences[m_CurrentSequenceIndex].challenge);
        }

        public void SC_OnThrowBullets()
        {
            print("Throw bullets");
        }

        #endregion
    }
}
