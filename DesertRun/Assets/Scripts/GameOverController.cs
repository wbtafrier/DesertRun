using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour, IStateController
{
    [SerializeField] GameObject gameOverProp = default;
    [SerializeField] GameObject gameOverBgProp = default;
    [SerializeField] GameObject restartButtonProp = default;
    [SerializeField] GameObject restartButtonBgProp = default;
    [SerializeField] GameObject exitButtonProp = default;
    [SerializeField] GameObject exitButtonBgProp = default;

    static GameObject gameOverObj;
    static GameObject gameOverBgObj;
    static GameObject restartButtonObj;
    static GameObject restartButtonBgObj;
    static GameObject exitButtonObj;
    static GameObject exitButtonBgObj;

    // Start is called before the first frame update
    void Start()
    {
        gameOverObj = gameOverProp;
        gameOverBgObj = gameOverBgProp;
        restartButtonObj = restartButtonProp;
        restartButtonBgObj = restartButtonBgProp;
        exitButtonObj = exitButtonProp;
        exitButtonBgObj = exitButtonBgProp;
    }

    public void OnStateEnable()
    {
        Start();
        Debug.Log("GAME OVER");
        gameOverBgObj.SetActive(true);
        gameOverObj.SetActive(true);
        restartButtonBgObj.SetActive(true);
        restartButtonObj.SetActive(true);
        exitButtonBgObj.SetActive(true);
        exitButtonObj.SetActive(true);
    }

    public void OnStateDisable()
    {
        if (gameOverObj.activeSelf)
        {
            gameOverObj.SetActive(false);
        }

        if (gameOverBgObj.activeSelf)
        {
            gameOverBgObj.SetActive(false);
        }

        if (restartButtonObj.activeSelf)
        {
            restartButtonObj.SetActive(false);
        }

        if (restartButtonBgObj.activeSelf)
        {
            restartButtonBgObj.SetActive(false);
        }

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

    public void RestartGame()
    {
        if (GameStateMachine.IsGameOver())
        {
            GameStateMachine.RestartGame();
        }
    }

    public void ExitGame()
    {
        if (GameStateMachine.IsGameOver())
        {
            GameStateMachine.ReturnToMainMenu();
        }
    }
}
