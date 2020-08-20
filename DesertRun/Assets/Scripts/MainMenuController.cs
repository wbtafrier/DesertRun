using System.Runtime.InteropServices;
using TMPro;
using UnityEditor;
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
    [SerializeField] GameObject versionTextProp = default;
    [SerializeField] GameObject versionTextBgProp = default;
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
    static GameObject versionTextObj;
    static GameObject versionTextBgObj;
    static GameObject closeButtonObj;
    static GameObject closeButtonBgObj;

    static Button startButton;
    static Button settingsButton;
    static Button closeButton;
    static TextMeshProUGUI versionText;
    static TextMeshProUGUI versionTextBg;
    static MainMenuSign sign;
    static MainMenuAlien alien1;
    static MainMenuAlien alien2;
    static MenuCactus cactus;

    private bool transitioning = false;

#if !UNITY_EDITOR && UNITY_WEBGL
    [DllImport("__Internal")]
    private static extern void QuitAnimation();
#endif

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
        versionTextObj = versionTextProp;
        versionTextBgObj = versionTextBgProp;
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
        versionTextObj.SetActive(true);
        versionTextBgObj.SetActive(true);
        closeButtonObj.SetActive(true);
        closeButtonBgObj.SetActive(true);

        startButton = startButtonObj.GetComponent<Button>();
        settingsButton = settingsButtonObj.GetComponent<Button>();
        closeButton = closeButtonObj.GetComponent<Button>();
        versionText = versionTextObj.GetComponent<TextMeshProUGUI>();
        versionTextBg = versionTextBgObj.GetComponent<TextMeshProUGUI>();

        string v = "V" + Application.version + " BETA";
        versionText.text = v;
        versionTextBg.text = v;
        sign = signObj.GetComponent<MainMenuSign>();
        alien1 = alien1Obj.GetComponent<MainMenuAlien>();
        alien2 = alien2Obj.GetComponent<MainMenuAlien>();
        cactus = cactusObj.GetComponent<MenuCactus>();

#if UNITY_EDITOR
        GameStateMachine.EnableSounds();
#endif
    }

    public void OnStateEnable()
    {
        Start();
        transitioning = false;
        startButton.interactable = false;
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
        versionTextObj.SetActive(false);
        versionTextBgObj.SetActive(false);
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
        else if (sign.HasEntered() && alien1.HasEntered() && alien2.HasEntered() && cactus.HasEntered())
        {
            startButton.interactable = true;
            closeButton.interactable = true;
            settingsButton.interactable = true;
        }
    }

    public void StartGame()
    {
        transitioning = true;
        closeButton.interactable = false;
        settingsButton.interactable = false;
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

        GameStateMachine.DisableSounds();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif

#if !UNITY_EDITOR && UNITY_WEBGL
        QuitAnimation();
#endif

    }
}
