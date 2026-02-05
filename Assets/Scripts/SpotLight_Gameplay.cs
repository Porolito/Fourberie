using System;
using Timeline;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

public class SpotLight_Gameplay : MonoBehaviour
{
    private int m_CurrentPhaseID;

    [Header("Refs")]
    [SerializeField] private GameObject[] spotLight;
    
    [Header("Events")]
    [SerializeField] private EV_MalusEvent m_MalusEvent;
    [SerializeField] private EV_PhaseSuccessEvent m_PhaseSuccessEvent;

    public void TurnOnRandomSpotlight()
    {
        int rand =  Random.Range(0, spotLight.Length);
        GameObject actualSpotLight = spotLight[rand];
        actualSpotLight.GetComponent<SpotLight_Script>().TurnOn(this);
    }

    public void CallSuccess()
    {
        //m_PhaseSuccessEvent.CallPhaseSuccess(m_CurrentPhaseID);
        Debug.Log("Spotlight success");
    }

    public void CallFail()
    {
        Debug.Log("Spotlight fail");
        m_MalusEvent.CallMalus();
    }
}
