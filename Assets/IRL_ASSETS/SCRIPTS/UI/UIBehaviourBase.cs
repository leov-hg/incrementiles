using HomaGames.Internal.Utilities;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class UIBehaviourBase : UIBehaviour
{
    [Separator("Anim Settings", true)]
    [SerializeField] private bool useAnim = false;
    [ConditionalHide(nameof(useAnim), true)] [SerializeField] private Animator anim;
    [ConditionalHide(nameof(useAnim), true)] [SerializeField] private string openTrigger = "Open";
    [ConditionalHide(nameof(useAnim), true)] [SerializeField] private string closeTrigger = "Close";

    [Separator("Base Settings", true)]
    [SerializeField] private bool enabledOnAwake = false;

    private Canvas canvas;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        canvas.enabled = enabledOnAwake;

        if (useAnim)
        {
            anim.enabled = false;
        }
    }

    public override void Open()
    {
        canvas.enabled = true;

        if (useAnim)
        {
            anim.enabled = true;
            anim.Rebind();
            anim.SetTrigger(openTrigger);
        }
    }

    public override void Close()
    {
        if (useAnim)
        {
            anim.SetTrigger(closeTrigger);
        }
        else
        {
            canvas.enabled = false;
        }
    }
}
