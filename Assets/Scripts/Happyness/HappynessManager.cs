using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Timeline;
using UnityEngine;

public class HappynessManager : MonoBehaviour, IEV_MalusEvent
{
    private int malusCount;

    private List<SpriteRenderer> _publicMasks;
    
    [SerializeField] PublicState badState;
    [SerializeField] PublicState neutralState;
    void Start()
    {
        var objects = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "Name");
        foreach (var go in objects)
        {
            _publicMasks.Add(go.GetComponent<SpriteRenderer>());
        }
    }
    public void OnMalusReceived()
    {
        malusCount++;
        StartCoroutine(Timer());
    }

    private void ChangePublicMask(PublicState state)
    {
        foreach (var go in _publicMasks)
        {
            go.sprite = state.sprite;
        }
        Debug.Log("Malus received");
    }

    IEnumerator Timer()
    {
        Debug.Log("Timer");
        ChangePublicMask(badState);
        yield return new WaitForSeconds(3f);
        ChangePublicMask(neutralState);
    }
}
