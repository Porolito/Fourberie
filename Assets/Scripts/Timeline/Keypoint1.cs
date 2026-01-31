using UnityEngine;

namespace Timeline
{
    public class Keypoint1 : MonoBehaviour, IKeypoints
    {
        public void ProcessKeypoint()
        {
            Debug.Log("StartKeypoint");
        }

        public void EndKeypoint()
        {
            Debug.Log("EndKeypoint");
        }
    }
}
