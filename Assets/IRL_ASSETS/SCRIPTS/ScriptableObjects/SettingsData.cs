using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

[CreateAssetMenu(fileName = "SettingsData", menuName = "IRL_Assets/SettingsData")]
public class SettingsData : ScriptableObject
{
    public enum TARGET_FPS
    {
        _30,
        _60,
    }

    public bool customFrameRate = false;
    [ConditionalField("customFrameRate", true)] public TARGET_FPS targetFrameRate = TARGET_FPS._60;
    [ConditionalField("customFrameRate")] public int customFPS = 30;
    [Space(10)]
    public bool debugMode = false;
    public bool showFPS;
    public bool displayDollarSymbol = false;
}