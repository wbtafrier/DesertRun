using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour, IStateController
{
    [SerializeField] GameObject pausedTextProp = default;
    [SerializeField] GameObject pausedTextBgProp = default;
    [SerializeField] GameObject resumeButtonProp = default;
    [SerializeField] GameObject resumeButtonBgProp = default;
    [SerializeField] GameObject restartButtonProp = default;
    [SerializeField] GameObject restartButtonBgProp = default;
    [SerializeField] GameObject exitButtonProp = default;
    [SerializeField] GameObject exitButtonBgProp = default;

    static GameObject pausedTextObj;
    static GameObject pausedTextBgObj;
    static GameObject resumeButtonObj;
    static GameObject resumeButtonBgObj;
    //static GameObject restartButtonObj;
    //static GameObject restartButtonBgObj;
    static GameObject exitButtonObj;
    static GameObject exitButtonBgObj;

    // Start is called before the first frame update
    void Start()
    {
        pausedTextObj = pausedTextProp;
        pausedTextBgObj = pausedTextBgProp;
        resumeButtonObj = resumeButtonProp;
        resumeButtonBgObj = resumeButtonBgProp;
        //restartButtonObj = restartButtonProp;
        //restartButtonBgObj = restartButtonBgProp;
        exitButtonObj = exitButtonProp;
        exitButtonBgObj = exitButtonBgProp;
    }

    public void OnStateEnable()
    {
        Start();
        Debug.Log("GAME PAUSED");
        pausedTextBgObj.SetActive(true);
        pausedTextObj.SetActive(true);
        resumeButtonBgObj.SetActive(true);
        resumeButtonObj.SetActive(true);
        //restartButtonBgObj.SetActive(true);
        //restartButtonObj.SetActive(true);
        exitButtonBgObj.SetActive(true);
        exitButtonObj.SetActive(true);
    }

    public void OnStateDisable()
    {
        if (pausedTextObj.activeSelf)
        {
            pausedTextObj.SetActive(false);
        }

        if (pausedTextBgObj.activeSelf)
        {
            pausedTextBgObj.SetActive(false);
        }

        if (resumeButtonObj.activeSelf)
        {
            resumeButtonObj.SetActive(false);
        }

        if (resumeButtonBgObj.activeSelf)
        {
            resumeButtonBgObj.SetActive(false);
        }

        //if (restartButtonObj.activeSelf)
        //{
        //    restartButtonObj.SetActive(false);
        //}

        //if (restartButtonBgObj.activeSelf)
        //{
        //    restartButtonBgObj.SetActive(false);
        //}

        if (exitButtonObj.activeSelf)
        {
            exitButtonObj.SetActive(false);
        }

        if (exitButtonBgObj.activeSelf)
        {
            exitButtonBgObj.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UnpauseGame()
    {
        if (GameStateMachine.IsGamePaused())
        {
            GameStateMachine.UnpauseGame();
        }
    }

    public void RestartGame()
    {
        if (GameStateMachine.IsGamePaused())
        {
            GameStateMachine.RestartGame();
        }
    }

    public void ExitGame()
    {
        if (GameStateMachine.IsGamePaused())
        {
            GameStateMachine.ReturnToMainMenu();
        }
    }
}
