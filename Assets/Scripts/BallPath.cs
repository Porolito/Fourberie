using UnityEngine;
using DG.Tweening;

public class BallPath : MonoBehaviour
{
    private Vector3 _startPos;
    
    public float endPosX; //X pos to reach
    public float waveAmplitude;
    public float waveFrequency;
    public Vector3 spawnPosition = Vector3.zero;
    
    float moveDuration = 15;

    public float startY = 1;
    // Update is called once per frame
    
    
    void Start()
    {
        _startPos = transform.position;
        
    }
    private void TurnOffBall()
    {
        gameObject.SetActive(false);
    }

    public void GiveAPath()
    {
        transform.DOMoveX(endPosX, moveDuration).SetEase(Ease.Linear).OnComplete(TurnOffBall);

        // Move along y-axis following the sine wave
        DOVirtual.Float(0, moveDuration, moveDuration, (t) =>
        {
            // Calculate the new y position using the sine function and apply the shift to our og y
            float newY = waveAmplitude * (Mathf.Sin(t * waveFrequency * 2) * startY);
            transform.position = new Vector3(transform.position.x, _startPos.y + newY, _startPos.z);
        }).SetEase(Ease.Linear);
    }
}
