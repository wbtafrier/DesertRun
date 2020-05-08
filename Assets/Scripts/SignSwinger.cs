using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignSwinger : MenuElement
{
    float maxX = 0f, minX = 0f;
    float initX = 0f;
    float currX = 0f;
    float deltaX = 0.05f;
    float maxShift = 5f;
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
            currX -= deltaX;
        }
        else
        {
            currX += deltaX;
        }
        
        transform.Translate((shiftLeft ? -deltaX : deltaX), 0, 0, Space.World);
    }
}
