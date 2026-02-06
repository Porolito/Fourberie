using System.Collections.Generic;
using UnityEngine;


    [CreateAssetMenu(fileName = "EV_MalusEvent", menuName = "Scriptable Objects/Events/EV_MalusEvent")]
    public class EV_MalusEvent : ScriptableObject
    {
        private List<IEV_MalusEvent> listener = new List<IEV_MalusEvent>();

        public void Register(IEV_MalusEvent ev) => listener.Add(ev);

        public void Unregister() => listener.Clear();

        public void CallMalus() => listener.ForEach(l => l.OnMalusReceived());
    }
