using DG.Tweening;
using HomaGames.Internal.Utilities;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UI_MoveTween : MonoBehaviour
{
    [Separator("References", true)]
    [SerializeField] private RectTransform refSpace;
    [SerializeField] private RectTransform target;

    [Separator("Way Settings", true)]
    [SerializeField] private Vector2 pointA_offset;
    [SerializeField] private Vector2 pointB_offset;

    [Separator("Animation Settings", true)]
    [SerializeField] private Ease easeType = Ease.Linear;
    [SerializeField] private bool speedBased = false;
    [SerializeField] private float duration = 1;
    [SerializeField] private float delay = 1;
    [SerializeField] private bool isLooped = true;

    //[Separator("Loop Settings", true)]
    
    //[ConditionalField(nameof(isLooped))] [SerializeField] private int loopCount = -1;
    //[ConditionalField(nameof(isLooped))] [SerializeField] private LoopType loopType = LoopType.Restart;


    private Canvas canvas;
    private RectTransform canvasRect;
    private RectTransform refRect;

    private Vector2 pointA;
    private Vector2 pointB;

    private Tween currentTween;
    private Vector2 targetPos;

    private void Awake()
    {
        
    }

    private void OnEnable()
    {
        if (target == null)
        {
            target = GetComponent<RectTransform>();
        }

        CalculatePoints();

        target.anchoredPosition = pointA_offset;
        //currentTween = target.DOMove(pointB, duration).SetEase(easeType).SetSpeedBased(speedBased).SetLoops(loopCount, loopType);
        currentTween = DOTween.To(() => pointA_offset, x => targetPos = x, pointB_offset, duration).SetEase(easeType).SetSpeedBased(speedBased).SetDelay(delay);

        currentTween.onUpdate += () => target.anchoredPosition = targetPos;

        if (isLooped)
            currentTween.onComplete += () => currentTween.Restart();
    }

    private void OnDisable()
    {
        currentTween.Kill();
    }

    private void CalculatePoints()
    {
        if (canvasRect == null)
        {
            if (GetComponentInParent<Canvas>())
            {
                canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
            }
            else
            {
                return;
            }
        }

        if (refSpace == null)
        {
            refRect = canvasRect;
        }
        else
        {
            refRect = refSpace;
        }

        pointA = refRect.position + new Vector3(pointA_offset.x * canvasRect.localScale.x, pointA_offset.y * canvasRect.localScale.y);
        pointB = refRect.position + new Vector3(pointB_offset.x * canvasRect.localScale.x, pointB_offset.y * canvasRect.localScale.y);
    }

    private void OnDrawGizmos()
    {
        if (Selection.activeGameObject != gameObject)
        {
            return;
        }

        CalculatePoints();

        Gizmos.color = Color.red;

        Gizmos.DrawSphere(pointA, 20);
        Gizmos.DrawSphere(pointB, 20);
    }
}
