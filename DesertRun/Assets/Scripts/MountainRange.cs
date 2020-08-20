using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountainRange : MonoBehaviour
{
    readonly Vector3 START_POS = new Vector3(17.47f, -0.25f, -1.5f);
    static readonly float INIT_VELOCITY = -0.1f;
    static float currVelocity = INIT_VELOCITY;

    // Start is called before the first frame update
    public void Start()
    {
    }

    // Update is called once per frame
    public void Update()
    {
        if (GameController.IsRestarting() && currVelocity != INIT_VELOCITY)
        {
            currVelocity = INIT_VELOCITY;
        }

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
