using UnityEngine;
using DG.Tweening;

public class BallPath : MonoBehaviour
{
    public void GiveAPath(Vector2 startPos, float travelDistanceX, BulletManager.BulletInfo bulletInfo)
    {
        
        transform.position = startPos;
        
        transform.DOMoveX(
            bulletInfo.spawnAtLeft ? travelDistanceX : -travelDistanceX, 
            bulletInfo.travelTime)
            .SetEase(Ease.Linear).OnComplete(() => gameObject.SetActive(false));

        // Move along y-axis following the sine wave
        DOVirtual.Float(0, bulletInfo.travelTime, bulletInfo.travelTime, (t) =>
        {
            // Calculate the new y position using the sine function and apply the shift to our og y
            float newY = bulletInfo.sineAmplitude * Mathf.Sin(t * bulletInfo.sineFrequency * 2);
            transform.position = new Vector2(transform.position.x, startPos.y + newY);
        }).SetEase(Ease.Linear);
    }
}
