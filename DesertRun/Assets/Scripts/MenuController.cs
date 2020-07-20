using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    int elementsOffScreen = 0;
    bool transitioning = false;
    [SerializeField] MenuElement[] elementList = default;

    public void Update()
    {
        if (transitioning && elementsOffScreen >= elementList.Length)
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
        foreach (MenuElement e in elementList)
        {
            e.BeginExit();
        }
    }

    public void ElementReady()
    {
        elementsOffScreen++;
    }

    public void Quit()
    {
        Debug.Log("QUIT is not implemented yet, please refresh page.");
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
