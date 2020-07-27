using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesertGenerator : MonoBehaviour
{
    [SerializeField] List<GameObject> cactiList;
    [SerializeField] List<GameObject> rockList;
    [SerializeField] List<GameObject> snakeList;
    [SerializeField] bool debugOn = false;

    public static readonly float SPAWN_X = 11.41f;
    public static readonly float SPAWN_Y_CACTUS = -1.522f;
    public static readonly float SPAWN_Y_ROCK = -1.994309f;
    public static readonly float SPAWN_Y_SNAKE = -1.64f;

    private static Stack<DesertObject> cactiStack = new Stack<DesertObject>();
    private static Stack<DesertObject> rockStack = new Stack<DesertObject>();
    private static Stack<DesertObject> snakeStack = new Stack<DesertObject>();

    private float desertTimer = 0f;

    private static readonly float INIT_DESERT_TIMER_CURR_MIN = 1.5f;
    private static readonly float INIT_DESERT_TIMER_CURR_MAX = 2f;
    private static readonly float INIT_DESERT_TIMER_CURR_DURATION = 2f;
    private static readonly float INIT_DESERT_OBJECT_SPEED = -5f;

    private float desertTimerCurrMin = INIT_DESERT_TIMER_CURR_MIN;
    private float desertTimerCurrMax = INIT_DESERT_TIMER_CURR_MAX;
    private float desertTimerCurrDuration = INIT_DESERT_TIMER_CURR_DURATION;
    public static float desertObjectSpeed = INIT_DESERT_OBJECT_SPEED;

    private bool[] scoreCheckpoints = new bool[3];

    private bool lastSpawnWasDouble = false;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < scoreCheckpoints.Length; i++)
        {
            scoreCheckpoints[i] = false;
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
    }

    public void Restart()
    {
        desertTimer = 0f;
        desertTimerCurrMin = INIT_DESERT_TIMER_CURR_MIN;
        desertTimerCurrMax = INIT_DESERT_TIMER_CURR_MAX;
        desertTimerCurrDuration = INIT_DESERT_TIMER_CURR_DURATION;
        desertObjectSpeed = INIT_DESERT_OBJECT_SPEED;
        //timer1kCheckpoint = false;
        //timer5kCheckpoint = false;
        //timer10kCheckpoint = false;
        for (int i = 0; i < scoreCheckpoints.Length; i++)
        {
            scoreCheckpoints[i] = false;
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
    }

    public void OnGenDisable()
    {
        desertTimer = 0f;
        cactiStack.Clear();
        rockStack.Clear();
        snakeStack.Clear();

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
    }

    // Update is called once per frame
    void Update()
    {
        if (debugOn)
        {
            return;
        }

        desertTimer += Time.deltaTime;
        if (!GameController.IsRestarting() && !GameController.IsPlayerEnteringScene() && !GameStateMachine.IsGameOver()
            && transform.position.x >= -17.5)
        {
            float score = GameController.GetRoughScore();

            if (desertTimer >= desertTimerCurrDuration)
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

                obj.gameObject.transform.position = new Vector3(SPAWN_X, y, obj.gameObject.transform.position.z);
                obj.Activate();
                desertTimerCurrDuration = GetNextDesertTimerDuration(score);
                desertTimer = 0f;
            }

            if (!scoreCheckpoints[0] && score >= 1000)
            {
                desertTimerCurrMax -= 0.25f;
                desertTimerCurrMin -= 0.25f;
                desertObjectSpeed *= 1.2f;
                GameController.MultiplyGameSpeed(1.1f);
                scoreCheckpoints[0] = true;
            }
            else if (!scoreCheckpoints[1] && score >= 3000)
            {
                desertTimerCurrMax -= 0.25f;
                desertTimerCurrMin -= 0.15f;
                desertObjectSpeed *= 1.2f;
                GameController.MultiplyGameSpeed(1.1f);
                scoreCheckpoints[1] = true;
            }
            else if (!scoreCheckpoints[2] && score >= 5000)
            {
                desertTimerCurrMax -= 0.25f;
                desertTimerCurrMin -= 0.15f;
                desertObjectSpeed *= 1.2f;
                GameController.MultiplyGameSpeed(1.1f);
                scoreCheckpoints[2] = true;
            }
        }
    }

    private float GetNextDesertTimerDuration(float score)
    {
        if (scoreCheckpoints[1] && !lastSpawnWasDouble)
        {
            int doubleSpawnChance = 10;
            int r = Random.Range(0, 100);

            if (r < doubleSpawnChance)
            {
                lastSpawnWasDouble = true;
                return !scoreCheckpoints[1] ? 0.3f : 0.2f;
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

        if (r < 50 && cactiStack.Count > 0)
        {
            obj = cactiStack.Pop();
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
    }
}
