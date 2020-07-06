using UnityEngine;

public class MenuElement : MonoBehaviour
{
    protected RectTransform rectTransform;
    protected bool exiting = false;
    protected bool stopped = false;
    private float currExitSpeed = 1f;
    private float exitTimer = 0f;
    private Vector3 exitVec;

    [SerializeField] float exitSpeed = default;
    [SerializeField] bool xExit = default;
    [SerializeField] bool yExit = default;

    public virtual void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public virtual void Update()
    {
        if (exiting)
        {
            if (exitTimer < 5f)
            {
                exitTimer += Time.deltaTime;
                rectTransform.Translate(exitVec);
            }
            else if (!stopped)
            {
                Stop();
                FindObjectOfType<MenuController>().ElementReady();
            }
        }
    }

    public void Stop()
    {
        exitTimer = 0;
        exiting = false;
        exitVec = new Vector3(0, 0, 0);
        stopped = true;
    }

    public virtual void BeginExit()
    {
        exiting = true;
        currExitSpeed = exitSpeed * Time.deltaTime;
        exitVec = new Vector3(xExit ? currExitSpeed : 0, yExit ? currExitSpeed : 0, 0);
    }
}
