﻿
using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using DG.Tweening;
using DG.Tweening.Plugins.Options;
using MyBox;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class Tile : MonoBehaviour
{
    [SerializeField] private TileType tileType;
    [SerializeField] private TileState tileState;

    [SerializeField] private GameObject buildingModel;
    [SerializeField] private GameObject houseModel;
    [SerializeField] private GameObject parkModel;
    [SerializeField] private GameObject spawnParticles;

    private GameObject _currentDisplayedModel;
    private MeshRenderer _currentDisplayModelMesh;

    private MeshRenderer _tileBaseMeshRenderer;

    private Tween selectionOutlineTween;
    private Tween selectionScaleXTween;
    private Tween selectionScaleYTween;
    private Tween selectionScaleZTween;

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

        SetRandomType();
    }




    private void SetType(TileType type)
    {
        //Disable the model based on the current Type
        switch (Type)
        {
            case TileType.Building :
                buildingModel.SetActive(false);
                break;
            case TileType.House :
                houseModel.SetActive(false);
                break;
            case TileType.Park :
                parkModel.SetActive(false);
                break;
        }
        
        Type = type;

        switch (type)
        {
            case TileType.Building :
                buildingModel.SetActive(true);
                _currentDisplayedModel = buildingModel;
                _currentDisplayModelMesh = buildingModel.GetComponent<MeshRenderer>();
                break;
            case TileType.House :
                houseModel.SetActive(true);
                _currentDisplayedModel = houseModel;
                _currentDisplayModelMesh = houseModel.GetComponent<MeshRenderer>();
                break;
            case TileType.Park :
                parkModel.SetActive(true);
                _currentDisplayedModel = parkModel;
                _currentDisplayModelMesh = parkModel.GetComponent<MeshRenderer>();
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
       selectionOutlineTween = DOTween.To(()=> _currentDisplayModelMesh.materials[1].GetFloat("_OutlineWidth"), x=> _currentDisplayModelMesh.materials[1].SetFloat("_OutlineWidth", x), 0.2f, 0.5f).SetLoops(-1, LoopType.Yoyo);
       //selectionScaleXTween = transform.DOScaleX(0.7f, 0.5f).SetLoops(-1, LoopType.Yoyo);
       //selectionScaleYTween = transform.DOScaleY(0.12f, 0.5f).SetLoops(-1, LoopType.Yoyo);
       //selectionScaleZTween = transform.DOScaleZ(0.7f, 0.5f).SetLoops(-1, LoopType.Yoyo);
       
       selectionScaleXTween = _currentDisplayedModel.transform.DOScaleX(0.6f, 0.5f).SetLoops(-1, LoopType.Yoyo);
       selectionScaleYTween = _currentDisplayedModel.transform.DOScaleY(5.2f, 0.5f).SetLoops(-1, LoopType.Yoyo);
       selectionScaleZTween = _currentDisplayedModel.transform.DOScaleZ(0.6f, 0.5f).SetLoops(-1, LoopType.Yoyo);
    }

    public void Deselect()
    {
        _currentDisplayModelMesh.materials[1].SetFloat("_OutlineWidth", 0);
        _currentDisplayedModel.transform.localScale = new Vector3(0.5f, 5, 0.5f);
        
        selectionOutlineTween.Kill();
        selectionScaleXTween.Kill();
        selectionScaleYTween.Kill();
        selectionScaleZTween.Kill();
    }

    public void Validate()
    {
        
    }

    [ButtonMethod]
    public void Hide()
    {
        State = TileState.Hidden;
        transform.DORotate(new Vector3(0, 0, 0), 0.5f);
    }

    [ButtonMethod]
    public void Discover()
    {
        State = TileState.Discovered;
        transform.DORotate(new Vector3(180, 0, 0), 0.25f).OnComplete(TriggerSpawnEffect);
        _currentDisplayedModel.transform.DOScale(new Vector3(0.5f, 5, 0.5f), 0.25f).SetEase(Ease.OutElastic).SetDelay(0.25f);
    }

    private void TriggerSpawnEffect()
    {
        spawnParticles.SetActive(true);
    }


    public void SetColor(Color color)
    {
        _tileBaseMeshRenderer.material.color = color;
    }
    
    
    public enum TileType
    {
        Building,
        House,
        Park
    }
    
    public enum TileState
    {
        Hidden,
        Discovered
    }
}