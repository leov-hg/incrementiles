using DG.Tweening;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ButtonAnimations", menuName = "IRL_Assets/ButtonAnimations")]
public class ButtonAnimations : ScriptableObject
{
    public enum ScaleWay
    {
        Decrease,
        Increase
    }

    public float animStrength = 1.1f;
    public bool inputAnim = true;

    [Separator("Down", true)]
    [ConditionalHide(nameof(inputAnim), true)] public float downAnimDuration = .1f;
    [ConditionalHide(nameof(inputAnim), true)] public ScaleWay scaleWay = ScaleWay.Increase;
    [ConditionalHide(nameof(inputAnim), true)] public Ease downEaseType = Ease.OutQuad;

    [Separator("Up", true)]
    [ConditionalHide(nameof(inputAnim), true)] public float upAnimDuration = .1f;
    [ConditionalHide(nameof(inputAnim), true)] public Ease upEaseType = Ease.OutQuad;

    [Separator("Looping", true)]
    public bool loopAnim = true;
    [ConditionalHide(nameof(loopAnim), true)] public AnimationCurve loopCurve;
    [ConditionalHide(nameof(loopAnim), true)] public float duration = 1;
    [ConditionalHide(nameof(loopAnim), true)] public float interval = 1;
}
