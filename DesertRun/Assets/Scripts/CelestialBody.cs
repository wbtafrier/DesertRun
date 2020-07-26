using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialBody : GameElement
{
    SpriteRenderer spriteRenderer;
    Sprite sunSprite;
    Sprite fullMoonSprite;
    Vector3 initPos;
    readonly float INIT_SUNDOWN_VELOCITY_Y = -0.5f;
    static readonly float INIT_VELOCITY_Y = -0.15f;
    static readonly float INIT_VELOCITY_X = -0.2625f;
    float currVelocityY = INIT_VELOCITY_Y;
    float currVelocityX = INIT_VELOCITY_X;
    bool enterFrame = true;

    public override void Restart() { }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
        sunSprite = spriteRenderer.sprite;
        fullMoonSprite = Resources.Load<Sprite>("Sprites/fullMoon");
        initPos = transform.position;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (!GameController.IsRestarting())
        {
            if (enterFrame)
            {
                if (transform.position.y <= 3.75f)
                {
                    currVelocityY = INIT_VELOCITY_Y;
                    currVelocityX = INIT_VELOCITY_X;
                    enterFrame = false;
                }
                else
                {
                    transform.Translate(0, INIT_SUNDOWN_VELOCITY_Y * Time.deltaTime, 0);
                }
            }
            else if (transform.position.y <= -2.55)
            {
                DayNightHandler.LightSwitch();
            }
            else
            {
                transform.Translate(currVelocityX * Time.deltaTime, currVelocityY * Time.deltaTime, 0);
            }
        }
    }

    public void MultiplySpeed(float factor)
    {
        currVelocityX *= factor;
        currVelocityY *= factor;
    }

    public void ChangeToMoon()
    {
        enterFrame = true;
        spriteRenderer.sprite = fullMoonSprite;
        transform.localScale = new Vector3(1.5f, 1.5f);
        transform.position = new Vector3(initPos.x, initPos.y - 1.5f);
    }
    
    public void ChangeToSun()
    {
        enterFrame = true;
        spriteRenderer.sprite = sunSprite;
        transform.localScale = new Vector3(2.5f, 2.5f);
        transform.position = initPos;
    }
}
