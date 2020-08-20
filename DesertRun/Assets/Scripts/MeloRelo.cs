using System.Collections.Generic;
using UnityEngine;

public class MeloRelo : GameElement
{
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidbodyComp;
    private AudioSource deathSfxSource;
    private AudioSource jumpSfxSource;
    private AudioSource runningSfxSource;
    private AudioSource powerUpSfxSource;

    [SerializeField] Color powerUpColorProp = default;

    private static readonly float ENTER_VELOCITY_X = 1.5f;
    private static readonly float ENTER_FRAME_DURATION = 0.005f;
    private static readonly float JUMP_PRESS_MAX_TIME = 0.3f;
    private static readonly float JUMP_VELOCITY_MIN = 7f;
    private static readonly float GRAVITY_SCALE = 1.75f;
    private static readonly float RUNNING_SFX_BREAK = 0.03f;
    private static readonly float INVINCIBILITY_MAX = 20f;

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
    private float runSfxTimer = 0f;
    //private float jumpDuration = 1f;
    //private float jumpHeight = 1.8f;
    //private float initHeight = 0f;

    private bool invincible = false;
    private float invincibleTimer = 0f;

    private bool dead = false;

    static List<Sprite> spriteList = new List<Sprite>();
    static List<PolygonCollider2D> colliderList = new List<PolygonCollider2D>();

    private static Color reloDefaultColor;
    private static Color reloPowerUpColor;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbodyComp = GetComponent<Rigidbody2D>();
        AudioSource[] sfxList = GetComponents<AudioSource>();

        foreach (AudioSource sfx in sfxList)
        {
            string sfxName = sfx.clip.name;
            if (sfxName.Equals("DEATH"))
            {
                deathSfxSource = sfx;
            }
            else if (sfxName.Equals("JUMP"))
            {
                jumpSfxSource = sfx;
            }
            else if (sfxName.Equals("RUNNING"))
            {
                runningSfxSource = sfx;
            }
            else if (sfxName.Equals("POWER UP"))
            {
                powerUpSfxSource = sfx;
            }
        }
        
        SetPlayerSfxVolume(GameStateMachine.GetSoundVolume());

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
        reloDefaultColor = spriteRenderer.color;
        reloPowerUpColor = powerUpColorProp;
    }

    public override void Restart()
    {
        SetPlayerSfxVolume(GameStateMachine.GetSoundVolume());
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

                if (!dead && invincible)
                {
                    float warningTime = (INVINCIBILITY_MAX - (INVINCIBILITY_MAX * 0.25f));
                    invincibleTimer += Time.deltaTime;
                    if (invincibleTimer >= warningTime && invincibleTimer <= INVINCIBILITY_MAX)
                    {
                        if (transform.localScale.x > 1f)
                        {
                            transform.localScale = new Vector3(1f, 1f, 1f);
                            jumpSfxSource.pitch = 1f;
                            runningSfxSource.pitch = 1f;
                            rigidbodyComp.gravityScale = GRAVITY_SCALE;
                        }
                        GameController.WarnPowerUpExpiring();
                    }
                    else if (invincibleTimer < warningTime)
                    {
                        if (transform.localScale.x != 1.5f)
                        {
                            if (transform.position.y < -0.52f)
                            {
                                float x = transform.position.x;
                                float z = transform.position.z;
                                transform.position = new Vector3(x, -0.52f, z);
                            }

                            transform.localScale = new Vector3(1.5f, 1.5f, 1f);
                            jumpSfxSource.pitch = 0.75f;
                            runningSfxSource.pitch = 0.75f;
                            rigidbodyComp.gravityScale = GRAVITY_SCALE * 2f;
                        }
                        spriteRenderer.color = reloPowerUpColor;
                    }

                    if (invincibleTimer > INVINCIBILITY_MAX)
                    {
                        KillInvincibility();
                    }
                }
            }
        }

        UpdateCollider();
    }

    public void FlashColor()
    {
        spriteRenderer.color = spriteRenderer.color.Equals(reloDefaultColor) ? reloPowerUpColor : reloDefaultColor;
    }

    public void KillInvincibility()
    {
        if (transform.localScale.x > 1f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
            jumpSfxSource.pitch = 1f;
            runningSfxSource.pitch = 1f;
            rigidbodyComp.gravityScale = GRAVITY_SCALE;
        }
        spriteRenderer.color = reloDefaultColor;
        invincible = false;
        invincibleTimer = 0f;
        GameController.ResetCoins(false);
    }

    void Jump(float axis, float dTime)
    {
        if (!jumping)
        {
            jumpSfxSource.Play();
            jumping = true;
        }

        float multiplier = (1f + (1.5f * dTime)) * axis;
        rigidbodyComp.velocity = new Vector2(0f, JUMP_VELOCITY_MIN * multiplier);
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

    public void PowerUpInvincible()
    {
        if (!dead)
        {
            powerUpSfxSource.Play();
            invincible = true;
        }
    }

    public bool IsInvincible()
    {
        return invincible;
    }

    public void Die()
    {
        deathSfxSource.Play();
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

    public void SetPlayerSfxVolume(float vol)
    {
        if (!deathSfxSource || !jumpSfxSource || !runningSfxSource || !powerUpSfxSource)
        {
            return;
        }

        deathSfxSource.volume = vol;
        jumpSfxSource.volume = vol;
        runningSfxSource.volume = vol * 0.5f;
        powerUpSfxSource.volume = vol;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.transform.tag.Equals("Surface") && !collision.transform.tag.Equals("Balloon"))
        {
            if (!invincible)
            {
                rigidbodyComp.constraints = RigidbodyConstraints2D.None;
                GameController.SetGameOver();
            }
            else
            {
                DesertObject desertObj = collision.gameObject.GetComponent<DesertObject>();
                if (desertObj)
                {
                    desertObj.Fly();
                }
            }
        }
        else if (collision.transform.tag.Equals("Surface"))
        {
            if (jumpTimer > 0f)
            {
                jumpTimer = 0f;
                jumping = false;
            }

            if (!dead)
            {
                if (runSfxTimer > RUNNING_SFX_BREAK)
                {
                    runSfxTimer = 0f;
                    runningSfxSource.Play();
                }
                runSfxTimer += Time.deltaTime;
            }
        }
    }
}
