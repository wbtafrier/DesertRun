using System.Collections.Generic;
using UnityEngine;

public class MeloRelo : GameElement
{
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidbodyComp;

    private int currSpriteIndex = 0;
    private float spriteChangeTimer = 0f;
    private float secondsBtwnSpriteChange = 0.05f;

    private bool enteringFrame = false;
    private float enteringFrameTimer = 0;
    private float enteringFrameDuration = 0.005f;

    private bool jumping = false;
    //private bool firstJump = false;
    //private float jumpVelocity = 0.1f;
    //private float jumpTimer = 0f;
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
                if (enteringFrameTimer >= enteringFrameDuration)
                {
                    enteringFrameTimer = 0f;
                    if (rigidbodyComp.position.x < -6.5)
                    {
                        rigidbodyComp.velocity = new Vector2(1.5f, 0f);
                    }
                    else
                    {
                        if (rigidbodyComp.position.x > -6.5)
                        {
                            rigidbodyComp.position = new Vector3(-6.5f, rigidbodyComp.position.y);
                        }
                        rigidbodyComp.velocity = new Vector2(0f, 0f);
                        rigidbodyComp.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
                        rigidbodyComp.gravityScale = 1f;
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
            }
        }

        UpdateCollider();
    }

    void Jump()
    {
        if (!jumping && !dead)
        {
            jumping = true;
            if (rigidbodyComp.position.y < 1f)
            {
                rigidbodyComp.velocity = new Vector2(0f, 8f);
            }
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
        if (!collision.transform.Equals(GameController.GetSand().transform))
        {
            rigidbodyComp.constraints = RigidbodyConstraints2D.None;
            GameController.SetGameOver();
        }
        else if (jumping)
        {
            jumping = false;
        }
    }
}
