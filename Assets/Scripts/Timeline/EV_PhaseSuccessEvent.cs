using System.Collections.Generic;
using UnityEngine;

namespace Timeline
{
    [CreateAssetMenu(fileName = "EV_PhaseSuccessEvent", menuName = "Scriptable Objects/Events/EV_PhaseSuccessEvent")]
    public class EV_PhaseSuccessEvent : ScriptableObject
    {
        private List<IEV_PhaseSuccessEvent> listener = new List<IEV_PhaseSuccessEvent>();

        public void Register(IEV_PhaseSuccessEvent ev) => listener.Add(ev);

        public void Unregister(IEV_PhaseSuccessEvent ev) => listener.Remove(ev);

        public void CallPhaseSuccess(int id) => listener.ForEach(l => l.OnPhaseSuccess(id));
    }
}