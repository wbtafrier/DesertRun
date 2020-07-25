using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCactus : MonoBehaviour
{
    private readonly float EXIT_SPEED = 4f;

    private Vector3 initPos = Vector3.zero;
    private Vector3 exitPos = Vector3.zero;

    private bool entering = false;
    private bool exiting = false;
    private bool hasExited = false;

    // Start is called before the first frame update
    void Start()
    {
        initPos = transform.position;
        exitPos = new Vector3(initPos.x, -8.35f, initPos.z);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currPos = transform.position;
        if (!entering && exiting && !hasExited)
        {
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
