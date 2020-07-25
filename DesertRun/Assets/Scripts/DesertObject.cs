using UnityEngine;

public class DesertObject : GameElement
{
    Rigidbody2D rigidbodyComp;
    private bool activated = false;

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
        if (activated && !GameController.IsRestarting() && !GameController.IsPlayerEnteringScene() && !GameStateMachine.IsGameOver() && transform.position.x >= -17.5)
        {
            transform.Translate(DesertGenerator.desertObjectSpeed * Time.deltaTime, 0f, 0f);
            //rigidbodyComp.velocity = new Vector2(DesertGenerator.desertObjectSpeed, 0f);
        }
        else if (activated && (transform.position.x < -17.5 || GameController.IsRestarting() || GameStateMachine.IsGameOver()))
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
