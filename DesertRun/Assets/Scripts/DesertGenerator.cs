using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesertGenerator : MonoBehaviour
{
    [SerializeField] List<GameObject> cactiList;
    [SerializeField] List<GameObject> rockList;
    [SerializeField] List<GameObject> snakeList;
    [SerializeField] List<GameObject> tallCactiList;
    [SerializeField] List<GameObject> balloonList;
    [SerializeField] bool debugOn = false;

    public static readonly float SPAWN_X = 11.41f;
    public static readonly float SPAWN_Y_CACTUS = -1.522f;
    public static readonly float SPAWN_Y_ROCK = -2.128f;
    public static readonly float SPAWN_Y_SNAKE = -1.64f;
    public static readonly float SPAWN_Y_TALL_CACTUS = -1.37f;
    public static readonly float SPAWN_Y_BALLOON = 1.83f;

    private static Stack<DesertObject> cactiStack = new Stack<DesertObject>();
    private static Stack<DesertObject> rockStack = new Stack<DesertObject>();
    private static Stack<DesertObject> snakeStack = new Stack<DesertObject>();
    private static Stack<DesertObject> tallCactiStack = new Stack<DesertObject>();
    private static Stack<DesertObject> balloonStack = new Stack<DesertObject>();

    private float desertTimer = 0f;
    private float balloonTimer = 0f;

    private static readonly float INIT_DESERT_TIMER_CURR_MIN = 1.5f;
    private static readonly float INIT_DESERT_TIMER_CURR_MAX = 2f;
    private static readonly float INIT_DESERT_TIMER_CURR_DURATION = 2f;
    private static readonly float INIT_DESERT_OBJECT_SPEED = -5f;
    private static readonly float BALLOON_TIMER_MIN = 3f;

    private static readonly float[] SCORE_CHECKPOINTS = new float[] {
        1000f, 3000f, 5000f
    };
    private static readonly int CHECKPOINTS = SCORE_CHECKPOINTS.Length;

    private float desertTimerCurrMin = INIT_DESERT_TIMER_CURR_MIN;
    private float desertTimerCurrMax = INIT_DESERT_TIMER_CURR_MAX;
    private float desertTimerCurrDuration = INIT_DESERT_TIMER_CURR_DURATION;
    public static float desertObjectSpeed = INIT_DESERT_OBJECT_SPEED;
    private static float balloonTimerMin = BALLOON_TIMER_MIN;

    private int currCheckpoint = -1;
    private bool[] scoreCheckpointsMet = new bool[CHECKPOINTS];

    private bool lastSpawnWasDouble = false;
    private bool spawnBreak = false;

    // Start is called before the first frame update
    void Start()
    {
        currCheckpoint = -1;
        for (int i = 0; i < scoreCheckpointsMet.Length; i++)
        {
            scoreCheckpointsMet[i] = false;
        }

        foreach (GameObject obj in cactiList)
        {
            cactiStack.Push(obj.GetComponent<DesertObject>());
        }

        foreach (GameObject obj in rockList)
        {
            rockStack.Push(obj.GetComponent<DesertObject>());
        }

        foreach (GameObject obj in snakeList)
        {
            snakeStack.Push(obj.GetComponent<DesertObject>());
        }

        foreach (GameObject obj in tallCactiList)
        {
            tallCactiStack.Push(obj.GetComponent<DesertObject>());
        }

        foreach (GameObject obj in balloonList)
        {
            balloonStack.Push(obj.GetComponent<DesertObject>());
        }
    }

    public void Restart()
    {
        desertTimer = 0f;
        balloonTimer = 0f;
        spawnBreak = false;
        desertTimerCurrMin = INIT_DESERT_TIMER_CURR_MIN;
        desertTimerCurrMax = INIT_DESERT_TIMER_CURR_MAX;
        desertTimerCurrDuration = INIT_DESERT_TIMER_CURR_DURATION;
        desertObjectSpeed = INIT_DESERT_OBJECT_SPEED;
        //timer1kCheckpoint = false;
        //timer5kCheckpoint = false;
        //timer10kCheckpoint = false;
        currCheckpoint = -1;
        for (int i = 0; i < scoreCheckpointsMet.Length; i++)
        {
            scoreCheckpointsMet[i] = false;
        }
        lastSpawnWasDouble = false;
    }

    public void OnGenEnable()
    {
        Start();
        foreach (GameObject obj in cactiList)
        {
            obj.SetActive(true);
        }

        foreach (GameObject obj in rockList)
        {
            obj.SetActive(true);
        }

        foreach (GameObject obj in snakeList)
        {
            obj.SetActive(true);
        }

        foreach (GameObject obj in tallCactiList)
        {
            obj.SetActive(true);
        }

        foreach (GameObject obj in balloonList)
        {
            obj.SetActive(true);
        }
    }

    public void OnGenDisable()
    {
        desertTimer = 0f;
        balloonTimer = 0f;
        spawnBreak = false;
        foreach (GameObject obj in cactiList)
        {
            obj.GetComponent<DesertObject>().ReturnToInitialPosition();
        }

        foreach (GameObject obj in rockList)
        {
            obj.GetComponent<DesertObject>().ReturnToInitialPosition();
        }

        foreach (GameObject obj in snakeList)
        {
            obj.GetComponent<DesertObject>().ReturnToInitialPosition();
        }

        foreach (GameObject obj in tallCactiList)
        {
            obj.GetComponent<DesertObject>().ReturnToInitialPosition();
        }

        foreach (GameObject obj in balloonList)
        {
            Balloon b = obj.GetComponent<Balloon>();
            b.ReturnToInitialPosition();
            b.ResetSparkles();
        }
        cactiStack.Clear();
        rockStack.Clear();
        snakeStack.Clear();
        tallCactiStack.Clear();
        balloonStack.Clear();

        foreach (GameObject obj in cactiList)
        {
            obj.SetActive(false);
        }

        foreach (GameObject obj in rockList)
        {
            obj.SetActive(false);
        }

        foreach (GameObject obj in snakeList)
        {
            obj.SetActive(false);
        }

        foreach (GameObject obj in tallCactiList)
        {
            obj.SetActive(false);
        }

        foreach (GameObject obj in balloonList)
        {
            obj.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (debugOn)
        {
            return;
        }

        if (GameController.IsBalloonDebugOn() && balloonTimerMin == BALLOON_TIMER_MIN)
        {
            balloonTimerMin = 0.1f;
        }

        desertTimer += Time.deltaTime;
        if (!GameController.IsRestarting() && !GameController.IsPlayerEnteringScene() && !GameStateMachine.IsGameOver())
        {
            float score = GameController.GetRoughScore();
            if (desertTimer >= desertTimerCurrDuration && !spawnBreak)
            {
                DesertObject obj = FindNextObstacle();
                float y = SPAWN_Y_CACTUS;

                if (obj.gameObject.tag.Equals("Rock"))
                {
                    y = SPAWN_Y_ROCK;
                }
                else if (obj.gameObject.tag.Equals("Snake"))
                {
                    y = SPAWN_Y_SNAKE;
                }
                else if (obj.gameObject.tag.Equals("TallCactus"))
                {
                    y = SPAWN_Y_TALL_CACTUS;
                }

                obj.gameObject.transform.position = new Vector3(SPAWN_X, y, obj.gameObject.transform.position.z);
                obj.Activate();
                desertTimerCurrDuration = GetNextDesertTimerDuration(score);
                desertTimer = 0f;
            }

            balloonTimer += Time.deltaTime;
            if (balloonTimer >= balloonTimerMin && !GameController.IsReloInvincible())
            {
                int r = 0;
                if (!GameController.IsBalloonDebugOn())
                {
                    r = Random.Range(0, 3);
                    
                }

                if (r == 0)
                {
                    DesertObject balloon = RetrieveBalloon();

                    if (balloon != null)
                    {
                        balloon.gameObject.transform.position = new Vector3(SPAWN_X, SPAWN_Y_BALLOON,
                            balloon.gameObject.transform.position.z);
                        balloon.Activate();
                    }
                }
                balloonTimer = 0f;
            }

            int nextCheck = currCheckpoint + 1;
            if (nextCheck < SCORE_CHECKPOINTS.Length)
            {
                if (!scoreCheckpointsMet[nextCheck] && score >= SCORE_CHECKPOINTS[nextCheck])
                {
                    float gameSpeedMultiplier = 1.1f;

                    desertTimerCurrMax -= nextCheck != 2 ? 0.25f : 0f;
                    desertTimerCurrMin -= nextCheck == 0 ? 0.25f : 0.1f;
                    desertObjectSpeed *= gameSpeedMultiplier;
                    GameController.MultiplyGameSpeed(gameSpeedMultiplier);
                    scoreCheckpointsMet[nextCheck] = true;
                    currCheckpoint++;
                    spawnBreak = false;
                }
                else if (!spawnBreak && !scoreCheckpointsMet[nextCheck]
                    && score >= (SCORE_CHECKPOINTS[nextCheck] - (50 * (nextCheck + 1))))
                {
                    spawnBreak = true;
                }
            }
        }
    }

    private float GetNextDesertTimerDuration(float score)
    {
        if (currCheckpoint == 1 && !lastSpawnWasDouble)
        {
            int doubleSpawnChance = 10;
            int r = Random.Range(0, 100);

            if (r < doubleSpawnChance)
            {
                lastSpawnWasDouble = true;
                return !scoreCheckpointsMet[1] ? 0.3f : 0.2f;
            }
        }
        else if (lastSpawnWasDouble)
        {
            lastSpawnWasDouble = false;
        }

        return Random.Range(desertTimerCurrMin, desertTimerCurrMax);
    }

    private DesertObject FindNextObstacle()
    {
        int r = Random.Range(0, 100);
        DesertObject obj = null;

        if (r < 30 && cactiStack.Count > 0)
        {
            obj = cactiStack.Pop();
        }
        else if (r < 50 && tallCactiStack.Count > 0)
        {
            obj = tallCactiStack.Pop();
        }
        else if (r < 75 && rockStack.Count > 0)
        {
            obj = rockStack.Pop();
        }
        else if (r < 100 && snakeStack.Count > 0)
        {
            obj = snakeStack.Pop();
        }

        if (obj != null)
        {
            return obj;
        }
        else
        {
            return FindNextObstacle();
        }
    }

    private DesertObject RetrieveBalloon()
    {
        if (balloonStack.Count > 0)
        {
            return balloonStack.Pop();
        }
        return null;
    }

    public static void ReturnToStack(DesertObject obj)
    {
        string tag = obj.gameObject.tag;
        if (tag.Equals("Cactus") && !cactiStack.Contains(obj))
        {
            cactiStack.Push(obj);
        }
        else if (tag.Equals("Rock") && !rockStack.Contains(obj))
        {
            rockStack.Push(obj);
        }
        else if (tag.Equals("Snake") && !snakeStack.Contains(obj))
        {
            snakeStack.Push(obj);
        }
        else if (tag.Equals("TallCactus") && !tallCactiStack.Contains(obj))
        {
            tallCactiStack.Push(obj);
        }
        else if (tag.Equals("Balloon") && !balloonStack.Contains(obj))
        {
            balloonStack.Push(obj);
        }
    }
}
