using Cinemachine;
using DG.Tweening;
using HomaGames.Internal.Utilities;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Rendering.PostProcessing;

public class MixingCameraController : Singleton<MixingCameraController>
{
    [Separator("References", true)]
    [SerializeField] private CinemachineMixingCamera cmMixCam;

    [Separator("Settings", true)]
    [SerializeField] Ease easeType = Ease.Unset;
    [SerializeField] float changeDuration;
    [SerializeField] int gameCameraIndex;
    [SerializeField] int awakeCamIndex;
    [SerializeField] int editorCamIndex;

    private List<Tween> currentTweens = new List<Tween>();

    private void Awake()
    {
        SetCamera(awakeCamIndex, 0);
    }

    public void SetChangeDuration(float time)
    {
        changeDuration = time;
    }

    public void SetCamera(int index) => SetCamera(index, changeDuration, easeType);
    public void SetCamera(int index, float time) => SetCamera(index, time, easeType);

    public void SetCamera(int index, float time, Ease easeType)
    {
        if (!isActiveAndEnabled)
            return;

        for (int i = 0; i < cmMixCam.ChildCameras.Length; i++)
        {
            int camInd = i;
            bool active;
            if (index == 0)
            {
                active = camInd == gameCameraIndex;
            }
            else
            {
                active = camInd == index;
            }

            if(currentTweens.Count - 1 >= i && currentTweens[i] != null)
                currentTweens[i].Kill();

            if (currentTweens.Count - 1 < i)
            {
                currentTweens.Add(DOTween.To(() => cmMixCam.GetWeight(camInd), v => cmMixCam.SetWeight(camInd, v), active ? 1.0f : 0.0f, time).SetEase(easeType));
            }
            else
            {
                currentTweens[i] = DOTween.To(() => cmMixCam.GetWeight(camInd), v => cmMixCam.SetWeight(camInd, v), active ? 1.0f : 0.0f, time).SetEase(easeType);
            }
        }
    }
}