
using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using DG.Tweening;
using MyBox;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class Tile : MonoBehaviour
{
    [SerializeField] private TileType tileType;
    [SerializeField] private TileState tileState;
    [SerializeField] private Color baseHiddenColor;
    [SerializeField] private Color baseUnselectedColor;
    [SerializeField] private Color baseSelectedColor;
    [SerializeField] private GameObject cubeModel;
    [SerializeField] private GameObject circleModel;
    [SerializeField] private GameObject triangleModel;

    private MeshRenderer _tileBaseMeshRenderer;

    public TileType Type
    {
        get => tileType;
        set => tileType = value;
    }

    public TileState State
    {
        get => tileState;
        set => tileState = value;
    }
    
    
    
    
    

    private void Awake()
    {
        _tileBaseMeshRenderer = GetComponent<MeshRenderer>();
        _tileBaseMeshRenderer.material.color = baseHiddenColor;
        
        SetRandomType();
    }




    private void SetType(TileType type)
    {
        //Disable the model based on the current Type
        switch (Type)
        {
            case TileType.Circle :
                circleModel.SetActive(false);
                break;
            case TileType.Square :
                cubeModel.SetActive(false);
                break;
            case TileType.Triangle :
                triangleModel.SetActive(false);
                break;
        }
        
        Type = type;

        switch (type)
        {
            case TileType.Circle :
                circleModel.SetActive(true);
                break;
            case TileType.Square :
                cubeModel.SetActive(true);
                break;
            case TileType.Triangle :
                triangleModel.SetActive(true);
                break;
        }
    }

    [ButtonMethod]
    private void SetRandomType()
    {
        SetType((TileType) Random.Range(0, Enum.GetNames(typeof(TileType)).Length));
    }
    
    public void Select()
    {
        _tileBaseMeshRenderer.material.color = baseSelectedColor;
    }

    public void Deselect()
    {
        _tileBaseMeshRenderer.material.color = baseUnselectedColor;
    }

    public void Validate()
    {
        
    }

    [ButtonMethod]
    public void Hide()
    {
        State = TileState.Hidden;
        transform.DORotate(new Vector3(0, 0, 0), 0.5f);
        _tileBaseMeshRenderer.material.color = baseHiddenColor;
    }

    [ButtonMethod]
    public void Discover()
    {
        State = TileState.Discovered;
        transform.DORotate(new Vector3(180, 0, 0), 0.5f);
        _tileBaseMeshRenderer.material.color = baseUnselectedColor;
    }
    
    
    
    
    
    public enum TileType
    {
        Circle,
        Square,
        Triangle
    }
    
    public enum TileState
    {
        Hidden,
        Discovered
    }
}