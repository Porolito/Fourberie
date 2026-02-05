using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BulletManager : MonoBehaviour
{
    private List<GameObject> _ballsPool =  new List<GameObject>();
    //[SerializeField] private GameObject ball;
    [SerializeField] private BallPathScriptable[] ballPathScriptable;
    [SerializeField] private GameObject[] ballPrefabs;
    private bool skinPeople = false;
    [SerializeField] private InputActionReference ballSpawn;
    [SerializeField] private InputActionReference ballMove;

    public float spawnTimer;
    public int spawnCount;
    public string type;

    private Coroutine ActualCoroutine;
    
    public enum Pattern
    {
        Straight,
        Sine,
        SineBis
    }
    // Update is called once per frame
    // void Update()
    // {
    //     if (ballSpawn.action.WasPressedThisFrame()) //Used for test, can be deleted or put in comm temporary
    //     {
    //         LaunchCoroutine(spawnTimer, spawnCount, type, 3) ;
    //     }
    //     if  (ballMove.action.WasPressedThisFrame()) StopCoroutine(ActualCoroutine);
    // }

    private GameObject SummonBall() //Re-use a ball or instantiate is needed
    {
        foreach (GameObject ballSummoned in _ballsPool)
        {
            if (ballSummoned.activeSelf) continue;
            ballSummoned.SetActive(true);
            return ballSummoned;
        }
        int rand;
        if (!skinPeople)
        {
            rand = Random.Range(0, ballPrefabs.Length);
            skinPeople = true;
        }
        else rand = Random.Range(0, ballPrefabs.Length - 1);
        GameObject newBall = Instantiate(ballPrefabs[rand], Vector3.zero, Quaternion.identity);
        _ballsPool.Add(newBall);
        return newBall;
    }
    
    private void BallPatternMaker(BallPathScriptable ballPattern, GameObject ballSpawned, bool isDoubleSine, float sinFrequency, float ballSpeed) //Give pattern to ball and their spawn position
    {
        BallPath ballPath = ballSpawned.GetComponent<BallPath>();
        ballPath.endPosX = ballPattern.endPosX;
        ballPath.waveFrequency = sinFrequency;
        ballPath.waveAmplitude = ballPattern.waveAmplitude;
        ballPath.spawnPosition = ballSpawned.transform.position;
        ballPath.moveDuration = ballSpeed;
        if (isDoubleSine) ballPath.startY = -1;
        else  ballPath.startY = 1;
        ballPath.GiveAPath();
    }
    IEnumerator SpawnerPattern(float spawnFrequency, int spawnQuantity, Pattern pattern, float timeBeforeNewWave, float sinFrequency, float ballSpeed) //Function to use to do a ball pattern
    {
        for (int i = 0; i < spawnQuantity; i++)
        {
            yield return new WaitForSeconds(spawnFrequency);
            var _lastBall = SummonBall();
            switch (pattern)
            {
                case Pattern.Straight:
                    BallPatternMaker(ballPathScriptable[0], _lastBall, false, sinFrequency, ballSpeed); 
                    break;
                case Pattern.Sine: 
                    BallPatternMaker(ballPathScriptable[1], _lastBall, false,  sinFrequency, ballSpeed); 
                    break;
                case Pattern.SineBis: 
                    BallPatternMaker(ballPathScriptable[1], _lastBall, false,  sinFrequency, ballSpeed);
                    var _lastBall1 = SummonBall();
                    BallPatternMaker(ballPathScriptable[1], _lastBall1, true, sinFrequency, ballSpeed);
                    break;
            }
        }
        yield return new WaitForSeconds(timeBeforeNewWave);
        LaunchCoroutine(spawnFrequency, spawnQuantity, pattern, timeBeforeNewWave, sinFrequency, ballSpeed);
    }

    public void LaunchCoroutine(float spawnFrequency, int spawnQuantity, Pattern pattern, float timeBeforeNewWave, float  sinFrequency,  float ballSpeed)
    {
        ActualCoroutine = StartCoroutine(SpawnerPattern(spawnFrequency, spawnQuantity, pattern, timeBeforeNewWave,  sinFrequency, ballSpeed));
    }

    public void CancelCoroutine()
    {
        if (ActualCoroutine != null) StopCoroutine(ActualCoroutine);
    }
}
