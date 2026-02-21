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
    [SerializeField] private float xOffset = 10f;
    
    [Serializable]
    public struct BulletInfo
    {
        public float spawnFrequency;
        public int spawnQuantity;
        public float timeBeforeNewWave;
        public float sinFrequency;
        public float sinMagnitude;
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
        GameObject newBall = Instantiate(GetBallPrefab(), Vector3.zero, Quaternion.identity);
        _ballsPool.Add(newBall);
        return newBall;
    }

    private GameObject GetBallPrefab()
    {
        return Random.value switch
        {
            < 0.45f => ballPrefabs[0],
            < 0.9f => ballPrefabs[1],
            _ => ballPrefabs[2]
        };
    }
    
    private void BallPatternMaker(GameObject ballSpawned, bool isDoubleSine, float sinFrequency, float sinMagnitude, float ballSpeed) //Give pattern to ball and their spawn position
    {
        BallPath ballPath = ballSpawned.GetComponent<BallPath>();
        ballPath.waveFrequency = sinFrequency;
        ballPath.waveAmplitude = sinMagnitude;
        ballPath.spawnPosition = ballSpawned.transform.position;
        ballPath.moveDuration = ballSpeed;
        if (isDoubleSine) ballPath.startY = -1;
        else  ballPath.startY = 1;
        ballPath.GiveAPath();
    }
    
    
    IEnumerator SpawnerPattern(BulletInfo bulletInfo) //Function to use to do a ball pattern
    {
        for (int i = 0; i < bulletInfo.spawnQuantity; i++)
        {
            yield return new WaitForSeconds(bulletInfo.spawnFrequency);
            var _lastBall = SummonBall();
            BallPatternMaker(_lastBall, false,  bulletInfo.sinFrequency, bulletInfo.sinMagnitude,bulletInfo.travelTime);
        }
        yield return new WaitForSeconds(bulletInfo.timeBeforeNewWave);
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
