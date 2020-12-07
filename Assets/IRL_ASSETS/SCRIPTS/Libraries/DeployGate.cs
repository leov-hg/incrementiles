using UnityEngine;
using System.Collections;

public static class DeployGate
{

    private static AndroidJavaClass deployGate = null;

    public static void Install()
    {
        // Get Android context
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject app = activity.Call<AndroidJavaObject>("getApplicationContext");

        // Install DeployGate, make sure it will not called twice in your app
        deployGate = new AndroidJavaClass("com.deploygate.sdk.DeployGate");

        activity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
        {
            deployGate.CallStatic("install", app);
        }));
    }

    public static void LogError(string text)
    {
        deployGate.CallStatic("logError", text);
    }

    public static void LogWarn(string text)
    {
        deployGate.CallStatic("logWarn", text);
    }

    public static void LogDebug(string text)
    {
        deployGate.CallStatic("logDebug", text);
    }

    public static void LogInfo(string text)
    {
        deployGate.CallStatic("logInfo", text);
    }

    public static void LogVerbose(string text)
    {
        deployGate.CallStatic("logVerbose", text);
    }
}