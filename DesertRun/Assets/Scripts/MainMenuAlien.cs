using UnityEngine;

public class MainMenuAlien : MonoBehaviour
{
    [SerializeField] bool rotateAbove = false;
    [SerializeField] float exitPositionX = 0;
    [SerializeField] float exitPositionY = 0;

    private readonly float ROTATION_SPEED = 20f;
    private readonly float CIRCLING_SPEED = 3f;
    private readonly float POP_SPEED = 15f;
    private readonly float EXIT_SPEED = 5f;

    private readonly float MAX_REACH = 1f;
    private readonly float MIN_REACH = 0.75f;

    private Vector3 initPos = Vector3.zero;
    private Vector3 centerPos = Vector3.zero;
    private Vector3 exitPos = Vector3.zero;
    private Vector3[] circlePoints;
    private int currCircleIndex = 0;

    int rotateDir = 0;
    float currRot = 0f;
    bool popping = false;
    bool popTargetMet = false;
    Vector3 popTarget = Vector3.zero;
    Vector3 prePopPos = Vector3.zero;
    private bool entering = false;
    private bool exiting = false;
    private bool hasExited = false;

    // Start is called before the first frame update
    void Start()
    {
        initPos = transform.position;
        float initY = initPos.y;
        float centerX = initPos.x;
        float centerY = rotateAbove ? initY + MAX_REACH : initY - MAX_REACH;
        float centerZ = initPos.z;

        if (rotateAbove)
        {
            circlePoints = new Vector3[] {
                initPos,
                new Vector3(centerX - MIN_REACH, centerY - MIN_REACH, centerZ),
                new Vector3(centerX - MAX_REACH, centerY, centerZ),
                new Vector3(centerX - MIN_REACH, centerY + MIN_REACH, centerZ),
                new Vector3(centerX, centerY + MAX_REACH, centerZ),
                new Vector3(centerX + MIN_REACH, centerY + MIN_REACH, centerZ),
                new Vector3(centerX + MAX_REACH, centerY, centerZ),
                new Vector3(centerX + MIN_REACH, centerY - MIN_REACH, centerZ),
            };
        }
        else
        {
            circlePoints = new Vector3[] {
                initPos,
                new Vector3(centerX + MIN_REACH, centerY + MIN_REACH, centerZ),
                new Vector3(centerX + MAX_REACH, centerY, centerZ),
                new Vector3(centerX + MIN_REACH, centerY - MIN_REACH, centerZ),
                new Vector3(centerX, centerY - MAX_REACH, centerZ),
                new Vector3(centerX - MIN_REACH, centerY - MIN_REACH, centerZ),
                new Vector3(centerX - MAX_REACH, centerY, centerZ),
                new Vector3(centerX - MIN_REACH, centerY + MIN_REACH, centerZ),
            };
        }

        exitPos = new Vector3(exitPositionX, exitPositionY, centerZ);
    }

    // Update is called once per frame
    void Update()
    {
        if (!entering && !exiting)
        {
            if (!popping)
            {
                Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(new Vector2(point.x, point.y), Vector2.zero, 0);
                if (hit && hit.collider && hit.collider.CompareTag("MenuAlien"))
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        hit.collider.GetComponent<MainMenuAlien>().Pop();
                    }
                }

                float deltaRot = rotateDir == 0 ? -ROTATION_SPEED * Time.deltaTime : ROTATION_SPEED * Time.deltaTime;
                currRot += deltaRot;

                transform.Rotate(new Vector3(0f, 0f, deltaRot));
                if (currRot <= -45f)
                {
                    rotateDir = 1;
                }
                else if (currRot >= 45f)
                {
                    rotateDir = 0;
                }

                int nextCircleIndex = currCircleIndex != circlePoints.Length - 1 ? currCircleIndex + 1 : 0;
                Vector3 currPos = transform.position;
                Vector3 nextPos = circlePoints[nextCircleIndex];
                if (!currPos.Equals(nextPos))
                {
                    transform.position = Vector3.MoveTowards(currPos, nextPos, CIRCLING_SPEED * Time.deltaTime);
                }
                else
                {
                    currCircleIndex = nextCircleIndex;
                }

            }
            else if (popping && popTarget.Equals(Vector3.zero))
            {
                Vector3 currPos = transform.position;
                prePopPos = currPos;

                float xRand = Random.Range(-5f, 5f);
                float yRand = Random.Range(-5f, 5f);
                float absX = System.Math.Abs(xRand);
                float absY = System.Math.Abs(yRand);

                if (absX < 2f && absY < 2f)
                {
                    int r = Random.Range(0, 2);
                    if (r == 0)
                    {
                        float sign = xRand / absX;
                        xRand += sign * 2f;
                    }
                    else
                    {
                        float sign = yRand / absY;
                        yRand += sign * 2f;
                    }
                }

                popTarget = new Vector3(currPos.x + xRand, currPos.y + yRand, currPos.z);
            }
            else if (popping && !popTargetMet)
            {
                Vector3 currPos = transform.position;
                transform.position = Vector3.MoveTowards(currPos, popTarget, POP_SPEED * Time.deltaTime);
                if (transform.position.Equals(popTarget))
                {
                    popTargetMet = true;
                }
            }
            else if (popping && popTargetMet)
            {
                Vector3 currPos = transform.position;
                transform.position = Vector3.MoveTowards(currPos, prePopPos, POP_SPEED * Time.deltaTime);
                if (transform.position.Equals(prePopPos))
                {
                    popping = false;
                    popTarget = Vector3.zero;
                    popTargetMet = false;
                }
            }
        }
        else if (!entering && exiting && !hasExited)
        {
            if (popping || popTargetMet)
            {
                popping = false;
                popTarget = Vector3.zero;
                popTargetMet = false;
            }

            Vector3 currPos = transform.position;
            if (!currPos.Equals(exitPos))
            {
                transform.position = Vector3.MoveTowards(currPos, exitPos, EXIT_SPEED * Time.deltaTime);
            }
            else
            {
                hasExited = true;
            }
        }
        else if (entering && !exiting && !hasExited)
        {
            Vector3 currPos = transform.position;
            if (!currPos.Equals(initPos))
            {
                transform.position = Vector3.MoveTowards(currPos, initPos, EXIT_SPEED * Time.deltaTime);
            }
            else
            {
                entering = false;
            }
        }
    }
    
    void Pop()
    {
        popping = true;
    }

    public void Exit()
    {
        exiting = true;
    }

    public bool HasExited()
    {
        return hasExited;
    }

    public void ResetComp()
    {
        exiting = false;
        hasExited = false;
        entering = true;
    }
}
