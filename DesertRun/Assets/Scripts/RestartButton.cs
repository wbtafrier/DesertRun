﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartButton : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Color initColor;

    float clickTimer = 0f;
    readonly float clickDuration = 0.1f;
    bool clicked = false;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        initColor = spriteRenderer.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (clicked)
        {
            if (clickTimer < clickDuration)
            {
                clickTimer += Time.deltaTime;
            }
            else
            {
                clickTimer = 0f;
                GameController.Restart();
                clicked = false;
            }
        }

        if (!GameController.IsRestarting() && !clicked && Input.GetMouseButtonDown(0))
        {
            Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(point.x, point.y), Vector2.zero, 0);
            if (hit && hit.collider && hit.collider.CompareTag("RestartButton"))
            {
                hit.collider.GetComponent<SpriteRenderer>().color = Color.magenta;
                clicked = true;
            }
        }
    }

    public void ResetButton()
    {
        spriteRenderer.color = initColor;
        clicked = false;
        clickTimer = 0f;
    }
}