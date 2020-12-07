using DG.Tweening;
using HomaGames.Internal.Utilities;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static ButtonAnimations;

public class ButtonHolding : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerExitHandler, IPointerUpHandler
{
    #region Configuration Variables

    [Separator("Additional Data", true)]
    [SerializeField] private ButtonAnimations data;

    [Separator("Events", true)]
    [SerializeField] private UnityEvent onDownEvent;
    [SerializeField] private UnityEvent onUpEvent;
    [SerializeField] private UnityEvent onClickEvent;

    [Separator("Other Settings", true)]
    [SerializeField] private bool overrideAnimTarget;
    [ConditionalHide(nameof(overrideAnimTarget), true)] [SerializeField] private Transform animTarget;
    [Space(10)]
    [SerializeField] private bool onScrollRect = false;
    [SerializeField] private bool blockRaycastingAfterClick = false;
    [ConditionalHide(nameof(blockRaycastingAfterClick), true)] [SerializeField] private float blockingDelay = 1;
    #endregion

    #region Private Variables
    private Transform t;
    private Image image;

    private bool alreadyDown = false;
    private Vector3 startTouchPos;
    private Vector3 dragTouchPos;

    private Tween currentTween;
    private Transform target;
    private Vector3 initialScale;
    #endregion

    private void Awake()
    {
        t = transform;
        image = GetComponent<Image>();

        target = overrideAnimTarget && animTarget != null ? animTarget : t;
        initialScale = overrideAnimTarget && animTarget != null ? animTarget.localScale : t.localScale;
    }

    private void OnEnable()
    {
        StartLoopingAnim();
    }

    private void StartLoopingAnim()
    {
        if (data != null)
        {
            currentTween.Kill();
            currentTween = DOTween.To(() => initialScale / data.animStrength, x => target.localScale = x, initialScale * data.animStrength, data.duration).SetEase(data.loopCurve).SetDelay(data.interval);
            //currentTween = target.DOScale(initialScale * animStrength, duration)
            currentTween.onComplete += () => currentTween.Restart();
        }
    }

    private IEnumerator BlockRaycastingAfterClick()
    {
        image.raycastTarget = false;

        yield return new WaitForSeconds(blockingDelay);

        image.raycastTarget = true;
    }

    private IEnumerator DragUpdate()
    {
        while (true)
        {
            dragTouchPos = Input.mousePosition;

            if (Vector3.Distance(startTouchPos, dragTouchPos) > 50)
            {
                image.raycastTarget = false;
                Up();
                break;
            }

            yield return null;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onClickEvent?.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Down();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!alreadyDown)
        {
            return;
        }

        Up();
    }

    private void Down()
    {
        onDownEvent?.Invoke();

        if (onScrollRect)
        {
            image.raycastTarget = true;
            startTouchPos = Input.mousePosition;
            StartCoroutine(DragUpdate());
        }

        if (!alreadyDown)
        {
            if (data != null && data.inputAnim)
            {
                currentTween.Kill();
                currentTween = target.DOScale(data.scaleWay == ScaleWay.Decrease ? initialScale / data.animStrength : initialScale * data.animStrength, data.downAnimDuration).SetEase(data.downEaseType);
            }

            alreadyDown = true;
        }
    }

    private void Up()
    {
        onUpEvent?.Invoke();

        if (blockRaycastingAfterClick)
        {
            StartCoroutine(BlockRaycastingAfterClick());
        }

        if (data != null && data.inputAnim)
        {
            currentTween.Kill();
            currentTween = target.DOScale(initialScale, data.upAnimDuration).SetEase(data.upEaseType);
            currentTween.onComplete += () => StartLoopingAnim();
        }

        alreadyDown = false;
    }

    private void OnDisable()
    {
        if (data != null)
        {
            currentTween.Kill();
            target.localScale = initialScale;
        }
        alreadyDown = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!alreadyDown)
        {
            return;
        }

        Up();
    }
}
