using System.Collections.Generic;
using UnityEngine;

public class MeloRelo : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private PolygonCollider2D polygonCollider;
    private Rigidbody2D rigidbody;

    private int currSpriteIndex = 0;
    private float spriteChangeTimer = 0f;
    private float secondsBtwnSpriteChange = 0.05f;

    private bool enteringFrame = false;
    private float enteringFrameTimer = 0;
    private float enteringFrameDuration = 0.005f;

    private bool jumping = false;
    private bool firstJump = false;
    private float jumpVelocity = 0.1f;
    private float jumpTimer = 0f;
    private float jumpDuration = 1f;
    private float jumpHeight = 1.8f;
    private float initHeight = 0f;

    private bool dead = false;

    static List<Sprite> spriteList = new List<Sprite>();
    static List<PolygonCollider2D> colliderList = new List<PolygonCollider2D>();

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        polygonCollider = GetComponent<PolygonCollider2D>();
        rigidbody = GetComponent<Rigidbody2D>();

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

    // Update is called once per frame
    void Update()
    {
        if (/*!jumping && */!dead)
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
                    rigidbody.gravityScale = 1f;
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

            //if (jumping && !dead)
            //{

            //    float currHeight = transform.position.y;

                //if (firstJump)
                //{
                //    for (int i = 0; i < colliderList.Count; i++)
                //    {
                //        PolygonCollider2D col = colliderList[i];
                //        if (i != currSpriteIndex && col.enabled)
                //        {
                //            col.enabled = false;
                //        }
                //    }
                //    firstJump = false;
                //}

                //if (currHeight >= jumpHeight && jumpVelocity > 0f)
                //{
                //    jumpTimer += Time.deltaTime;
                //    if (currHeight > jumpHeight)
                //    {
                //        transform.position = new Vector3(transform.position.x, jumpHeight);
                //    }

                //    if (jumpTimer >= jumpDuration)
                //    {
                //        jumpTimer = 0f;
                //        jumpVelocity = -jumpVelocity;
                //    }
                //}
                //else if (currHeight <= -1.1 && jumpVelocity < 0f)
                //{
                //    jumpVelocity = -jumpVelocity;
                //    if (currHeight < initHeight)
                //    {
                //        transform.position = new Vector3(transform.position.x, initHeight);
                //    }

                //    jumping = false;
                //}
                //else
                //{
                //    transform.Translate(0, jumpVelocity, 0);
                //}
            //}
        }

        UpdateCollider();
    }

    void Jump()
    {
        if (!jumping && !dead)
        {
            //initHeight = transform.position.y;
            jumping = true;
            //firstJump = true;
            //colliderList[currSpriteIndex].enabled = false;
            //currSpriteIndex = 0;
            //spriteRenderer.sprite = spriteList[currSpriteIndex];
            //rigidbody.AddForce(new Vector2(0f, 400f));
            if (rigidbody.velocity.y < 1f)
            {
                rigidbody.velocity = new Vector2(0f, 8f);
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
            rigidbody.constraints = RigidbodyConstraints2D.None;
            GameController.SetGameOver();
        }
        else if (jumping)
        {
            jumping = false;
        }
    }
}
