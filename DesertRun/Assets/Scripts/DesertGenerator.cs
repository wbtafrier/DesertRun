using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesertGenerator : MonoBehaviour
{
    [SerializeField] List<GameObject> cactiList;
    [SerializeField] List<GameObject> rockList;
    [SerializeField] List<GameObject> snakeList;

    public static readonly float SPAWN_X = 11.41f;
    public static readonly float SPAWN_Y_CACTUS = -1.522f;
    public static readonly float SPAWN_Y_ROCK = -1.994309f;
    public static readonly float SPAWN_Y_SNAKE = -1.64f;

    private static Stack<DesertObject> cactiStack = new Stack<DesertObject>();
    private static Stack<DesertObject> rockStack = new Stack<DesertObject>();
    private static Stack<DesertObject> snakeStack = new Stack<DesertObject>();

    private float desertTimer = 0f;

    private static readonly float INIT_DESERT_TIMER_CURR_MIN = 1.75f;
    private static readonly float INIT_DESERT_TIMER_CURR_MAX = 3f;
    private static readonly float INIT_DESERT_TIMER_CURR_DURATION = 2f;
    private static readonly float INIT_DESERT_OBJECT_SPEED = -5f;

    private float desertTimerCurrMin = INIT_DESERT_TIMER_CURR_MIN;
    private float desertTimerCurrMax = INIT_DESERT_TIMER_CURR_MAX;
    private float desertTimerCurrDuration = INIT_DESERT_TIMER_CURR_DURATION;
    public static float desertObjectSpeed = INIT_DESERT_OBJECT_SPEED;

    private bool speed1kCheckpoint = false;
    //private bool speed2kCheckpoint = false;
    private bool speed3kCheckpoint = false;
    //private bool speed4kCheckpoint = false;
    //private bool speed5kCheckpoint = false;
    //private bool speed6kCheckpoint = false;
    private bool speed7kCheckpoint = false;
    //private bool speed8kCheckpoint = false;
    //private bool speed9kCheckpoint = false;
    //private bool speed10kCheckpoint = false;

    private bool lastSpawnWasDouble = false;

    // Start is called before the first frame update
    void Start()
    {
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
        speed1kCheckpoint = false;
        speed3kCheckpoint = false;
        speed7kCheckpoint = false;
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
        desertTimer += Time.deltaTime;
        if (!GameController.IsRestarting() && !GameController.IsPlayerEnteringScene() && !GameStateMachine.IsGameOver() && transform.position.x >= -17.5)
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

            if (!speed1kCheckpoint && score >= 1000)
            {
                desertTimerCurrMax -= 0.5f;
                desertTimerCurrMin -= 0.25f;
                desertObjectSpeed *= 1.2f;
                GameController.MultiplyGameSpeed(1.2f);
                speed1kCheckpoint = true;
            }
            else if (!speed3kCheckpoint && score >= 3000)
            {
                desertTimerCurrMax -= 0.25f;
                desertTimerCurrMin -= 0.25f;
                desertObjectSpeed *= 1.2f;
                GameController.MultiplyGameSpeed(1.2f);
                speed3kCheckpoint = true;
            }
            else if (!speed7kCheckpoint && score >= 7000)
            {
                desertTimerCurrMax -= 0.25f;
                desertTimerCurrMin -= 0.25f;
                desertObjectSpeed *= 1.2f;
                GameController.MultiplyGameSpeed(1.2f);
                speed7kCheckpoint = true;
            }
        }
    }

    private float GetNextDesertTimerDuration(float score)
    {
        if (!lastSpawnWasDouble)
        {
            int doubleSpawnChance = 10;
            int r = Random.Range(0, 100);

            if (r < doubleSpawnChance)
            {
                lastSpawnWasDouble = true;
                return score < 7000 ? 0.3f : 0.2f;
            }
        }
        else
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
