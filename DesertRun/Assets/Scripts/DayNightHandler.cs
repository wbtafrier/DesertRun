using UnityEngine;

public class DayNightHandler : MonoBehaviour
{
    [SerializeField] GameObject celestialBodyProp = default;

    static CelestialBody celestialBody;

    static Color dayColor = default;
    static bool isDaytime = true;

    bool isResetOnRestart = false;

    // Start is called before the first frame update
    void Start()
    {
        celestialBody = celestialBodyProp.GetComponent<CelestialBody>();
        dayColor = Camera.main.backgroundColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.IsRestarting() && !isResetOnRestart)
        {
            isResetOnRestart = true;
            SetDay();
        }
        else if (!GameController.IsRestarting() && isResetOnRestart)
        {
            isResetOnRestart = false;
        }
    }

    public static void MultiplyCycleSpeed(float factor)
    {
        celestialBody.MultiplySpeed(factor);
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
        Camera.main.backgroundColor = new Color(0.17254902f, 0.0431372549f, 0.235294118f);
    }

    public static void SetDay()
    {
        isDaytime = true;
        celestialBody.ChangeToSun();
        Camera.main.backgroundColor = dayColor;
    }
}
