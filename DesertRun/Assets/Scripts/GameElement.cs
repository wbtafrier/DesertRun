using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameElement : MonoBehaviour
{
    Vector3 initialPosition;

    // Start is called before the first frame update
    public virtual void Start()
    {
        initialPosition = transform.position;
    }

    public abstract void Restart();

    // Update is called once per frame
    public virtual void Update()
    {
        if (GameController.IsRestarting())
        {
            Restart();
            if (!transform.position.Equals(initialPosition))
            {
                transform.position = initialPosition;
            }
        }
    }

    public Vector3 GetInitialPosition()
    {
        return initialPosition;
    }
}
