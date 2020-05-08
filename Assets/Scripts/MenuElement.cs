using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuElement : MonoBehaviour
{
    protected bool exiting = false;
    protected bool stopped = false;
    private float exitSpeed = 1f;
    private float exitTimer = 0f;
    private Vector3 exitVec;

    public virtual void Update()
    {
        if (exiting)
        {
            if (exitTimer < 3f)
            {
                exitTimer += Time.deltaTime;
                transform.Translate(exitVec);
            }
            else if (!stopped)
            {
                Stop();
                FindObjectOfType<MenuController>().ElementReady(this);
            }
        }
    }

    public void Stop()
    {
        exitTimer = 0;
        exiting = false;
        exitVec = new Vector3(0, 0, 0);
        stopped = true;
    }

    public virtual void BeginExit(float speed, bool x, bool y, bool z)
    {
        exiting = true;
        this.exitSpeed = speed;
        exitVec = new Vector3(x ? speed : 0, y ? speed : 0, z ? speed : 0);
    }
}
