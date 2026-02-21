using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BulletManager : MonoBehaviour
{
    private List<GameObject> _ballsPool =  new List<GameObject>();
    private Coroutine ActualCoroutine;

    [SerializeField] private GameObject[] ballPrefabs;
    [SerializeField] [Tooltip("Left/Right distance of spawning bullets")] private float xOffset = 10f;
    [SerializeField] [Tooltip("Low/Medium/High height of spawning bullets")] private float yOffset = 2;
    
    [Serializable]
    public struct BulletInfo
    {
        public float spawnFrequency;
        public int spawnQuantity;
        public float waveCooldown;
        [Tooltip("Leave at 0 for strait ball")] public float sineFrequency;
        public float sineAmplitude;
        public float travelTime;
        public SpawnHeight spawnHeight;
        public bool spawnAtLeft;
        
        public enum SpawnHeight
        {
            Low,
            Medium,
            High
        }
    }

    private GameObject SummonBall() //Re-use a ball or instantiate is needed
    {
        foreach (GameObject ballSummoned in _ballsPool)
        {
            if (ballSummoned.activeSelf) continue;
            ballSummoned.SetActive(true);
            return ballSummoned;
        }
        GameObject newBall = Instantiate(GetRandomBallPrefab(), Vector3.zero, Quaternion.identity);
        _ballsPool.Add(newBall);
        return newBall;
    }

    private GameObject GetRandomBallPrefab()
    {
        return Random.value switch
        {
            < 0.45f => ballPrefabs[0],
            < 0.95f => ballPrefabs[1],
            _ => ballPrefabs[2]
        };
    }

    private Vector2 GetSpawnPosition(BulletInfo bulletInfo)
    {
        float ballSpawnPosX = bulletInfo.spawnAtLeft ? transform.position.x - xOffset : transform.position.x + xOffset;
        return bulletInfo.spawnHeight switch
        {
            BulletInfo.SpawnHeight.Low => new Vector2(ballSpawnPosX, transform.position.y - yOffset),
            BulletInfo.SpawnHeight.Medium => new Vector2(ballSpawnPosX, transform.position.y),
            BulletInfo.SpawnHeight.High => new Vector2(ballSpawnPosX, transform.position.y + yOffset)
        };
    }
    
    IEnumerator SpawnerPattern(BulletInfo bulletInfo) //Function to use to do a ball pattern
    {
        for (int i = 0; i < bulletInfo.spawnQuantity; i++)
        {
            yield return new WaitForSeconds(bulletInfo.spawnFrequency);
            var currBall = SummonBall();
            currBall.GetComponent<BallPath>().Init(GetSpawnPosition(bulletInfo), xOffset*2, bulletInfo);
        }
        yield return new WaitForSeconds(bulletInfo.waveCooldown);
        LaunchCoroutine(bulletInfo);
    }

    public void LaunchCoroutine(BulletInfo bulletInfo)
    {
        ActualCoroutine = StartCoroutine(SpawnerPattern(bulletInfo));
    }

    public void CancelCoroutine()
    {
        if (ActualCoroutine != null) StopCoroutine(ActualCoroutine);
    }
}
