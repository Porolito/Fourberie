using System.Collections.Generic;
using UnityEngine;

namespace Timeline
{
    [CreateAssetMenu(fileName = "EV_KPEndEvent", menuName = "Scriptable Objects/Events/EV_KPEndEvent")]
    public class EV_KPEndEvent : ScriptableObject
    {
        private List<IEV_KPEndEvent> listener = new List<IEV_KPEndEvent>();

        public void Register(IEV_KPEndEvent ev) => listener.Add(ev);

        public void Unregister(IEV_KPEndEvent ev) => listener.Remove(ev);

        public void CallFinishedKeypoint() => listener.ForEach(l => l.OnKeypointFinished());
    }
}