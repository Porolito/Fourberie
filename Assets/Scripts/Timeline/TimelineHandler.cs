using System;
using UnityEngine;
using System.Collections.Generic;

namespace Timeline
{
    public class TimelineHandler : MonoBehaviour, IEV_KPEndEvent
    {
        [SerializeField] private List<AbstractKeypoints> keypoints;
        
        [SerializeField] private EV_KPEndEvent evKPEndEvent;
        private int currentKeypointIndex = 0;

        private void Awake()
        {
            evKPEndEvent.Register(this);
        }

        //Start first keypoint in list
        private void Start()
        {
            keypoints[0].ProcessKeypoint();
        }

        public void OnKeypointFinished()
        {
            Debug.Log("Switch Keypoint");
            currentKeypointIndex++;
            keypoints[currentKeypointIndex].ProcessKeypoint();
        }
    }
}
