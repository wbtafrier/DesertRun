using UnityEngine;

public class DesertObject : GameElement
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    public override void Restart() { }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (!GameController.IsRestarting() && !GameController.IsPlayerEnteringScene() && !GameStateMachine.IsGameOver() && transform.position.x >= -17.5)
        {
            transform.Translate(-5f * Time.deltaTime, 0f, 0f);
        }
    }
}
