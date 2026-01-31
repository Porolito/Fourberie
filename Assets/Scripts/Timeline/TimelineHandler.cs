using UnityEngine;
using System.Collections.Generic;

namespace Timeline
{
    public class TimelineHandler : MonoBehaviour, IEV_KPEndEvent
    {
        [SerializeField] private List<AbstractKeypoints> keypoints;
        private int currentKeypointIndex = 0;

        //Start first keypoint in list
        private void Start()
        {
            keypoints[0].ProcessKeypoint();
        }

        public void OnKeypointFinished()
        {
            currentKeypointIndex++;
            keypoints[currentKeypointIndex].ProcessKeypoint();
        }
    }
}
