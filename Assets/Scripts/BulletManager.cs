using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BulletManager : MonoBehaviour
{
    private List<GameObject> _ballsPool =  new List<GameObject>();
    [SerializeField] private GameObject ball;
    [SerializeField] private BallPathScriptable[] ballPathScriptable;
    
    [SerializeField] private InputActionReference ballSpawn;

    public float spawnTimer;
    public int spawnCount;
    public string type;

    // Update is called once per frame
    void Update()
    {
        if (ballSpawn.action.WasPressedThisFrame())
        {
            StartCoroutine(SpawnerPattern(spawnTimer, spawnCount, type)) ;
        }
    }

    private GameObject SummonBall(string typePattern)
    {
        if (_ballsPool.Count <= 30)
        {
            return Instantiate(ball, Vector3.zero, Quaternion.identity);
        }
        else
        {
            for (int i = 0; i < _ballsPool.Count; i++)
            {
                if (_ballsPool[i].activeSelf) continue;
                return _ballsPool[i];
            }
        }
        return null;
    }
    
    private void BallPatternMaker(BallPathScriptable ballPattern, GameObject ballSpawned)
    {
        BallPath ballPath = ballSpawned.GetComponent<BallPath>();
        ballPath.endPosX = ballPattern.endPosX;
        ballPath.waveFrequency = ballPattern.waveFrequency;
        ballPath.waveAmplitude = ballPattern.waveAmplitude;
        ballPath.spawnPosition = ballSpawned.transform.position;
        ballPath.GiveAPath();
    }

    private void DoubleSin(BallPathScriptable ballPattern, GameObject ballSpawned)
    {
        BallPath ballPath = ballSpawned.GetComponent<BallPath>();
        ballPath.endPosX = ballPattern.endPosX;
        ballPath.waveFrequency = ballPattern.waveFrequency;
        ballPath.waveAmplitude = ballPattern.waveAmplitude;
        ballPath.spawnPosition = ballSpawned.transform.position;
        ballPath.startY = -1;
        ballPath.GiveAPath();
    }
    IEnumerator SpawnerPattern(float spawnFrequency, int spawnQuantity, string typePattern)
    {
        for (int i = 0; i < spawnQuantity; i++)
        {
            yield return new WaitForSeconds(spawnFrequency);
            var _lastBall = SummonBall(typePattern);
            switch (typePattern)
            {
                case "Straight":
                    BallPatternMaker(ballPathScriptable[0], _lastBall); 
                    break;
                case "Sine": 
                    BallPatternMaker(ballPathScriptable[1], _lastBall); 
                    break;
                case "SineBis": 
                    BallPatternMaker(ballPathScriptable[1], _lastBall);
                    //_lastBall = SummonBall(typePattern);
                    //DoubleSin(ballPathScriptable[1], _lastBall);
                    break;
            }
        }
    }
}
