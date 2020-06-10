using UnityEngine;
using UnityEngine.SceneManagement;

public class CloudScroller : MenuElement
{
    int sceneIndex;
    Vector3 pos;
    public static Vector3 cloud1Pos, cloud2Pos;

    // Start is called before the first frame update
    public override void Start()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (sceneIndex == 0)
        {
            base.Start();
            pos = rectTransform.anchoredPosition;
        }
        else
        {
            pos = transform.position;
        }
    }

    // Update is called once per frame
    public override void Update()
    {
        if (sceneIndex == 0)
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
        else
        {
            if (pos.x <= -14.3833f)
            {
                transform.position = new Vector3(20.6167f, pos.y, pos.z);
            }

            transform.Translate(-0.7f * Time.deltaTime, 0, 0);
            pos = transform.position;
        }
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
