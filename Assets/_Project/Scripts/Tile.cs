using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public enum Type
    {
        Circle,
        Square,
        Triangle
    };

    public void Select()
    {
        transform.position = transform.position - new Vector3(0, 1, 0);
    }

    public void Deselect()
    {
        transform.position = transform.position + new Vector3(0, 1, 0);
    }

    public void Validate()
    {
        
    }

    public Type type;
}