using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : DesertObject
{
    private int currSpriteIndex = 0;
    private float spriteChangeTimer = 0f;
    private float secondsBtwnSpriteChange = 0.2f;

    private SpriteRenderer spriteRenderer;

    static List<Sprite> spriteList = new List<Sprite>();
    static List<PolygonCollider2D> colliderList = new List<PolygonCollider2D>();

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteList.Count == 0)
        {
            spriteList = new List<Sprite>(Resources.LoadAll<Sprite>("Sprites/snake/"));
        }
        spriteRenderer.sprite = spriteList[currSpriteIndex];

        if (colliderList.Count == 0)
        {
            colliderList = new List<PolygonCollider2D>(GetComponents<PolygonCollider2D>());
        }

        foreach (PolygonCollider2D col in colliderList)
        {
            col.enabled = false;
        }
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        Animate();
        UpdateCollider();
    }

    public void UpdateCollider()
    {
        GetLastCollider().enabled = false;
        GetCurrentCollider().enabled = true;
    }

    private PolygonCollider2D GetLastCollider()
    {
        if (currSpriteIndex == 0)
        {
            return colliderList[colliderList.Count - 1];
        }

        return colliderList[currSpriteIndex - 1];
    }

    private PolygonCollider2D GetCurrentCollider()
    {
        return colliderList[currSpriteIndex];
    }

    void Animate()
    {
        spriteChangeTimer += Time.deltaTime;
        if (spriteChangeTimer >= secondsBtwnSpriteChange)
        {
            spriteChangeTimer = 0f;
            if (currSpriteIndex >= spriteList.Count - 1)
            {
                currSpriteIndex = 0;
            }
            else
            {
                currSpriteIndex++;
            }
            spriteRenderer.sprite = spriteList[currSpriteIndex];
        }
    }
}
