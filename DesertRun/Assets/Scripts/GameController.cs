using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour, IStateController
{
    //[SerializeField] GameObject clouds1 = default;
    //[SerializeField] GameObject clouds2 = default;

    [SerializeField] GameObject desertGeneratorProp = default;
    [SerializeField] GameObject scoreProp = default;
    [SerializeField] GameObject scoreBgProp = default;
    [SerializeField] GameObject pauseButtonProp = default;
    [SerializeField] GameObject pauseButtonBgProp = default;
    [SerializeField] GameObject sandProp = default;
    [SerializeField] GameObject playerProp = default;
    [SerializeField] GameObject cactus1Prop = default;
    [SerializeField] GameObject cactus2Prop = default;
    [SerializeField] GameObject rockProp = default;
    [SerializeField] GameObject snakeProp = default;

    static readonly float ROUGH_SCORE_PER_SEC = 50f;
    static readonly float RESTART_DURATION = 1.25f;

    static Camera mainCamera;
    static float roughScore = 0f;
    static int score = 0;
    static float restartTimer = 0f;
    static bool restarting = false;

    static GameObject desertGeneratorObj;
    static GameObject scoreTextObj;
    static GameObject scoreBgTextObj;
    static GameObject pauseButtonObj;
    static GameObject pauseButtonBgObj;
    static GameObject sand;
    static GameObject meloReloObj;
    static GameObject cactus1;
    static GameObject cactus2;
    static GameObject rock;
    static GameObject snake;

    static DesertGenerator desertGenerator;
    static TextMeshProUGUI scoreText;
    static TextMeshProUGUI scoreBgText;
    static Button pauseButton;
    static MeloRelo meloReloComp;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        desertGeneratorObj = desertGeneratorProp;
        scoreTextObj = scoreProp;
        scoreBgTextObj = scoreBgProp;
        pauseButtonObj = pauseButtonProp;
        pauseButtonBgObj = pauseButtonBgProp;
        sand = sandProp;
        meloReloObj = playerProp;
        cactus1 = cactus1Prop;
        cactus2 = cactus2Prop;
        rock = rockProp;
        snake = snakeProp;

        desertGenerator = desertGeneratorObj.GetComponent<DesertGenerator>();
        scoreText = scoreTextObj.GetComponent<TextMeshProUGUI>();
        scoreBgText = scoreBgTextObj.GetComponent<TextMeshProUGUI>();
        pauseButton = pauseButtonObj.GetComponent<Button>();
        meloReloComp = meloReloObj.GetComponent<MeloRelo>();
    }

    public void OnStateEnable()
    {
        Start();
        desertGeneratorObj.SetActive(true);
        scoreTextObj.SetActive(true);
        scoreBgTextObj.SetActive(true);
        meloReloObj.SetActive(true);
        //cactus1.SetActive(true);
        //cactus2.SetActive(true);
        //rock.SetActive(true);
        //snake.SetActive(true);
        desertGenerator.OnGenEnable();
        Restart();
    }

    public void OnStateDisable()
    {
        desertGeneratorObj.SetActive(false);
        scoreTextObj.SetActive(false);
        scoreBgTextObj.SetActive(false);
        pauseButtonObj.SetActive(false);
        pauseButtonBgObj.SetActive(false);
        meloReloObj.SetActive(false);
        desertGenerator.OnGenDisable();
        //cactus1.SetActive(false);
        //cactus2.SetActive(false);
        //rock.SetActive(false);
        //snake.SetActive(false);
    }

    public void Pause()
    {
        if (!GameStateMachine.IsGamePaused())
        {
            pauseButtonBgObj.SetActive(false);
            pauseButtonObj.SetActive(false);
            GameStateMachine.PauseGame();
        }
    }

    public void Resume()
    {
        pauseButtonBgObj.SetActive(true);
        pauseButtonObj.SetActive(true);
    }

    public void Restart()
    {
        string scoreStr = "0";
        restartTimer = 0f;
        restarting = true;
        roughScore = 0f;
        score = 0;
        desertGenerator.Restart();

        scoreBgText.text = scoreStr;
        scoreText.text = scoreStr;
        pauseButtonBgObj.SetActive(true);
        pauseButtonObj.SetActive(true);
        pauseButton.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsPlayerEnteringScene() && !IsRestarting() && !GameStateMachine.IsGameOver())
        {
            if (!pauseButton.interactable)
            {
                pauseButton.interactable = true;
            }

            string scoreStr = score.ToString();
            roughScore += ROUGH_SCORE_PER_SEC * Time.deltaTime;
            score = (int)roughScore;
            scoreBgText.text = scoreStr;
            scoreText.text = scoreStr;
        }

        if (restarting)
        {
            restartTimer += Time.deltaTime;
            if (restartTimer >= RESTART_DURATION)
            {
                restarting = false;
            }
        }
    }

    public static bool IsPlayerEnteringScene()
    {
        return meloReloComp.IsEnteringFrame();
    }

    public static float GetRoughScore()
    {
        return roughScore;
    }

    public static void MultiplyGameSpeed(float factor)
    {
        DayNightHandler.MultiplyCycleSpeed(factor);
        MountainRange.MultiplySpeed(factor);
        Cloud.MultiplySpeed(factor);
    }
    
    public static GameObject GetSand()
    {
        return sand;
    }

    public static void SetGameOver()
    {
        if (!GameStateMachine.IsGameOver())
        {
            pauseButtonBgObj.SetActive(false);
            pauseButtonObj.SetActive(false);
            meloReloComp.Die();
            GameStateMachine.GameOver();
        }
    }

    public void ResetRelo()
    {
        Vector3 reloInitPos = meloReloComp.GetInitialPosition();
        meloReloComp.Restart();
        if (!meloReloObj.transform.position.Equals(reloInitPos))
        {
            meloReloObj.transform.position = reloInitPos;
        }
    }

    public static bool IsRestarting()
    {
        return restarting;
    }
}
