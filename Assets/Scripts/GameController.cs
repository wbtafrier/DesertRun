using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    //[SerializeField] GameObject clouds1 = default;
    //[SerializeField] GameObject clouds2 = default;

    [SerializeField] GameObject scoreObj = default;
    [SerializeField] GameObject scoreBgObj = default;
    [SerializeField] GameObject gameOverObj = default;
    [SerializeField] GameObject gameOverBgObj = default;
    [SerializeField] GameObject restartObj = default;
    [SerializeField] GameObject restartBgObj = default;
    [SerializeField] GameObject sandObj = default;
    [SerializeField] GameObject playerObj = default;
    [SerializeField] GameObject celestialBodyObj = default;
    [SerializeField] GameObject cactus1Obj = default;
    [SerializeField] GameObject cactus2Obj = default;

    static readonly float RESTART_DURATION = 1.25f;

    static Camera mainCamera;
    static Color dayColor;
    static int daysSurvived = 1;
    static int score = 0;
    static float restartTimer = 0f;
    static bool isDaytime = true;
    static bool isGameOver = false;
    static bool restarting = false;

    static TextMeshPro scoreText;
    static TextMeshPro scoreBgText;
    static GameObject gameOver;
    static GameObject gameOverBg;
    static GameObject restartButton;
    static GameObject restartButtonBg;
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
        scoreText = scoreObj.GetComponent<TextMeshPro>();
        scoreBgText = scoreBgObj.GetComponent<TextMeshPro>();
        gameOver = gameOverObj;
        gameOverBg = gameOverBgObj;
        restartButton = restartObj;
        restartButtonBg = restartBgObj;
        sand = sandObj;
        meloRelo = playerObj.GetComponent<MeloRelo>();
        celestialBody = celestialBodyObj.GetComponent<CelestialBody>();

        DeactivateGameOverAssets();
    }

    public static void Restart()
    {
        string scoreStr = "0";
        restartTimer = 0f;
        restarting = true;
        DeactivateGameOverAssets();
        isDaytime = true;
        score = 0;
        daysSurvived = 0;
        mainCamera.backgroundColor = dayColor;
        scoreBgText.text = scoreStr;
        scoreText.text = scoreStr;
    }

    static void DeactivateGameOverAssets()
    {
        if (gameOver.activeSelf)
        {
            gameOver.SetActive(false);
        }

        if (gameOverBg.activeSelf)
        {
            gameOverBg.SetActive(false);
        }

        if (restartButton.activeSelf)
        {
            restartButton.SetActive(false);
        }

        if (restartButtonBg.activeSelf)
        {
            restartButtonBg.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsPlayerEnteringScene() && !IsRestarting() && !IsGameOver())
        {
            string scoreStr = score.ToString();
            score++;
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
        gameOverBg.SetActive(true);
        gameOver.SetActive(true);
        restartButtonBg.SetActive(true);
        restartButton.GetComponent<RestartButton>().ResetButton();
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
