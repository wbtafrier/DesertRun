using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuSign : MonoBehaviour
{
    private readonly float MAX_MOVEMENT = 0.4f;
    private readonly float SIGN_SPEED = 0.3f;
    private readonly float SIGN_EXIT_SPEED = 3f;
    private readonly float EXIT_Y = 6.9f;

    private Vector3 initPos = Vector3.zero;
    private Vector3 leftPos, rightPos;
    private Vector3 exitPos = Vector3.zero;
    private bool movingLeft = true;
    private bool exiting = false;
    private bool hasExited = false;

    // Start is called before the first frame update
    void Start()
    {
        initPos = transform.position;

        float initX = initPos.x;
        float initY = initPos.y;
        float initZ = initPos.z;
        leftPos = new Vector3(initX - MAX_MOVEMENT, initY, initZ);
        rightPos = new Vector3(initX + MAX_MOVEMENT, initY, initZ);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currPos = transform.position;
        Vector3 targetPos;
        
        if (!exiting)
        {
            targetPos = movingLeft ? leftPos : rightPos;

            if (!currPos.Equals(targetPos))
            {
                transform.position = Vector3.MoveTowards(currPos, targetPos, SIGN_SPEED * Time.deltaTime);
            }
            else
            {
                movingLeft = !movingLeft;
            }
        }
        else if (!hasExited)
        {
            if (exitPos.Equals(Vector3.zero))
            {
                exitPos = new Vector3(currPos.x, EXIT_Y, currPos.z);
            }

            if (!currPos.Equals(exitPos))
            {
                transform.position = Vector3.MoveTowards(currPos, exitPos, SIGN_EXIT_SPEED * Time.deltaTime);
            }
            else
            {
                hasExited = true;
            }
        }

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
        transform.position = initPos;
    }
}
