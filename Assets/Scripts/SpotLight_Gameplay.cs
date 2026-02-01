using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SpotLight_Gameplay : MonoBehaviour
{
    public GameObject[] spotLight;
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    private void spotLightGame()
    {
        int rand =  Random.Range(0, spotLight.Length);
        GameObject actualSpotLight = spotLight[rand];
        actualSpotLight.SetActive(true);
    }

    private void CheckTrigger()
    {
        Debug.Log(GetComponent<SpotLight_Script>().isOnSpotLight ? "SpotLight Success" : "SpotLight Failed");
    }
    
}
