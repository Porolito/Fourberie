using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    private List<GameObject> _ballsPool =  new List<GameObject>();
    [SerializeField] private GameObject[] ballType;
    
    private void SummonBall(int index)
    {
        _ballsPool.Add(Instantiate(ballType[index], transform.position, Quaternion.identity));
    }
}
