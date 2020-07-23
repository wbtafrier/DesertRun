using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour, IStateController
{
    [SerializeField] GameObject signProp = default;
    [SerializeField] GameObject alien1Prop = default;
    [SerializeField] GameObject alien2Prop = default;
    [SerializeField] GameObject cactusProp = default;
    [SerializeField] GameObject startButtonProp = default;
    [SerializeField] GameObject startButtonBgProp = default;
    [SerializeField] GameObject settingsButtonProp = default;
    [SerializeField] GameObject settingsButtonBgProp = default;
    [SerializeField] GameObject closeButtonProp = default;
    [SerializeField] GameObject closeButtonBgProp = default;

    static GameObject signObj;
    static GameObject alien1Obj;
    static GameObject alien2Obj;
    static GameObject cactusObj;
    static GameObject startButtonObj;
    static GameObject startButtonBgObj;
    static GameObject settingsButtonObj;
    static GameObject settingsButtonBgObj;
    static GameObject closeButtonObj;
    static GameObject closeButtonBgObj;

    static MainMenuSign sign;
    static MainMenuAlien alien1;
    static MainMenuAlien alien2;
    static MenuCactus cactus;

    private bool transitioning = false;

    // Start is called before the first frame update
    void Start()
    {
        signObj = signProp;
        alien1Obj = alien1Prop;
        alien2Obj = alien2Prop;
        cactusObj = cactusProp;
        startButtonObj = startButtonProp;
        startButtonBgObj = startButtonBgProp;
        settingsButtonObj = settingsButtonProp;
        settingsButtonBgObj = settingsButtonBgProp;
        closeButtonObj = closeButtonProp;
        closeButtonBgObj = closeButtonBgProp;

        signObj.SetActive(true);
        alien1Obj.SetActive(true);
        alien2Obj.SetActive(true);
        cactusObj.SetActive(true);
        startButtonObj.SetActive(true);
        startButtonBgObj.SetActive(true);
        settingsButtonObj.SetActive(true);
        settingsButtonBgObj.SetActive(true);
        closeButtonObj.SetActive(true);
        closeButtonBgObj.SetActive(true);

        sign = signObj.GetComponent<MainMenuSign>();
        alien1 = alien1Obj.GetComponent<MainMenuAlien>();
        alien2 = alien2Obj.GetComponent<MainMenuAlien>();
        cactus = cactusObj.GetComponent<MenuCactus>();
    }

    public void OnStateEnable()
    {
        Start();
        sign.ResetComp();
        alien1.ResetComp();
        alien2.ResetComp();
        cactus.ResetComp();
        DayNightHandler.SetDay();
    }

    public void OnStateDisable()
    {
        signObj.SetActive(false);
        alien1Obj.SetActive(false);
        alien2Obj.SetActive(false);
        cactusObj.SetActive(false);
        startButtonObj.SetActive(false);
        startButtonBgObj.SetActive(false);
        settingsButtonObj.SetActive(false);
        settingsButtonBgObj.SetActive(false);
        closeButtonObj.SetActive(false);
        closeButtonBgObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (transitioning)
        {
            if (sign.HasExited() && alien1.HasExited() && alien2.HasExited() && cactus.HasExited())
            {
                GameStateMachine.StartGame();
            }
        }
    }

    public void StartGame()
    {
        transitioning = true;
        sign.Exit();
        alien1.Exit();
        alien2.Exit();
        cactus.Exit();
    }

    public void OpenSettingsMenu()
    {
        GameStateMachine.OpenSettingsMenu();
    }

    public void Quit()
    {
        Debug.Log("QUIT is not implemented yet, please refresh page.");
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
