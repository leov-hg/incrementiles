using System;
using Cinemachine;
using DG.Tweening;
using HomaGames.Internal.Utilities;
using System.Collections;
using UnityEngine;
using Cinemachine;

public class CameraController : Singleton<CameraController>
{
    [SerializeField]
    CinemachineMixingCamera cmMixCam;
    [SerializeField]
    float changeDuration;
    [SerializeField]
    int gameCameraIndex;

    private CinemachineVirtualCamera duelVirtualCamera;

    private void Awake()
    {
        duelVirtualCamera = cmMixCam.ChildCameras[duelCameraIndex].GetComponent<CinemachineVirtualCamera>();
    }

    public int duelCameraIndex;

    public void SetCamera(int index) => SetCamera(index, changeDuration);

    public void SetCamera(int index, float time)
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
            DOTween.To(() => cmMixCam.GetWeight(camInd), v => cmMixCam.SetWeight(camInd, v), active ? 1.0f : 0.0f, time).SetEase(Ease.InOutSine);
        }
    }

    public void StartCameraShakeIn(float timeBeforeShake, float shakeFor)
    {
        StartCoroutine(StartCameraShakeInCoroutine(timeBeforeShake, shakeFor));
    }
    
    private IEnumerator StartCameraShakeInCoroutine(float timeBeforeShake, float shakeFor)
    {
        yield return new WaitForSeconds(timeBeforeShake);
        duelVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 3;
        yield return new WaitForSeconds(shakeFor);
        duelVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
    }

    public void ResetCameraShake()
    {
        duelVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
    }

    public float ChangeDuration
    {
        set => changeDuration = value;
    }
}