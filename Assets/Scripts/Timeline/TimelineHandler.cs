using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;
using UnityEngine.UI;

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
        [SerializeField] private SpotLight_Gameplay m_SpotLights;
        [SerializeField] private HappynessManager m_HappinessManager;
        [SerializeField] private Image m_EndingImage;
        
        [Header("Game Events")]
        [SerializeField] private SO_GameEvent m_ChallengeSuccessGE;

        [Header("Endings")]
        [SerializeField] private int m_NeutralEndingSequenceMinimumHappiness;
        [SerializeField] private int m_NeutralEndingSequenceMaximumHappiness;
        [SerializeField] private SO_Sequence m_BadEndingSequence;
        [SerializeField] private SO_Sequence m_NeutralEndingSequence;
        [SerializeField] private SO_Sequence m_GoodEndingSequence;
        [SerializeField] private Sprite m_BadEndingSprite;
        [SerializeField] private Sprite m_NeutralEndingSprite;
        [SerializeField] private Sprite m_GoodEndingSprite;
        
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

        private void Update()
        {
            if (Keyboard.current.rKey.wasPressedThisFrame)
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            
#if UNITY_EDITOR
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
#endif
        }
        
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
            m_TimelineDirector.Stop();

            if (m_CurrentSequenceIndex >= m_Sequences.Length)
                DisplayEndScreen();
            else
                StartSequence();
        }

        private void DisplayEndScreen()
        {
            Debug.Log($"End game gg");
            if (m_HappinessManager.malusCount < m_NeutralEndingSequenceMinimumHappiness)
            {
                m_EndingImage.sprite = m_GoodEndingSprite;
                m_TimelineDirector.playableAsset = m_GoodEndingSequence.timeline;
            }
            else if (m_HappinessManager.malusCount > m_NeutralEndingSequenceMaximumHappiness)
            {
                m_EndingImage.sprite = m_BadEndingSprite;
                m_TimelineDirector.playableAsset = m_BadEndingSequence.timeline;
            }
            else
            {
                m_EndingImage.sprite = m_NeutralEndingSprite;
                m_TimelineDirector.playableAsset = m_NeutralEndingSequence.timeline;
            }
            
            m_EndingImage.gameObject.SetActive(true);
            m_TimelineDirector.Play();
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

        private void OnChallengeSuccess(object args) => EndSequence();

        
        #region Signals Callbacks // Called by signals in timelines

        public void SC_OnEndSequence() => EndSequence();
        
        public void SC_OnDisplayNextSubtitle()
        {
            if (m_CurrentSequenceIndex >= m_Sequences.Length)
            {
                //Debug.LogError($"[{m_Sequences[m_CurrentSequenceIndex].name}] No subtitle found");
                return;
            }
            
            m_DialogManager.DisplayNextSubtitle(m_Sequences[m_CurrentSequenceIndex].dialogs);
        }

        public void SC_OnOpenEntranceCurtains() => m_Menus.OpenCurtains();

        public void SC_OnEnableChallenge() => StartChallenge(m_Sequences[m_CurrentSequenceIndex].challenge);

        public void SC_OnThrowBullets()
        {
            BulletManager.BulletInfo[] bulletPatterns = m_Sequences[m_CurrentSequenceIndex].bulletPatterns;

            if (bulletPatterns.Length == 0)
            {
                Debug.LogWarning($"[{m_Sequences[m_CurrentSequenceIndex].name}] No bullet patterns found");
                return;
            }
            
            //TODO: Prendre en compte plusieurs patterns ?
            m_BulletManager.LaunchCoroutine(bulletPatterns[0]);
        }

        public void SC_OnActivateSpotlight() => m_SpotLights.TurnOnRandomSpotlight();

        #endregion
    }
}
