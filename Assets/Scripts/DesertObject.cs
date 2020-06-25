using UnityEngine;

public class DesertObject : MonoBehaviour
{
    // Start is called before the first frame update
    public virtual void Start()
    {
        
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (!GameController.IsPlayerEnteringScene() && !GameController.IsGameOver() && transform.position.x >= -17.5)
        {
            transform.Translate(-5f * Time.deltaTime, 0f, 0f);
        }
    }
}
