using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour, IStateController
{
    [SerializeField] GameObject desertGeneratorProp = default;
    [SerializeField] GameObject scoreProp = default;
    [SerializeField] GameObject scoreBgProp = default;
    [SerializeField] GameObject fasterTextProp = default;
    [SerializeField] GameObject fasterTextBgProp = default;
    [SerializeField] GameObject pauseButtonProp = default;
    [SerializeField] GameObject pauseButtonBgProp = default;
    [SerializeField] GameObject sessionHighScoreTextBgProp = default;
    [SerializeField] GameObject sessionHighScoreTextProp = default;
    [SerializeField] GameObject sandProp = default;
    [SerializeField] GameObject playerProp = default;
    [SerializeField] GameObject[] coinProps = default;
    [SerializeField] Color coinPowerUpColorProp = default;

    [SerializeField] bool balloonDebugProp = false;

    static readonly float ROUGH_SCORE_PER_SEC = 50f;
    static readonly float RESTART_DURATION = 1.25f;
    static readonly float TEXT_SPEED_EFFECT_DURATION = 3f;
    static readonly string SESSION_HIGH_SCORE_TEXT = "YOUR HIGH SCORE: ";
    static readonly float BALLOON_PITCH_INCREMENT = 0.05f;

    static float roughScore = 0f;
    static int score = 0;
    static float restartTimer = 0f;
    static bool restarting = false;
    static bool textSpeedEffect = false;
    static float textSpeedEffectTimer = 0f;
    static int sessionHighScore = 0;
    static bool newHighScore = false;
    static int totalCoinsCollected = 0;
    static float balloonPitch = 1f;
    static float invColorChangeTimer = 0f;
    static bool powerUpWarning = false;

    static GameObject desertGeneratorObj;
    static GameObject scoreTextObj;
    static GameObject scoreBgTextObj;
    static GameObject fasterTextObj;
    static GameObject fasterBgTextObj;
    static GameObject pauseButtonObj;
    static GameObject pauseButtonBgObj;
    static GameObject sessionHighScoreTextBgObj;
    static GameObject sessionHighScoreTextObj;
    static GameObject sand;
    static GameObject meloReloObj;
    static GameObject[] coinObjs;
    static int currentCoin = 0;

    static DesertGenerator desertGenerator;
    static TextMeshProUGUI scoreText;
    static TextMeshProUGUI scoreBgText;
    static TextMeshProUGUI fasterText;
    static TextMeshProUGUI fasterBgText;
    static TextMeshProUGUI sessionHighScoreTextBg;
    static TextMeshProUGUI sessionHighScoreText;
    static Button pauseButton;
    static MeloRelo meloReloComp;
    static Image[] coinImgs;

    static Color scoreTextDefaultColor;
    static Color sessionHighScoreTextDefaultColor;
    static Color coinDefaultColor;
    static Color coinPowerUpColor;

    static bool balloonDebug = false;

    // Start is called before the first frame update
    void Start()
    {
        desertGeneratorObj = desertGeneratorProp;
        scoreTextObj = scoreProp;
        scoreBgTextObj = scoreBgProp;
        fasterTextObj = fasterTextProp;
        fasterBgTextObj = fasterTextBgProp;
        sessionHighScoreTextBgObj = sessionHighScoreTextBgProp;
        sessionHighScoreTextObj = sessionHighScoreTextProp;
        pauseButtonObj = pauseButtonProp;
        pauseButtonBgObj = pauseButtonBgProp;
        sand = sandProp;
        meloReloObj = playerProp;
        coinObjs = coinProps;

        desertGenerator = desertGeneratorObj.GetComponent<DesertGenerator>();
        scoreText = scoreTextObj.GetComponent<TextMeshProUGUI>();
        scoreBgText = scoreBgTextObj.GetComponent<TextMeshProUGUI>();
        fasterText = fasterTextObj.GetComponent<TextMeshProUGUI>();
        fasterBgText = fasterBgTextObj.GetComponent<TextMeshProUGUI>();
        sessionHighScoreTextBg = sessionHighScoreTextBgObj.GetComponent<TextMeshProUGUI>();
        sessionHighScoreText = sessionHighScoreTextObj.GetComponent<TextMeshProUGUI>();
        pauseButton = pauseButtonObj.GetComponent<Button>();
        meloReloComp = meloReloObj.GetComponent<MeloRelo>();
        coinImgs = new Image[coinObjs.Length];
        for (int i = 0; i < coinObjs.Length; i++)
        {
            coinImgs[i] = coinObjs[i].GetComponent<Image>();
        }
        coinDefaultColor = coinImgs[0].color;
        coinPowerUpColor = coinPowerUpColorProp;

        string sessionHighScoreStr = sessionHighScore.ToString();
        scoreTextDefaultColor = scoreText.color;
        sessionHighScoreTextDefaultColor = sessionHighScoreText.color;
        sessionHighScoreText.text = SESSION_HIGH_SCORE_TEXT + sessionHighScoreStr;
        sessionHighScoreTextBg.text = SESSION_HIGH_SCORE_TEXT + sessionHighScoreStr;

        balloonDebug = balloonDebugProp;
        if (balloonDebug)
        {
            Debug.LogWarning("BALLOON DEBUG IS ON!");
        }
    }

    public void OnStateEnable()
    {
        Start();
        desertGeneratorObj.SetActive(true);
        scoreTextObj.SetActive(true);
        scoreBgTextObj.SetActive(true);
        sessionHighScoreTextObj.SetActive(true);
        sessionHighScoreTextBgObj.SetActive(true);
        meloReloObj.SetActive(true);
        desertGenerator.OnGenEnable();
        Restart();
    }

    public void OnStateDisable()
    {
        ResetScoreTextColor();

        if (sessionHighScoreText.color.Equals(Color.red))
        {
            sessionHighScoreText.color = sessionHighScoreTextDefaultColor;
            sessionHighScoreText.fontStyle = FontStyles.Normal;
            sessionHighScoreTextBg.fontStyle = FontStyles.Normal;
        }
        newHighScore = true;

        desertGeneratorObj.SetActive(false);
        scoreTextObj.SetActive(false);
        scoreBgTextObj.SetActive(false);
        sessionHighScoreTextObj.SetActive(false);
        sessionHighScoreTextBgObj.SetActive(false);
        pauseButtonObj.SetActive(false);
        pauseButtonBgObj.SetActive(false);
        meloReloObj.SetActive(false);
        meloReloComp.KillInvincibility();
        desertGenerator.OnGenDisable();

        ResetCoins(true);
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
        ResetScoreTextColor();

        if (sessionHighScoreText.color.Equals(Color.red))
        {
            sessionHighScoreText.color = sessionHighScoreTextDefaultColor;
            sessionHighScoreText.fontStyle = FontStyles.Normal;
            sessionHighScoreTextBg.fontStyle = FontStyles.Normal;
        }

        pauseButtonBgObj.SetActive(true);
        pauseButtonObj.SetActive(true);
        pauseButton.interactable = false;
        ResetCoins(true);

        foreach (GameObject obj in coinObjs)
        {
            obj.SetActive(false);
        }
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

            if (score > sessionHighScore)
            {
                if (newHighScore)
                {
                    if (!sessionHighScoreText.color.Equals(Color.red))
                    {
                        sessionHighScoreText.color = Color.red;
                        sessionHighScoreText.fontStyle = FontStyles.Italic;
                        sessionHighScoreTextBg.fontStyle = FontStyles.Italic;
                    }
                }

                sessionHighScore = score;
                sessionHighScoreText.text = SESSION_HIGH_SCORE_TEXT + scoreStr;
                sessionHighScoreTextBg.text = SESSION_HIGH_SCORE_TEXT + scoreStr;
            }

            roughScore += ROUGH_SCORE_PER_SEC * Time.deltaTime;
            score = (int)roughScore;
            scoreBgText.text = scoreStr;
            scoreText.text = scoreStr;

            if (textSpeedEffect)
            {
                textSpeedEffectTimer += Time.deltaTime;
                if (scoreText.color != Color.red)
                {
                    scoreText.color = Color.red;
                    scoreText.fontStyle = FontStyles.Italic;
                    scoreBgText.fontStyle = FontStyles.Italic;
                    fasterTextObj.SetActive(true);
                    fasterBgTextObj.SetActive(true);
                }
                
                if (textSpeedEffectTimer >= TEXT_SPEED_EFFECT_DURATION)
                {
                    ResetScoreTextColor();
                }
            }

            if (meloReloComp.IsInvincible() && powerUpWarning)
            {
                invColorChangeTimer += Time.deltaTime;
                if (invColorChangeTimer > 0.2f)
                {
                    foreach (Image i in coinImgs)
                    {
                        Color c = i.color.Equals(coinDefaultColor) ? coinPowerUpColor : coinDefaultColor;
                        i.color = c;
                    }
                    meloReloComp.FlashColor();
                    invColorChangeTimer = 0f;
                }
            }
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

    public static void ResetScoreTextColor()
    {
        scoreText.color = scoreTextDefaultColor;
        scoreText.fontStyle = FontStyles.Normal;
        scoreBgText.fontStyle = FontStyles.Normal;
        textSpeedEffectTimer = 0f;
        textSpeedEffect = false;
        fasterTextObj.SetActive(false);
        fasterBgTextObj.SetActive(false);
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
        textSpeedEffect = true;
    }

    public static void AddCoin()
    {
        if (!GameStateMachine.IsGameOver() && !meloReloComp.IsInvincible())
        {
            coinObjs[currentCoin].SetActive(true);
            currentCoin++;
            totalCoinsCollected++;
            balloonPitch += BALLOON_PITCH_INCREMENT;

            if (currentCoin >= coinObjs.Length)
            {
                foreach (Image i in coinImgs)
                {
                    i.color = coinPowerUpColor;
                }
                invColorChangeTimer = 0f;
                meloReloComp.PowerUpInvincible();
            }
        }
    }

    public static void ResetCoins(bool gameEnding)
    {
        currentCoin = 0;
        balloonPitch = 1f;
        invColorChangeTimer = 0f;
        powerUpWarning = false;

        for (int i = 0; i < coinObjs.Length; i++)
        {
            coinObjs[i].SetActive(false);
            coinImgs[i].color = coinDefaultColor;
        }

        if (gameEnding)
        {
            totalCoinsCollected = 0;
        }
    }

    public static void WarnPowerUpExpiring()
    {
        powerUpWarning = true;
    }
    
    public static GameObject GetSand()
    {
        return sand;
    }

    public static void SetGameOver()
    {
        if (!GameStateMachine.IsGameOver())
        {
            newHighScore = true;
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

    public static bool IsReloInvincible()
    {
        return meloReloComp.IsInvincible();
    }

    public static float GetBalloonPitch()
    {
        return balloonPitch;
    }

    public static bool IsBalloonDebugOn()
    {
        return balloonDebug;
    }

}
