using UnityEngine;

public class GameStateMachine : MonoBehaviour
{
    [SerializeField] GameObject mainMenuControllerProp = default;
    [SerializeField] GameObject settingsControllerProp = default;
    [SerializeField] GameObject gameControllerProp = default;
    [SerializeField] GameObject pauseControllerProp = default;
    [SerializeField] GameObject gameOverControllerProp = default;

    public static readonly GameState MAIN_MENU = new GameState(0, "main_menu");
    public static readonly GameState SETTINGS = new GameState(1, "settings");
    public static readonly GameState GAME = new GameState(2, "game");
    public static readonly GameState PAUSE = new GameState(3, "pause");
    public static readonly GameState GAME_OVER = new GameState(4, "game_over");

    static GameObject mainMenuController;
    static GameObject settingsController;
    static GameObject gameController;
    static GameObject pauseController;
    static GameObject gameOverController;

    private static GameState currentState = MAIN_MENU;

    public class GameState
    {
        private readonly int stateID = -1;
        private readonly string stateName = null;

        public GameState(int id, string name)
        {
            stateID = id;
            name = stateName;
        }

        public int GetId()
        {
            return stateID;
        }

        public string GetName()
        {
            return stateName;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        mainMenuController = mainMenuControllerProp;
        settingsController = settingsControllerProp;
        gameController = gameControllerProp;
        pauseController = pauseControllerProp;
        gameOverController = gameOverControllerProp;

        if (!mainMenuController.activeSelf)
        {
            mainMenuController.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void StartGame()
    {
        if (currentState.GetId() == MAIN_MENU.GetId())
        {
            currentState = GAME;
            mainMenuController.GetComponent<MainMenuController>().OnStateDisable();
            mainMenuController.SetActive(false);
            gameController.SetActive(true);
            gameController.GetComponent<GameController>().OnStateEnable();
        }
        else
        {
            Debug.Log("WARNING: There was an attempt to start the game from outside of the main menu.");
        }
    }

    public static void OpenSettingsMenu()
    {
        if (currentState.GetId() == MAIN_MENU.GetId())
        {
            Debug.Log("DEBUG: Settings Menu has not yet been implemented yet.");
            //currentState = SETTINGS;
            //mainMenuController.GetComponent<MainMenuController>().OnStateDisable();
            //mainMenuController.SetActive(false);
            //settingsController.SetActive(true);
            //settingsController.GetComponent<SettingsController>().OnStateEnable();
        }
        else
        {
            Debug.Log("WARNING: There was an attempt to open the Settings Menu from outside of the main menu.");
        }
    }

    public static void PauseGame()
    {
        if (currentState.GetId() == GAME.GetId())
        {
            Time.timeScale = 0f;
            currentState = PAUSE;
            pauseController.SetActive(true);
            pauseController.GetComponent<PauseController>().OnStateEnable();
        }
        else
        {
            Debug.Log("WARNING: There was an attempt to pause game from outside of the GAME State.");
        }
    }

    public static void UnpauseGame()
    {
        if (currentState.GetId() == PAUSE.GetId())
        {
            currentState = GAME;
            pauseController.GetComponent<PauseController>().OnStateDisable();
            pauseController.SetActive(false);
            Time.timeScale = 1f;
            gameController.GetComponent<GameController>().Resume();
        }
        else
        {
            Debug.Log("WARNING: There was an attempt to unpause game from outside of the PAUSE State.");
        }
    }

    public static void GameOver()
    {
        if (currentState.GetId() == GAME.GetId())
        {
            currentState = GAME_OVER;
            gameOverController.SetActive(true);
            gameOverController.GetComponent<GameOverController>().OnStateEnable();
        }
        else
        {
            Debug.Log("WARNING: There was an attempt to trigger game over from outside of the GAME State.");
        }
    }

    public static void RestartGame()
    {
        if (currentState.GetId() == GAME_OVER.GetId())
        {
            currentState = GAME;
            gameOverController.GetComponent<GameOverController>().OnStateDisable();
            gameOverController.SetActive(false);
            gameController.GetComponent<GameController>().Restart();
        }
        else if (currentState.GetId() == PAUSE.GetId())
        {
            currentState = GAME;
            pauseController.GetComponent<PauseController>().OnStateDisable();
            pauseController.SetActive(false);
            Time.timeScale = 1f;
            gameController.GetComponent<GameController>().Restart();
        }
        else
        {
            Debug.Log("WARNING: There was an attempt to restart game from outside of the GAME_OVER or PAUSE State.");
        }
    }

    public static void ReturnToMainMenu()
    {
        if (currentState.GetId() == GAME_OVER.GetId())
        {
            GameController gc = gameController.GetComponent<GameController>();
            currentState = MAIN_MENU;
            gc.ResetRelo();
            gameOverController.GetComponent<GameOverController>().OnStateDisable();
            gameOverController.SetActive(false);
            gc.OnStateDisable();
            gameController.SetActive(false);
            mainMenuController.SetActive(true);
            mainMenuController.GetComponent<MainMenuController>().OnStateEnable();
        }
        else if (currentState.GetId() == PAUSE.GetId())
        {
            currentState = MAIN_MENU;
            pauseController.GetComponent<PauseController>().OnStateDisable();
            pauseController.SetActive(false);
            Time.timeScale = 1f;
            gameController.GetComponent<GameController>().OnStateDisable();
            gameController.SetActive(false);
            mainMenuController.SetActive(true);
            mainMenuController.GetComponent<MainMenuController>().OnStateEnable();
        }
        else
        {
            Debug.Log("WARNING: There was an attempt to return to the main menu from outside of the GAME_OVER or PAUSE State.");
        }
    }

    public static bool IsGamePaused()
    {
        return currentState.GetId() == PAUSE.GetId();
    }

    public static bool IsGameOver()
    {
        return currentState.GetId() == GAME_OVER.GetId();
    }

    public static int GetCurrentStateId()
    {
        return currentState.GetId();
    }
}
