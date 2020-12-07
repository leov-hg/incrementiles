using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animation))]
public class TransitionScreen : MonoBehaviour
{
    [SerializeField] private string inAnimClip = string.Empty;
    [SerializeField] private string outAnimClip = string.Empty;

    private Animation anim;

    public void Init()
    {
        anim = GetComponent<Animation>();
        gameObject.SetActive(false);
    }

    public float StartIn()
    {
        anim.Play(inAnimClip);
        return anim[inAnimClip].length;
    }

    public float StartOut()
    {
        anim.Play(outAnimClip);
        return anim[outAnimClip].length;
    }
}
