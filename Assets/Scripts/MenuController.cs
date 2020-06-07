using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    List<MenuElement> elementsOffScreen = new List<MenuElement>();
    bool transitioning = false;
    [SerializeField] Image sign;
    [SerializeField] Image alien1;
    [SerializeField] Image alien2;
    [SerializeField] Image cactus;

    public void Update()
    {
        if (transitioning && elementsOffScreen.Count == 4)
        {
            Debug.Log("LOADING GAME...");
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
        sign.GetComponent<MenuElement>().BeginExit(150f, false, true, false);
        alien1.GetComponent<MenuElement>().BeginExit(225f, false, true, false);
        alien2.GetComponent<MenuElement>().BeginExit(225f, true, false, false);
        cactus.GetComponent<MenuElement>().BeginExit(-185f, false, true, false);
    }

    public void ElementReady(MenuElement e)
    {
        elementsOffScreen.Add(e);
    }
}
