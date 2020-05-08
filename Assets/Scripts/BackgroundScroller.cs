using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackgroundScroller : MonoBehaviour
{
    private static int currIndex = 0;
    static List<Sprite> spriteList = new List<Sprite>();
    private float timer = 0f;
    private int sceneIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (spriteList.Count == 0)
        {
            spriteList = new List<Sprite>(Resources.LoadAll<Sprite>("Sprites/bg/"));
        }
        sceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (sceneIndex == 0)
        {
            GetComponent<Image>().sprite = spriteList[currIndex];
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = spriteList[currIndex];
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= (sceneIndex == 0 ? 2f : 1f))
        {
            timer = 0f;
            if (currIndex >= spriteList.Count - 1)
            {
                currIndex = 0;
            }
            else
            {
                currIndex++;
            }
            if (sceneIndex == 0)
            {
                GetComponent<Image>().sprite = spriteList[currIndex];
            }
            else
            {
                GetComponent<SpriteRenderer>().sprite = spriteList[currIndex];
            }
        }
    }
}
