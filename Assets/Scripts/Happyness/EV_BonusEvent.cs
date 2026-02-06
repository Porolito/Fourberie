using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EV_BonusEvent", menuName = "Scriptable Objects/Events/EV_BonusEvent")]
public class EV_BonusEvent : ScriptableObject
{
    private List<IEV_BonusEvent> listener = new List<IEV_BonusEvent>();

    public void Register(IEV_BonusEvent ev) => listener.Add(ev);

    public void Unregister() => listener.Clear();

    public void CallBonus() => listener.ForEach(l => l.OnBonusReceived());
}
