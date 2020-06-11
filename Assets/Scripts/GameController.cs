using UnityEngine;

public class GameController : MonoBehaviour
{
    //[SerializeField] GameObject clouds1 = default;
    //[SerializeField] GameObject clouds2 = default;

    [SerializeField] GameObject sandObj = default;
    [SerializeField] GameObject playerObj = default;
    [SerializeField] GameObject celestialBodyObj = default;

    static Camera mainCamera;
    static Color dayColor;
    static int daysSurvived = 1;
    static bool isPlayerEntering = true;
    static bool isDaytime = true;
    static bool isGameOver = false;

    static GameObject sand;
    static MeloRelo meloRelo;
    static CelestialBody celestialBody;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        dayColor = mainCamera.backgroundColor;
        sand = sandObj;
        meloRelo = playerObj.GetComponent<MeloRelo>();
        celestialBody = celestialBodyObj.GetComponent<CelestialBody>();

        //Vector3 clouds1Pos = CloudScroller.cloud1Pos;
        //Vector3 clouds2Pos = CloudScroller.cloud2Pos;

        //clouds1.GetComponent<CloudScroller>().SetPosition(Vector3.Scale(clouds1Pos, new Vector3(0.00875f, 0.00875776376f, 1)));
        //clouds2.GetComponent<CloudScroller>().SetPosition(Vector3.Scale(clouds2Pos, new Vector3(0.00875f, 0.00875776376f, 1)));
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerEntering)
        {
            bool check = meloRelo.IsEnteringFrame();
            if (isPlayerEntering != check)
            {
                isPlayerEntering = check;
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
        return isPlayerEntering;
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
        meloRelo.Die();
    }

    public static bool IsGameOver()
    {
        return isGameOver;
    }
}
