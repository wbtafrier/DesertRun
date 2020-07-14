using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartButton : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    Color initColor;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        initColor = spriteRenderer.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameController.IsRestarting() && Input.GetMouseButtonDown(0))
        {
            Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(point.x, point.y), Vector2.zero, 0);
            Debug.Log(hit.collider);
            if (hit && hit.collider && hit.collider.CompareTag("RestartButton"))
            {
                hit.collider.GetComponent<SpriteRenderer>().color = Color.magenta;
                GameController.Restart();
            }
        }
    }

    public void ResetButton()
    {
        spriteRenderer.color = initColor;
    }
}