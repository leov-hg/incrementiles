using System.Collections;
using System.Collections.Generic;
using HomaGames.Internal.DataBank.BasicTypes;
using UnityEngine;
using UnityEngine.UI;

public class ProgressionUI : MonoBehaviour
{
    public IntData currentGridIndex;
    
    public Image circle1;
    public Image circle2;

    void Update()
    {
        circle1.color = Color.green;

        if (currentGridIndex.Value == 1)
            circle2.color = Color.green;
        else
            circle2.color = Color.white;
    }
}