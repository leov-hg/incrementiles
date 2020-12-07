using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingJoystick : Joystick
{
    private Vector2 initialPos;

    protected override void Start()
    {
        base.Start();

        initialPos = background.anchoredPosition;
        backgroundImage.color = upColor;
        handleImage.color = upColor;
        //background.gameObject.SetActive(false);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        backgroundImage.color = downColor;
        handleImage.color = downColor;

        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        background.gameObject.SetActive(true);
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        backgroundImage.color = upColor;
        handleImage.color = upColor;

        //background.gameObject.SetActive(false);
        background.anchoredPosition = initialPos;
        base.OnPointerUp(eventData);
    }
}