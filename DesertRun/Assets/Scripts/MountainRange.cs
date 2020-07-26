using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountainRange : GameElement
{
    readonly Vector3 START_POS = new Vector3(17.47f, -0.25f, -1.5f);
    static readonly float INIT_VELOCITY = -0.1f;
    static float currVelocity = INIT_VELOCITY;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    public override void Restart()
    {
        currVelocity = INIT_VELOCITY;
    }

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
            transform.Translate(currVelocity * Time.deltaTime, 0, 0);
        }
    }

    public static void MultiplySpeed(float factor)
    {
        currVelocity *= factor;
    }
}
