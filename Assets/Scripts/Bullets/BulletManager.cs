using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class BulletManager : MonoBehaviour
{
    private List<GameObject> _ballsPool =  new List<GameObject>();

    [Header("Settings")]
    [SerializeField] private GameObject[] ballPrefabs;
    [SerializeField] [Tooltip("Left/Right distance of spawning bullets")] private float xOffset = 10f;
    [SerializeField] [Tooltip("Low/Medium/High height of spawning bullets")] private float yOffset = 2;

    [Header("Debug")]
    [SerializeField] private SO_Sequence.BulletPattern m_TestBulletPattern;
    [SerializeField] private InputActionProperty m_StartDebugWave;
    [SerializeField] private InputActionProperty m_StopAllWaves;

#if UNITY_EDITOR
    private void Awake()
    {
        m_StartDebugWave.action.Enable();
        m_StopAllWaves.action.Enable();
        
        m_StartDebugWave.action.performed += _ =>
        {
            foreach (var bulletInfo in m_TestBulletPattern.bulletInfos)
                StartWave(bulletInfo);
        };
        m_StopAllWaves.action.performed += _ => StopWaves();
    }
#endif

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

    private Vector2 GetSpawnPosition(SO_BulletInfo bulletInfo)
    {
        float ballSpawnPosX = bulletInfo.spawnAtLeft ? transform.position.x - xOffset : transform.position.x + xOffset;
        return bulletInfo.spawnHeight switch
        {
            SO_BulletInfo.SpawnHeight.Low => new Vector2(ballSpawnPosX, transform.position.y - yOffset),
            SO_BulletInfo.SpawnHeight.Medium => new Vector2(ballSpawnPosX, transform.position.y),
            SO_BulletInfo.SpawnHeight.High => new Vector2(ballSpawnPosX, transform.position.y + yOffset),
            _ => Vector2.zero
        };
    }
    
    IEnumerator SpawnRoutine(SO_BulletInfo bulletInfo) //Function to use to do a ball pattern
    {
        for (int i = 0; i < bulletInfo.spawnQuantity; i++)
        {
            yield return new WaitForSeconds(bulletInfo.spawnFrequency);
            var currBall = SummonBall();
            currBall.GetComponent<BallPath>().Init(GetSpawnPosition(bulletInfo), xOffset*2, bulletInfo);
        }
        yield return new WaitForSeconds(bulletInfo.waveCooldown);
        StartWave(bulletInfo);
    }

    public void StartWave(SO_BulletInfo bulletInfo)
    {
        StartCoroutine(SpawnRoutine(bulletInfo));
    }

    public void StopWaves()
    {
        StopAllCoroutines();
    }
}
