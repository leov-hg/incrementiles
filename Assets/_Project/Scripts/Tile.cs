using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public enum Type
    {
        Circle,
        Square,
        Triangle
    };

    public Type type;
}