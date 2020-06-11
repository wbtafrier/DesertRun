using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountainRange : MonoBehaviour
{
    readonly Vector3 START_POS = new Vector3(-17.5f, -0.25f);
    readonly float VELOCITY = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameController.IsGameOver())
        {
            if (transform.position.x >= 17.5)
            {
                transform.position = START_POS;
            }
            transform.Translate(VELOCITY * Time.deltaTime, 0, 0);
        }
    }
}
