using UnityEngine;
using System.Collections.Generic;

namespace Timeline
{
    public class TimelineHandler : MonoBehaviour
    {
        [SerializeField] private List<IKeypoints> keypoints = new List<IKeypoints>();
    }
}
