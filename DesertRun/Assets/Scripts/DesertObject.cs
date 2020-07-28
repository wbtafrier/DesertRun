using UnityEngine;

public class DesertObject : GameElement
{
    public static readonly float STOP_X_POS = -17.5f;

    Rigidbody2D rigidbodyComp;
    protected bool activated = false;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        rigidbodyComp = GetComponent<Rigidbody2D>();
    }

    public override void Restart() { }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (activated && !GameController.IsRestarting() && !GameController.IsPlayerEnteringScene() && !GameStateMachine.IsGameOver() && transform.position.x >= STOP_X_POS)
        {
            transform.Translate(DesertGenerator.desertObjectSpeed * Time.deltaTime, 0f, 0f);
            //rigidbodyComp.velocity = new Vector2(DesertGenerator.desertObjectSpeed, 0f);
        }
        else if (activated && (transform.position.x < STOP_X_POS || GameController.IsRestarting() || GameStateMachine.IsGameOver()))
        {
            Deactivate();
        }
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
}
