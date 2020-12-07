using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using FM;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using HomaGames.Internal.Utilities;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private SettingsData settingsData;

    [ReadOnly] public bool firstStart;

    private void OnValidate()
    {
        if (settingsData.customFrameRate)
        {
            Application.targetFrameRate = settingsData.customFPS;
        }
        else
        {
            switch (settingsData.targetFrameRate)
            {
                case SettingsData.TARGET_FPS._30:
                    Application.targetFrameRate = 30;
                    break;
                case SettingsData.TARGET_FPS._60:
                    Application.targetFrameRate = 60;
                    break;
            }
        }
    }

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("FirstStart"))
        {
            firstStart = true;
            PlayerPrefs.SetInt("FirstStart", 1);
        }
        else
        {
            firstStart = false;
        }

        if (settingsData.customFrameRate)
        {
            Application.targetFrameRate = settingsData.customFPS;
        }
        else
        {
            switch (settingsData.targetFrameRate)
            {
                case SettingsData.TARGET_FPS._30:
                    Application.targetFrameRate = 30;
                    break;
                case SettingsData.TARGET_FPS._60:
                    Application.targetFrameRate = 60;
                    break;
            }
        }
    }
}
