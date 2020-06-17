using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialBody : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Sprite sunSprite;
    Sprite fullMoonSprite;
    Vector3 initPos;
    readonly float INIT_SUNDOWN_VELOCITY_Y = -0.5f;
    readonly float VELOCITY_Y = -0.1f;
    readonly float VELOCITY_X = -0.175f;
    bool enterFrame = true;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        sunSprite = spriteRenderer.sprite;
        fullMoonSprite = Resources.Load<Sprite>("Sprites/fullMoon");
        initPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (enterFrame)
        {
            if (transform.position.y <= 3.75f)
            {
                enterFrame = false;
            }
            else
            {
                transform.Translate(0, INIT_SUNDOWN_VELOCITY_Y * Time.deltaTime, 0);
            }
        }
        else if (transform.position.y <= -2.55)
        {
            enterFrame = true;
            GameController.LightSwitch();
        }
        else
        {
            transform.Translate(VELOCITY_X * Time.deltaTime, VELOCITY_Y * Time.deltaTime, 0);
        }
    }

    public void ChangeToMoon()
    {
        spriteRenderer.sprite = fullMoonSprite;
        transform.localScale = new Vector3(1.5f, 1.5f);
        transform.position = new Vector3(initPos.x, initPos.y - 1.5f);
    }
    
    public void ChangeToSun()
    {
        spriteRenderer.sprite = sunSprite;
        transform.localScale = new Vector3(2.5f, 2.5f);
        transform.position = initPos;
    }
}
