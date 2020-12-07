using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    [SerializeField] private SettingsData settingsData;

    public GameObject debugConsoleButton;
    [Header("Toggle States Colors")]
    [SerializeField] private Color offColor;
    [SerializeField] private Color onColor;

    [Header("Haptic Buttons ref")]
    [SerializeField] private Image hapticImage;
    [SerializeField] private Transform hapticTransform;

    [Header("Sound Buttons ref")]
    [SerializeField] private Image soundImage;
    [SerializeField] private Transform soundTransform;

    [Header("Sounds Actions")]
    public AudioSource popup;
    // 0 = off et 1 = on
    private int taptic;
    private int sound;

    private bool soundMute;

    private void Awake()
    {
        GetPrefs();

        if (debugConsoleButton != null)
            debugConsoleButton.SetActive(settingsData.debugMode);
    }

    public void TOGGLE_HAPTIC()
    {
        Taptic.tapticOn = !Taptic.tapticOn;

        if (Taptic.tapticOn)
        {
            taptic = 1;
            PlayerPrefs.SetInt("haptic", taptic);

            hapticImage.color = onColor;
            //hapticTransform.localScale = Vector3.one * 1.1f;
        }
        else
        {
            taptic = 0;
            PlayerPrefs.SetInt("haptic", taptic);

            hapticImage.color = offColor;
            //hapticTransform.localScale = Vector3.one * 0.9f;
        }
    }

    public void TOGGLE_SOUND()
    {
        soundMute = !soundMute;

        if (soundMute)
        {
            AudioListener.volume = 1;
            sound = 1;
            PlayerPrefs.SetInt("sound", sound);

            soundImage.color = onColor;
            //soundTransform.localScale = Vector3.one * 1.1f;
        }
        else
        {
            AudioListener.volume = 0;
            sound = 0;
            PlayerPrefs.SetInt("sound", sound);

            soundImage.color = offColor;
            //soundTransform.localScale = Vector3.one * 0.9f;
        }
    }

    private void GetPrefs()
    {
        if (PlayerPrefs.HasKey("haptic"))
        {
            taptic = PlayerPrefs.GetInt("haptic");
        }
        else
        {
            taptic = 1;
            PlayerPrefs.SetInt("haptic", taptic);
        }

        if (taptic == 1)
        {
            Taptic.tapticOn = true;
            hapticImage.color = onColor;
            //hapticTransform.localScale = Vector3.one * 1.1f;
        }
        else
        {
            Taptic.tapticOn = false;
            hapticImage.color = offColor;
            //hapticTransform.localScale = Vector3.one * 0.9f;
        }

        if (PlayerPrefs.HasKey("sound"))
        {
            sound = PlayerPrefs.GetInt("sound");
        }
        else
        {
            sound = 1;
            PlayerPrefs.SetInt("sound", sound);
        }

        if (sound == 1)
        {
            AudioListener.volume = 1;
            soundMute = false;
            soundImage.color = offColor;
            //soundTransform.localScale = Vector3.one * 0.9f;
        }
        else
        {
            soundMute = true;
            AudioListener.volume = 0;
            soundImage.color = onColor;
            //soundTransform.localScale = Vector3.one * 1.1f;
        }
    }
}
