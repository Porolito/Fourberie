using System;
using UnityEngine;

public class SpotLight_Script : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public bool isOnSpotLight = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("aaa");
        isOnSpotLight = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("bbb");
        isOnSpotLight = false;
    }

    private void TurnOffSpotLight()
    {
        gameObject.SetActive(false);
        isOnSpotLight = false;
    }
    private void CheckTrigger()
    {
        if (isOnSpotLight)
        {
            SpotLight_Gameplay.Instance.CallSuccess();
        }
        else
        {
            SpotLight_Gameplay.Instance.CallFail();
        }
    }
}
