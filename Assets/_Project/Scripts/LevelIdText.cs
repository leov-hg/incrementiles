using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelIdText : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<TextMeshProUGUI>().text = "Level " +PlayerPrefs.GetInt("LevelId", 0).ToString();
    }
}