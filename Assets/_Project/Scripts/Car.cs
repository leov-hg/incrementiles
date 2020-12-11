using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Car : MonoBehaviour
{
    public float timeTotravel;
    public SO_Car carRef;

    private Tween _moveTween;

    private void Awake()
    {
        carRef.car = this;
    }


    public void MoveTo(Vector3 pos)
    {
        _moveTween.Kill();
        _moveTween = transform.DOMove(pos, timeTotravel);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            MoveTo(transform.position + (Vector3.forward * 2));
        }
    }
}
