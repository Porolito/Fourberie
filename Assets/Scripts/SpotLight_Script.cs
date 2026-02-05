using System;
using UnityEngine;

public class SpotLight_Script : MonoBehaviour
{
    private SpotLight_Gameplay m_Parent;
    private bool isOnSpotLight = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isOnSpotLight = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        isOnSpotLight = false;
    }

    public void TurnOn(SpotLight_Gameplay parent)
    {
        m_Parent = parent;
        gameObject.SetActive(true);
    }

    // Called by animation (?)
    private void TurnOffSpotLight()
    {
        gameObject.SetActive(false);
        isOnSpotLight = false;
    }
    
    // Called by animation (?)
    private void CheckTrigger()
    {
        if (isOnSpotLight)
        {
            m_Parent.CallSuccess();
        }
        else
        {
            m_Parent.CallFail();
        }
    }
}
