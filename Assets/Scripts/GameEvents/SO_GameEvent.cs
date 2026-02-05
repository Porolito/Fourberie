using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Fourberies/Game Event", fileName = "GE_NewGameEvent")]
public class SO_GameEvent : ScriptableObject
{
    private UnityEvent<object> m_GameEvent = new UnityEvent<object>();
    [SerializeField] private bool m_ClearAfterTriggered = false;

    public void Trigger(object args = null)
    {
        m_GameEvent.Invoke(args);
        if(m_ClearAfterTriggered) m_GameEvent.RemoveAllListeners();
    }

    public void Bind(UnityAction<object> action) => m_GameEvent.AddListener(action);

    public void Unbind(UnityAction<object> action) => m_GameEvent.RemoveListener(action);
}