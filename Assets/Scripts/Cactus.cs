using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cactus : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameController.IsPlayerEnteringScene() && !GameController.IsGameOver() && transform.position.x >= -17.5)
        {
            transform.Translate(-5f * Time.deltaTime, 0f, 0f);
        }
    }
}
