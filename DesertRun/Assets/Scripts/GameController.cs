using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    //[SerializeField] GameObject clouds1 = default;
    //[SerializeField] GameObject clouds2 = default;

    [SerializeField] TMP_Text scoreObj = default;
    [SerializeField] TMP_Text scoreBgObj = default;
    [SerializeField] TMP_Text gameOverObj = default;
    [SerializeField] TMP_Text gameOverBgObj = default;
    [SerializeField] GameObject restartButtonObj = default;
    [SerializeField] Image restartBgObj = default;
    [SerializeField] GameObject sandObj = default;
    [SerializeField] GameObject playerObj = default;
    [SerializeField] GameObject celestialBodyObj = default;
    [SerializeField] GameObject cactus1Obj = default;
    [SerializeField] GameObject cactus2Obj = default;

    static readonly float ROUGH_SCORE_PER_SEC = 50f;
    static readonly float RESTART_DURATION = 1.25f;

    static Camera mainCamera;
    static Color dayColor;
    static int daysSurvived = 1;
    static float roughScore = 0f;
    static int score = 0;
    static float restartTimer = 0f;
    static bool isDaytime = true;
    static bool isGameOver = false;
    static bool restarting = false;

    static TextMeshProUGUI scoreText;
    static TextMeshProUGUI scoreBgText;
    static TextMeshProUGUI gameOver;
    static TextMeshProUGUI gameOverBg;
    static GameObject restartButton;
    static Image restartButtonBg;
    static GameObject sand;
    static MeloRelo meloRelo;
    static CelestialBody celestialBody;
    static DesertObject cactus1;
    static DesertObject cactus2;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        dayColor = mainCamera.backgroundColor;
        scoreText = scoreObj.GetComponent<TextMeshProUGUI>();
        scoreBgText = scoreBgObj.GetComponent<TextMeshProUGUI>();
        gameOver = gameOverObj.GetComponent<TextMeshProUGUI>();
        gameOverBg = gameOverBgObj.GetComponent<TextMeshProUGUI>();
        restartButton = restartButtonObj;
        restartButtonBg = restartBgObj;
        sand = sandObj;
        meloRelo = playerObj.GetComponent<MeloRelo>();
        celestialBody = celestialBodyObj.GetComponent<CelestialBody>();

        DeactivateGameOverAssets();
    }

    public void Restart()
    {
        string scoreStr = "0";
        restartTimer = 0f;
        restarting = true;
        DeactivateGameOverAssets();
        isDaytime = true;
        roughScore = 0f;
        score = 0;
        daysSurvived = 0;
        mainCamera.backgroundColor = dayColor;
        scoreBgText.text = scoreStr;
        scoreText.text = scoreStr;
    }

    static void DeactivateGameOverAssets()
    {
        if (gameOver.enabled)
        {
            gameOver.enabled = false;
        }

        if (gameOverBg.enabled)
        {
            gameOverBg.enabled = false;
        }

        if (restartButton.activeSelf)
        {
            restartButton.SetActive(false);
        }

        if (restartButtonBg.enabled)
        {
            restartButtonBg.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsPlayerEnteringScene() && !IsRestarting() && !IsGameOver())
        {
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
                isGameOver = false;
                restarting = false;
            }
        }
    }
    
    public static void LightSwitch()
    {
        if (isDaytime)
        {
            SetNight();
        }
        else
        {
            SetDay();
        }
    }

    public static void SetNight()
    {
        isDaytime = false;
        celestialBody.ChangeToMoon();
        mainCamera.backgroundColor = new Color(0.17254902f, 0.0431372549f, 0.235294118f);
    }

    public static void SetDay()
    {
        isDaytime = true;
        daysSurvived++;
        celestialBody.ChangeToSun();
        mainCamera.backgroundColor = dayColor;
    }

    public static bool IsPlayerEnteringScene()
    {
        return meloRelo.IsEnteringFrame();
    }

    public static bool IsDay()
    {
        return isDaytime;
    }

    public static Camera GetMainCamera()
    {
        return mainCamera;
    }

    public static GameObject GetSand()
    {
        return sand;
    }

    public static void SetGameOver()
    {
        Debug.Log("GAME OVER");
        isGameOver = true;
        gameOverBg.enabled = true;
        gameOver.enabled = true;
        restartButtonBg.enabled = true;
        //restartButton.GetComponent<RestartButton>().ResetButton();
        restartButton.SetActive(true);
        meloRelo.Die();
    }

    public static bool IsGameOver()
    {
        return isGameOver;
    }

    public static bool IsRestarting()
    {
        return restarting;
    }
}
