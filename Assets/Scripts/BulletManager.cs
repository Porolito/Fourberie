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
    // void Update()
    // {
    //     if (ballSpawn.action.WasPressedThisFrame()) //Used for test, can be deleted or put in comm temporary
    //     {
    //         StartCoroutine(SpawnerPattern(spawnTimer, spawnCount, type)) ;
    //     }
    // }

    private GameObject SummonBall(string typePattern) //Re-use a ball or instantiate is needed
    {
        foreach (GameObject ballSummoned in _ballsPool)
        {
            if (ballSummoned.activeSelf) continue;
            ballSummoned.SetActive(true);
            return ballSummoned;
        }
        GameObject newBall = Instantiate(ball, Vector3.zero, Quaternion.identity);
        _ballsPool.Add(newBall);
        return newBall;
    }
    
    private void BallPatternMaker(BallPathScriptable ballPattern, GameObject ballSpawned, bool isDoubleSine) //Give pattern to ball and their spawn position
    {
        BallPath ballPath = ballSpawned.GetComponent<BallPath>();
        ballPath.endPosX = ballPattern.endPosX;
        ballPath.waveFrequency = ballPattern.waveFrequency;
        ballPath.waveAmplitude = ballPattern.waveAmplitude;
        ballPath.spawnPosition = ballSpawned.transform.position;
        if (isDoubleSine) ballPath.startY = -1;
        else  ballPath.startY = 1;
        ballPath.GiveAPath();
    }
    public IEnumerator SpawnerPattern(float spawnFrequency, int spawnQuantity, string typePattern) //Function to use to do a ball pattern
    {
        for (int i = 0; i < spawnQuantity; i++)
        {
            yield return new WaitForSeconds(spawnFrequency);
            var _lastBall = SummonBall(typePattern);
            switch (typePattern)
            {
                case "Straight":
                    BallPatternMaker(ballPathScriptable[0], _lastBall, false); 
                    break;
                case "Sine": 
                    BallPatternMaker(ballPathScriptable[1], _lastBall, false); 
                    break;
                case "SineBis": 
                    BallPatternMaker(ballPathScriptable[1], _lastBall, false);
                    var _lastBall1 = SummonBall(typePattern);
                    BallPatternMaker(ballPathScriptable[1], _lastBall1, true);
                    break;
            }
        }
    }
}
