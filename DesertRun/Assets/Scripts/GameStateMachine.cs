using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class GameStateMachine : MonoBehaviour
{
    private static bool gameLoaded = false;
    private static float gameVolumeBeforeMute = 1f;

    [SerializeField] GameObject musicManagerProp = default;
    [SerializeField] GameObject mainMenuControllerProp = default;
    [SerializeField] GameObject settingsControllerProp = default;
    [SerializeField] GameObject gameControllerProp = default;
    [SerializeField] GameObject pauseControllerProp = default;
    [SerializeField] GameObject gameOverControllerProp = default;
    [SerializeField] GameObject muteButtonProp = default;

    public static readonly GameState MAIN_MENU = new GameState(0, "main_menu");
    public static readonly GameState SETTINGS = new GameState(1, "settings");
    public static readonly GameState GAME = new GameState(2, "game");
    public static readonly GameState PAUSE = new GameState(3, "pause");
    public static readonly GameState GAME_OVER = new GameState(4, "game_over");

    static GameObject musicManager;
    static GameObject mainMenuController;
    static GameObject settingsController;
    static GameObject gameController;
    static GameObject pauseController;
    static GameObject gameOverController;
    static GameObject muteButtonObj;

    private static GameState currentState = MAIN_MENU;

    static Button muteButton;
    static AudioSource music;
    static AudioSource click1Sfx;
    static AudioSource click2Sfx;

    static Sprite volumeOn;
    static Sprite volumeOff;

#if UNITY_WEBGL
    [DllImport("__Internal")] private static extern void GameLoaded();
#endif

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
        musicManager = musicManagerProp;
        mainMenuController = mainMenuControllerProp;
        settingsController = settingsControllerProp;
        gameController = gameControllerProp;
        pauseController = pauseControllerProp;
        gameOverController = gameOverControllerProp;
        muteButtonObj = muteButtonProp;

        if (!mainMenuController.activeSelf)
        {
            mainMenuController.SetActive(true);
        }

        gameVolumeBeforeMute = AudioListener.volume;
        muteButton = muteButtonObj.GetComponent<Button>();
        volumeOn = muteButton.image.sprite;
        volumeOff = Resources.Load<Sprite>("Sprites/volumeOff");

        music = musicManager.GetComponent<AudioSource>();

        AudioSource[] buttonSfxList = GetComponents<AudioSource>();
        foreach (AudioSource sfx in buttonSfxList)
        {
            string sfxName = sfx.clip.name;

            if (sfxName.Equals("CLICK"))
            {
                click1Sfx = sfx;
            }
            else if (sfxName.Equals("CLICK 2"))
            {
                click2Sfx = sfx;
            }
        }


        if (!gameLoaded)
        {
#if UNITY_WEBGL
            GameLoaded();
#elif UNITY_EDITOR
            Debug.Log("GAME LOADED");
#endif
            gameLoaded = true;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void EnableSounds()
    {
        if (!music.isPlaying)
        {
            music.Play();
        }
    }

    public static void DisableSounds()
    {
        if (music.isPlaying)
        {
            music.Stop();
        }
    }

    public void PlayClick1Sfx()
    {
        click1Sfx.Play();
    }

    public void PlayClick2Sfx()
    {
        click2Sfx.Play();
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
            currentState = SETTINGS;
            mainMenuController.GetComponent<MainMenuController>().OnStateDisable();
            mainMenuController.SetActive(false);
            settingsController.SetActive(true);
            settingsController.GetComponent<SettingsController>().OnStateEnable();
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
        else if (currentState.GetId() == SETTINGS.GetId())
        {
            currentState = MAIN_MENU;
            settingsController.GetComponent<SettingsController>().OnStateDisable();
            settingsController.SetActive(false);
            mainMenuController.SetActive(true);
            mainMenuController.GetComponent<MainMenuController>().OnStateEnable();
        }
        else
        {
            Debug.Log("WARNING: There was an attempt to return to the main menu from outside of the GAME_OVER, " +
                "PAUSE, or SETTINGS States.");
        }
    }

    public void MuteButtonClick()
    {
        string spriteName = muteButton.image.sprite.name;
        if (spriteName.Equals(volumeOn.name))
        {
            muteButton.image.sprite = volumeOff;
            gameVolumeBeforeMute = AudioListener.volume;
            AudioListener.volume = 0;
        }
        else
        {
            muteButton.image.sprite = volumeOn;
            AudioListener.volume = gameVolumeBeforeMute;
        }

        if (currentState.GetId() == SETTINGS.GetId())
        {
            SettingsController.UpdateVolume();
        }
    }

    public static void UpdateMuteButton()
    {
        string spriteName = muteButton.image.sprite.name;
        float volume = AudioListener.volume;
        Image muteButtonImg = muteButton.image;

        if (volume == 0f && !spriteName.Equals(volumeOff.name))
        {
            muteButtonImg.sprite = volumeOff;
        }
        else if (volume > 0f && !spriteName.Equals(volumeOn.name))
        {
            muteButtonImg.sprite = volumeOn;
        }    
    }

    public static float GetMusicVolume()
    {
        return music.volume;
    }

    public static void SetMusicVolume(float volume)
    {
        music.volume = volume;
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
