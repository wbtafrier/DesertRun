using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountainRange : GameElement
{
    readonly Vector3 START_POS = new Vector3(17.47f, -0.25f);
    readonly float VELOCITY = -0.1f;

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
        if (GameStateMachine.GetCurrentStateId() == GameStateMachine.GAME.GetId() && !GameController.IsRestarting())
        {
            if (transform.position.x <= -17.5)
            {
                transform.position = START_POS;
            }
            transform.Translate(VELOCITY * Time.deltaTime, 0, 0);
        }
    }
}
