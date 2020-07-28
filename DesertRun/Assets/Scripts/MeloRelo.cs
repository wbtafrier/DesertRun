using System.Collections.Generic;
using UnityEngine;

public class MeloRelo : GameElement
{
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidbodyComp;

    private static readonly float ENTER_VELOCITY_X = 1.5f;
    private static readonly float ENTER_FRAME_DURATION = 0.005f;
    private static readonly float JUMP_PRESS_MAX_TIME = 0.3f;
    private static readonly float JUMP_VELOCITY_MIN = 7f;
    private static readonly float GRAVITY_SCALE = 1.75f;

    private int currSpriteIndex = 0;
    private float spriteChangeTimer = 0f;
    private float secondsBtwnSpriteChange = 0.05f;

    private bool enteringFrame = false;
    private float enteringFrameTimer = 0;

    private bool jumping = false;
    //private bool firstJump = false;
    //private float jumpVelocity = 0.1f;
    private float jumpTimer = 0f;
    private float jumpPressTimer = 0f;
    //private float jumpDuration = 1f;
    //private float jumpHeight = 1.8f;
    //private float initHeight = 0f;

    private bool dead = false;

    static List<Sprite> spriteList = new List<Sprite>();
    static List<PolygonCollider2D> colliderList = new List<PolygonCollider2D>();

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbodyComp = GetComponent<Rigidbody2D>();

        if (spriteList.Count == 0)
        {
            spriteList = new List<Sprite>(Resources.LoadAll<Sprite>("Sprites/relo/"));
        }
        spriteRenderer.sprite = spriteList[currSpriteIndex];

        if (colliderList.Count == 0)
        {
            colliderList = new List<PolygonCollider2D>(GetComponents<PolygonCollider2D>());
        }

        foreach (PolygonCollider2D col in colliderList)
        {
            col.enabled = false;
        }
        enteringFrame = true;
    }

    public override void Restart()
    {
        currSpriteIndex = 0;
        foreach (PolygonCollider2D col in colliderList)
        {
            col.enabled = false;
        }
        spriteRenderer.sprite = spriteList[currSpriteIndex];
        colliderList[currSpriteIndex].enabled = true;
        transform.localRotation = Quaternion.identity;
        rigidbodyComp.constraints = RigidbodyConstraints2D.FreezeRotation;
        enteringFrame = true;
        dead = false;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        
        if (!GameController.IsRestarting())
        {
            if (!dead)
            {
                Animate();
            }

            if (enteringFrame)
            {
                enteringFrameTimer += Time.deltaTime;

                if (rigidbodyComp.gravityScale != GRAVITY_SCALE)
                {
                    rigidbodyComp.gravityScale = GRAVITY_SCALE;
                }

                if (enteringFrameTimer >= ENTER_FRAME_DURATION)
                {
                    Vector2 rigidPos = rigidbodyComp.position;
                    float x = rigidPos.x, y = rigidPos.y;

                    enteringFrameTimer = 0f;
                    if (x < -6.5)
                    {
                        rigidbodyComp.velocity = new Vector2(ENTER_VELOCITY_X, 0f);
                    }
                    else
                    {
                        if (x > -6.5)
                        {
                            rigidbodyComp.position = new Vector2(-6.5f, y);
                        }
                        rigidbodyComp.velocity = new Vector2(0f, 0f);
                        rigidbodyComp.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
                        enteringFrame = false;
                    }
                }
            }
            else
            {
                float axis = Input.GetAxis("Jump");

                if (axis > 0 && jumpPressTimer < JUMP_PRESS_MAX_TIME && !dead)
                {
                    float d = Time.deltaTime;
                    jumpPressTimer += d;
                    Jump(axis, d);
                }
                else if (axis <= 0 && jumping && jumpPressTimer > 0 && jumpPressTimer < JUMP_PRESS_MAX_TIME)
                {
                    jumpPressTimer = JUMP_PRESS_MAX_TIME;
                }
                else if (axis <= 0 && jumpPressTimer > 0 && !jumping && !dead)
                {
                    jumpPressTimer = 0f;
                }

                if (jumping)
                {
                    jumpTimer += Time.deltaTime;
                }
            }
        }

        UpdateCollider();
    }

    void Jump(float axis, float dTime)
    {
        //if (rigidbodyComp.velocity.y <= 0f)
        //{
        jumping = true;

        float multiplier = (1f + (1.5f * dTime)) * axis;
        rigidbodyComp.velocity = new Vector2(0f, JUMP_VELOCITY_MIN * multiplier);
        //}
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
            spriteRenderer.sprite = spriteList[currSpriteIndex];
        }
    }

    public void UpdateCollider()
    {
        GetLastCollider().enabled = false;
        GetCurrentCollider().enabled = true;
    }

    public bool IsEnteringFrame()
    {
        return enteringFrame;
    }

    public void Die()
    {
        dead = true;
        jumpTimer = 0f;
        jumping = false;
    }

    private PolygonCollider2D GetLastCollider()
    {
        if (currSpriteIndex == 0)
        {
            return colliderList[colliderList.Count - 1];
        }

        return colliderList[currSpriteIndex - 1];
    }

    private PolygonCollider2D GetCurrentCollider()
    {
        return colliderList[currSpriteIndex];
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.transform.tag.Equals("Surface") && !collision.transform.tag.Equals("Balloon"))
        {
            rigidbodyComp.constraints = RigidbodyConstraints2D.None;
            GameController.SetGameOver();
        }
        else if (collision.transform.tag.Equals("Surface") && jumpTimer > 0f)
        {
            jumpTimer = 0f;
            jumping = false;
        }
    }
}
