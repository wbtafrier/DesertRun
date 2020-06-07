using UnityEngine;

public class CloudScroller : MonoBehaviour
{
    RectTransform rect;

    // Start is called before the first frame update
    void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(rect.anchoredPosition);
        if (rect.anchoredPosition.x <= -625)
        {
            rect.anchoredPosition = new Vector2(975, rect.anchoredPosition.y);
        }

        rect.Translate(-25 * Time.deltaTime, 0, 0);
    }
}
