using UnityEngine;

public class SignSwinger : MenuElement
{
    float maxX = 0f, minX = 0f;
    float initX = 0f;
    float currX = 0f;
    readonly float deltaX = 8f;
    readonly float maxShift = 5f;
    bool shiftLeft = true;

    public override void Start()
    {
        base.Start();

        initX = rectTransform.anchoredPosition.x;
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
        
        rectTransform.Translate((shiftLeft ? -actualDelta : actualDelta), 0, 0);
    }
}
