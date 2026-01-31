using UnityEngine;
using DG.Tweening;

public class BallPath : MonoBehaviour
{
    private Vector3 _startPos;
    
    [SerializeField] private float _endPosX = 30; //X pos to reach
    [SerializeField] private float _speed; //Speed of sine wave
    [SerializeField] private float magnitude; //Magnitude of sine wave
    // Update is called once per frame
    void Update()
    {
        transform.DOMove(new Vector3(_endPosX,Mathf.Sin(Time.time * _speed) * magnitude, 0), 10);
    }
}
