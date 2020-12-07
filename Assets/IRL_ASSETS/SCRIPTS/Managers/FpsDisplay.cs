using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpsDisplay : MonoBehaviour
{
    [SerializeField] private SettingsData gameSettingV;

    private void Awake()
    {
        gameObject.SetActive(gameSettingV.showFPS);
    }
}
