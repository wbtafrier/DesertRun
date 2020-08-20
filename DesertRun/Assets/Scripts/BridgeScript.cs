using System.Runtime.InteropServices;
using UnityEngine;

public class BridgeScript : MonoBehaviour
{
#if !UNITY_EDITOR && UNITY_WEBGL
    [DllImport("__Internal")]
    private static extern void OpenGame();
#endif

    // Start is called before the first frame update
    void Start()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
    OpenGame();
#endif
    }

    public void ReceiveMessageFromPage(string message)
    {
        if (message.Equals("Game Open"))
        {
            GameStateMachine.EnableSounds();
        }
        else
        {
            Debug.Log("WARNING: There was an attempt to send a message to Desert Run from the webpage but we " +
                "do not recognize the message.");
        }
    }
}
