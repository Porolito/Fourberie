using System;
using Timeline;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

public class SpotLight_Gameplay : MonoBehaviour
{
    public static SpotLight_Gameplay Instance;

    private int m_CurrentPhaseID;

    [Header("Refs")]
    [SerializeField] private EV_MalusEvent m_MalusEvent;
    [SerializeField] private EV_PhaseSuccessEvent m_PhaseSuccessEvent;
    public GameObject[] spotLight;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void StartPhase(int phaseID)
    {
        m_CurrentPhaseID = phaseID;
        spotLightGame();
    }

    private void spotLightGame()
    {
        int rand =  Random.Range(0, spotLight.Length);
        GameObject actualSpotLight = spotLight[rand];
        actualSpotLight.SetActive(true);
    }

    public void CallSuccess()
    {
        m_PhaseSuccessEvent.CallPhaseSuccess(m_CurrentPhaseID);
    }

    public void CallFail()
    {
        m_MalusEvent.CallMalus();
        spotLightGame();
    }
}
