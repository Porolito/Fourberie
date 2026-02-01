using System.Collections;
using UnityEngine;

namespace Timeline
{
    public abstract class AbstractKeypoints : MonoBehaviour
    {
        public abstract IEnumerator ProcessKeypoint();
    }
}