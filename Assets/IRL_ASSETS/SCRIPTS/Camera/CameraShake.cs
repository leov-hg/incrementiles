using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;

    // Transform of the camera to shake. Grabs the gameObject's transform
    // if null.
    public Transform camTransform;
    private Quaternion initialRot;

    // How long the object should shake for.
    private float shakeDuration = 0f;

    // Amplitude of the shake. A larger value shakes the camera harder.
    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;

    Vector3 originalPos;

    void Awake()
    {
        Instance = this;

        if (camTransform == null)
        {
            camTransform = GetComponent(typeof(Transform)) as Transform;
        }

        initialRot = camTransform.rotation;
    }

    public void Shake(float duration)
    {
        StartCoroutine(Shakeit(duration));
    }

    private IEnumerator Shakeit(float duration)
    {
        float d = duration;

        while(d > 0)
        {
            camTransform.localPosition = camTransform.localPosition + Random.insideUnitSphere * shakeAmount;

            d -= Time.deltaTime * decreaseFactor;

            yield return null;
        }

        d = 0f;
        camTransform.localPosition = originalPos;

        yield return null;
    }

    void OnEnable()
    {
        originalPos = camTransform.localPosition;
    }

    public void EnableShaking()
    {
        StartCoroutine(Shaking());
    }
    public void DisableShaking()
    {
        StopAllCoroutines();
        ResetToOriginalPos();
    }

    private IEnumerator Shaking()
    {
        while (true)
        {
            camTransform.localPosition = camTransform.localPosition + Random.insideUnitSphere;

            yield return null;
        }
    }

    public void ResetToOriginalPos()
    {
        camTransform.localPosition = originalPos;
    }
}
