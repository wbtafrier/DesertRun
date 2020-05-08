using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeloRelo : MonoBehaviour
{
    private int currSpriteIndex = 0;
    private float spriteChangeTimer = 0f;
    private float secondsBtwnSpriteChange = 0.05f;

    private bool enteringFrame = false;
    private float enteringFrameTimer = 0;
    private float enteringFrameDuration = 0.005f;

    private bool jumping = false;
    private float jumpVelocity = 0.1f;
    private float jumpTimer = 0f;
    private float jumpDuration = 0.5f;
    private float jumpHeight = 1.8f;
    private float initHeight = 0f;

    static List<Sprite> spriteList = new List<Sprite>();

    // Start is called before the first frame update
    void Start()
    {
        if (spriteList.Count == 0)
        {
            spriteList = new List<Sprite>(Resources.LoadAll<Sprite>("Sprites/relo/"));
        }
        Debug.Log(spriteList.Count);
        GetComponent<SpriteRenderer>().sprite = spriteList[currSpriteIndex];
        enteringFrame = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!jumping)
        {
            Animate();
        }

        if (enteringFrame)
        {
            enteringFrameTimer += Time.deltaTime;
            if (enteringFrameTimer >= enteringFrameDuration)
            {
                enteringFrameTimer = 0f;
                if (transform.position.x < -6.5)
                {
                    transform.Translate(0.01f, 0, 0);
                }
                else
                {
                    if (transform.position.x > -6.5)
                    {
                        transform.position = new Vector3(-6.5f, transform.position.y);
                    }
                    enteringFrame = false;
                }
            }
        }
        else
        {
            if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }

            if (jumping)
            {
                float currHeight = transform.position.y;
                if (currHeight >= jumpHeight && jumpVelocity > 0f)
                {
                    jumpTimer += Time.deltaTime;
                    if (currHeight > jumpHeight)
                    {
                        transform.position = new Vector3(transform.position.x, jumpHeight);
                    }

                    if (jumpTimer >= jumpDuration)
                    {
                        jumpTimer = 0f;
                        jumpVelocity = -jumpVelocity;
                    }
                }
                else if (currHeight <= initHeight && jumpVelocity < 0f)
                {
                    jumpVelocity = -jumpVelocity;
                    if (currHeight < initHeight)
                    {
                        transform.position = new Vector3(transform.position.x, initHeight);
                    }
                    jumping = false;
                }
                else
                {
                    transform.Translate(0, jumpVelocity, 0);
                }
            }
        }
    }

    void Jump()
    {
        if (!jumping)
        {
            initHeight = transform.position.y;
            jumping = true;
            GetComponent<SpriteRenderer>().sprite = spriteList[0];
        }
    }

    void Animate()
    {
        spriteChangeTimer += Time.deltaTime;
        if (spriteChangeTimer >= secondsBtwnSpriteChange)
        {
            spriteChangeTimer = 0f;
            if (currSpriteIndex >= spriteList.Count - 1)
            {
                currSpriteIndex = 0;
            }
            else
            {
                currSpriteIndex++;
            }
            GetComponent<SpriteRenderer>().sprite = spriteList[currSpriteIndex];
        }
    }
}
