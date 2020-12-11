
using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using DG.Tweening;
using DG.Tweening.Plugins.Options;
using HomaGames.Internal.DataBank.BasicTypes;
using MyBox;
using UnityEditor;
using UnityEngine;
using Event = HomaGames.Internal.DataBank.Event;
using Random = UnityEngine.Random;

public class Tile : MonoBehaviour
{
    public Action OnDiscover;
    
    [SerializeField] private FloatData lerpValue;
    
    [SerializeField] private TileType tileType;
    [SerializeField] private TileState tileState;
    
    [SerializeField] private Vector3 startScale;
    [SerializeField] private Vector2 minMaxOutlineScale;
    [SerializeField] private float scaleFactor;

    [SerializeField] private GameObject buildingModel;
    [SerializeField] private GameObject houseModel;
    [SerializeField] private GameObject parkModel;
    [SerializeField] private GameObject roadModel;
    [SerializeField] private GameObject spawnParticles;
    [SerializeField] private Color previsualisationColor;

    private GameObject _currentDisplayedModel;
    public MeshRenderer _currentDisplayModelMesh;
    public MeshRenderer _tileBaseMeshRenderer;

    private Color _startColor;

    private bool _isSelected;
    private bool _isPrevisualised;
    
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

        if (Type == TileType.Road)
        {
            SetType(TileType.Road);
        }
        else
        {
            //SetRandomType();
            SetType(Type);
        }
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
            case TileType.Road :
                roadModel.SetActive(false);
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
            case TileType.Road :
                roadModel.SetActive(true);
                _currentDisplayedModel = roadModel;
                _currentDisplayModelMesh = roadModel.GetComponent<MeshRenderer>();
                break;
        }
    }

    [ButtonMethod]
    private void SetRandomType()
    {
        SetType((TileType) Random.Range(0, Enum.GetNames(typeof(TileType)).Length - 1));
    }
    
    public void Select()
    {
        _isSelected = true;
    }

    public void Deselect()
    {
        _isSelected = false;

        _currentDisplayModelMesh.materials[1].SetFloat("_OutlineWidth", 0);
        _currentDisplayedModel.transform.localScale = startScale;
    }
    
    public void StartPrevisualisation()
    {
        _isPrevisualised = true;
    }

    public void StopPrevisualisation()
    {
        _isPrevisualised = false;

        _tileBaseMeshRenderer.material.color = _startColor;
    }

    private void Update()
    {
        if (_isSelected)
        {
            _currentDisplayModelMesh.materials[1].SetFloat("_OutlineWidth", Mathf.Lerp(minMaxOutlineScale.x, minMaxOutlineScale.y, lerpValue.Value));

            Vector3 newScale = Vector3.Lerp(startScale, startScale * ( 1 + scaleFactor), lerpValue.Value);
            _currentDisplayedModel.transform.localScale = newScale;
        }

        if (_isPrevisualised)
        {
            _tileBaseMeshRenderer.material.color = Color.Lerp(_startColor, previsualisationColor, lerpValue.Value);
        }
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
        transform.DORotate(new Vector3(180, 0, 0), 0.5f).OnComplete(TriggerSpawnEffect);
        _currentDisplayedModel.transform.DOScale(new Vector3(0.5f, 5, 0.5f), 0.5f).SetEase(Ease.OutElastic).SetDelay(0.25f);
        
        OnDiscover?.Invoke();
    }

    private void TriggerSpawnEffect()
    {
        spawnParticles.SetActive(true);
    }


    public void SetColor(Color color)
    {
        _tileBaseMeshRenderer.material.color = color;
        _startColor = color;
    }
    
    
    public enum TileType
    {
        Building,
        House,
        Park,
        Road
    }
    
    public enum TileState
    {
        Hidden,
        Discovered
    }
}