using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAnimations : MonoBehaviour
{
    [Separator("References", true)]
    [SerializeField] private Animation anim;

    [Separator("Settings", true)]
    [SerializeField] private string homeStateHash = string.Empty;
    [SerializeField] private string startLevelHash = string.Empty;

    public void HomeState()
    {
        anim.Play(homeStateHash);
    }

    public void StartLevel()
    {
        anim.Play(startLevelHash);
    }
}
