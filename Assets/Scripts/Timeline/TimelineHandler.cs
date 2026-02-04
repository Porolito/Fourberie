using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
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
        [SerializeField] private BulletManager m_BulletManager;
        [SerializeField] private Player m_Player;
        
        [Header("Game Events")]
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

#if UNITY_EDITOR
        private void Update()
        {
            if (Keyboard.current.qKey.wasPressedThisFrame)
            {
                m_CurrentSequenceIndex -= 2;
                if (m_CurrentSequenceIndex <= -1) m_CurrentSequenceIndex = -1;
                EndSequence(false);
            }

            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                EndSequence(false);
            }
        }
#endif
        
        public void StartSequence()
        {
            Debug.Log($"[{m_Sequences[m_CurrentSequenceIndex].name}] Starting...");
            m_TimelineDirector.playableAsset = m_Sequences[m_CurrentSequenceIndex].timeline;
            m_TimelineDirector.Play();
        }

        private void EndSequence(bool log = true)
        {
            if (log) Debug.Log($"[{m_Sequences[m_CurrentSequenceIndex].name}] End!");
            m_BulletManager.CancelCoroutine();
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
                    m_Player.m_IsPlayerHitChallenge = true;
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
            if (m_CurrentSequenceIndex >= m_Sequences.Length)
            {
                Debug.LogError($"[{m_Sequences[m_CurrentSequenceIndex].name}] No subtitle patterns found");
                return;
            }
            
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
            SO_Sequence.BulletInfo[] bulletPatterns = m_Sequences[m_CurrentSequenceIndex].bulletPatterns;

            if (bulletPatterns.Length == 0)
            {
                Debug.LogWarning($"[{m_Sequences[m_CurrentSequenceIndex].name}] No bullet patterns found");
                return;
            }
            
            //TODO: Prendre en compte plusieurs patterns ?
            m_BulletManager.LaunchCoroutine(
                bulletPatterns[0].spawnFrequency, 
                bulletPatterns[0].spawnQuantity, 
                bulletPatterns[0].pattern, 
                bulletPatterns[0].timeBeforeNewWave);
        }

        #endregion
    }
}
