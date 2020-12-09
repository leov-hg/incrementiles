using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Car : MonoBehaviour
{
    public float timeTotravel;
    
    public void MoveTo(Vector3 pos)
    {
        transform.DOMove(pos, timeTotravel);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            MoveTo(transform.position + (Vector3.forward * 2));
        }
    }
}
