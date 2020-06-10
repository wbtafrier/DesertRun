using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    List<MenuElement> elementsOffScreen = new List<MenuElement>();
    bool transitioning = false;
    [SerializeField] Image sign = default;
    [SerializeField] Image alien1 = default;
    [SerializeField] Image alien2 = default;
    [SerializeField] Image cactus = default;
    [SerializeField] Image clouds1 = default;
    [SerializeField] Image clouds2 = default;
    [SerializeField] Image sun = default;
    [SerializeField] Button button = default;

    public void Update()
    {
        if (transitioning && elementsOffScreen.Count == 7)
        {
            Debug.Log("LOADING GAME...");
            //CloudScroller.cloud1Pos = clouds1.GetComponent<CloudScroller>().GetPosition();
            //CloudScroller.cloud2Pos = clouds2.GetComponent<CloudScroller>().GetPosition();
            transitioning = false;
            SceneManager.LoadScene(1);
        }
    }

    public void SendElementsAway()
    {
        //every second is 100pts
        //every balloon is 150pts
        //sand particles
        transitioning = true;
        sign.GetComponent<MenuElement>().BeginExit(80f, false, true, false);
        alien1.GetComponent<MenuElement>().BeginExit(200f, false, true, false);
        alien2.GetComponent<MenuElement>().BeginExit(200f, true, false, false);
        cactus.GetComponent<MenuElement>().BeginExit(-100f, false, true, false);
        clouds1.GetComponent<MenuElement>().BeginExit(-600f, true, false, false);
        clouds2.GetComponent<MenuElement>().BeginExit(-600f, true, false, false);
        sun.GetComponent<MenuElement>().BeginExit(50f, false, true, false);
        button.GetComponent<MenuElement>().BeginExit(-50f, true, true, false);
    }

    public void ElementReady(MenuElement e)
    {
        elementsOffScreen.Add(e);
    }
}
