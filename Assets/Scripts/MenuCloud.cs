using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuCloud : MenuElement
{
    Vector3 pos;
    public static Vector3 cloud1Pos, cloud2Pos;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        pos = rectTransform.anchoredPosition;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (!exiting && !stopped)
        {
            if (pos.x <= -625)
            {
                rectTransform.anchoredPosition = new Vector2(975, pos.y);
            }

            rectTransform.Translate(-25 * Time.deltaTime, 0, 0);
        }
        pos = rectTransform.anchoredPosition;
    }

    public Vector3 GetPosition()
    {
        return pos;
    }

    public void SetPosition(Vector3 pos)
    {
        this.pos = pos;
    }
}
