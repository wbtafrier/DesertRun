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

    public static float desertObjectSpeed = -5f;

    private static Stack<DesertObject> cactiStack = new Stack<DesertObject>();
    private static Stack<DesertObject> rockStack = new Stack<DesertObject>();
    private static Stack<DesertObject> snakeStack = new Stack<DesertObject>();

    private float desertTimer = 0f;

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
            if (desertTimer >= 2f)
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
                desertTimer = 0f;
            }
        }
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
