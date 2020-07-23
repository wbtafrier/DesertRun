using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : GameElement
{
    Vector3 pos;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        pos = transform.position;
    }

    public override void Restart() { }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (!GameController.IsRestarting())
        {
            if (pos.x <= -14.3833f)
            {
                transform.position = new Vector3(20.6167f, pos.y, pos.z);
            }

            transform.Translate(-0.7f * Time.deltaTime, 0, 0);
            pos = transform.position;
        }
    }
}
