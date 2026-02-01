using System;
using UnityEngine;
using System.Collections.Generic;

namespace Timeline
{
    public class TimelineHandler : MonoBehaviour, IEV_KPEndEvent
    {
        public static TimelineHandler instance;
        
        [SerializeField] private List<AbstractKeypoints> keypoints;
        
        [SerializeField] private EV_KPEndEvent evKPEndEvent;
        private int currentKeypointIndex = 0;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            
            evKPEndEvent.Register(this);
        }

        //Start first keypoint in list
        public void StartTimeline()
        {
            StartCoroutine(keypoints[0].ProcessKeypoint());
        }

        public void OnKeypointFinished()
        {
            Debug.Log("Switch Keypoint");
            currentKeypointIndex++;
            StartCoroutine(keypoints[currentKeypointIndex].ProcessKeypoint());
        }
    }
}
