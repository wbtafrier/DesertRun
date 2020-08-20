using System.Collections.Generic;
using UnityEngine;

public class DesertObject : GameElement
{
    public GameObject particleKick;
    public static readonly float STOP_X_POS = -17.5f;

    Rigidbody2D rigidbodyComp;
    protected bool activated = false;
    protected bool flying = false;
    private bool particleKickPlayed = false;

    private float flyingPlayerCollisionTimer = 0f;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        particleKick = Resources.Load<GameObject>("Prefabs/PowerUpCollisionSparkle");
        rigidbodyComp = GetComponent<Rigidbody2D>();
    }

    public override void Restart()
    {
        if (flying)
        {
            DeactivateAfterFlying();
        }
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (activated && !flying && !GameController.IsRestarting() && !GameController.IsPlayerEnteringScene()
            && !GameStateMachine.IsGameOver() && transform.position.x >= STOP_X_POS)
        {
            transform.Translate(DesertGenerator.desertObjectSpeed * Time.deltaTime, 0f, 0f);
            //rigidbodyComp.velocity = new Vector2(DesertGenerator.desertObjectSpeed, 0f);
        }
        else if (activated && (transform.position.x < STOP_X_POS || GameController.IsRestarting() || GameStateMachine.IsGameOver()))
        {
            Deactivate();
        }

        if (activated && flying && !GameController.IsReloInvincible())
        {
            DeactivateAfterFlying();
        }
    }

    public void Fly()
    {
        if (activated && !flying && !GameController.IsRestarting() && !GameController.IsPlayerEnteringScene()
            && !GameStateMachine.IsGameOver() && transform.position.x >= STOP_X_POS)
        {
            if (!particleKickPlayed)
            {
                particleKickPlayed = true;
                Instantiate(particleKick, transform.position, Quaternion.identity);
            }

            flying = true;
            rigidbodyComp.gravityScale = 1f;
            rigidbodyComp.bodyType = RigidbodyType2D.Dynamic;
            rigidbodyComp.velocity = new Vector2(10f, 5f);
        }
    }

    private void DeactivateAfterFlying()
    {
        flying = false;
        rigidbodyComp.gravityScale = 0f;
        rigidbodyComp.bodyType = RigidbodyType2D.Kinematic;
        rigidbodyComp.velocity = Vector2.zero;
        rigidbodyComp.angularVelocity = 0f;
        transform.localRotation = Quaternion.identity;
        transform.position = GetInitialPosition();
        particleKickPlayed = false;
        Deactivate();
    }

    public void Activate()
    {
        activated = true;
    }

    public void Deactivate()
    {
        activated = false;
        //rigidbodyComp.velocity = new Vector2(0f, 0f);
        DesertGenerator.ReturnToStack(this);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (activated && flying)
        {
            if (collision.transform.tag.Equals("Player"))
            {   
                flyingPlayerCollisionTimer += Time.deltaTime;

                if (flyingPlayerCollisionTimer >= 0.15f)
                {
                    flyingPlayerCollisionTimer = 0f;
                    DeactivateAfterFlying();
                    return;
                }
            }
            rigidbodyComp.velocity = new Vector2(10f, 5f);
        }
    }
}
