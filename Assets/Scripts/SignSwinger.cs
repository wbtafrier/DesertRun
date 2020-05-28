using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignSwinger : MenuElement
{
    float maxX = 0f, minX = 0f;
    float initX = 0f;
    float currX = 0f;
    readonly float deltaX = 8f;
    readonly float maxShift = 5f;
    bool shiftLeft = true;

    void Start()
    {
        initX = gameObject.transform.position.x;
        currX = initX;
        maxX = initX + maxShift;
        minX = initX - maxShift;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        float actualDelta = deltaX * Time.deltaTime;

        if (exiting || stopped)
        {
            return;
        }

        if (currX <= minX)
        {
            shiftLeft = false;
        }
        else if (currX >= maxX)
        {
            shiftLeft = true;
        }

        if (shiftLeft)
        {
            currX -= actualDelta;
        }
        else
        {
            currX += actualDelta;
        }
        
        transform.Translate((shiftLeft ? -actualDelta : actualDelta), 0, 0, Space.World);
    }
}
