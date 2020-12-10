using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DG.Tweening;
using HomaGames.Internal.DataBank.BasicTypes;
using UnityEngine;

public class MatchingController : MonoBehaviour
{
    public FloatData lerpValue;
    
    public SO_GridManager gridManager;
    
    public LayerMask tileLayer;
    public float neighboringDistance;
    
    private Tile _startTile;
    [SerializeField] private List<Tile> _targetedTiles;
    
    private RaycastHit _raycastHit;

    private Camera _mainCamera;

    private int _comboCount;

    private void Awake()
    {
        _mainCamera = Camera.main;;
        lerpValue.Value = 0;
        DOTween.To(() =>lerpValue.Value, x=>lerpValue.Value = x, 1, 0.5f).SetLoops(-1, LoopType.Yoyo);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(_mainCamera.ScreenPointToRay(Input.mousePosition), out _raycastHit, Mathf.Infinity, tileLayer))
            {
                Tile hitTile = _raycastHit.transform.GetComponent<Tile>();
                
                if (hitTile != null && hitTile.State == Tile.TileState.Discovered && hitTile.Type != Tile.TileType.Road)
                {
                    _startTile = hitTile;
                    
                    _targetedTiles.Add(_startTile);
                    _startTile.Select();
                    _comboCount++;
                    gridManager.gridManager.Expand(_targetedTiles, 2);
                }
            }
            else
            {
                _startTile = null;
            }
        }

        if (Input.GetMouseButton(0) && _startTile != null)
        {
            Debug.DrawRay(_mainCamera.transform.position, _mainCamera.ScreenPointToRay(Input.mousePosition).direction * 100);
            if (Physics.Raycast(_mainCamera.ScreenPointToRay(Input.mousePosition), out _raycastHit, Mathf.Infinity, tileLayer))
            {
                Tile hitTile = _raycastHit.transform.GetComponent<Tile>();

                if (hitTile != null && hitTile.Type == _startTile.Type && hitTile.Type != Tile.TileType.Road && !_targetedTiles.Contains(hitTile) && IsNeighbor(hitTile) && hitTile.State == Tile.TileState.Discovered)
                {
                    _targetedTiles.Add(hitTile);
                    hitTile.Select();
                    _comboCount++;
                    gridManager.gridManager.Expand(_targetedTiles, _comboCount);
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (_targetedTiles.Count > 1)
            {
                gridManager.gridManager.ValidateExpand();
                foreach (Tile tile in _targetedTiles)
                {
                    tile.Deselect();
                }
            }
            else
            {
                foreach (Tile tile in _targetedTiles)
                {
                    tile.Deselect();
                }
                gridManager.gridManager.StopPrevisualisation();
            }

            _targetedTiles = new List<Tile>();
            _comboCount = 0;
        }
    }
    
    
    private bool IsNeighbor(Tile targetedTile)
    {
        bool res = false;

        foreach (Tile tile in _targetedTiles)
        {
            if (gridManager.gridManager.AreNeighbors(tile, targetedTile))
            {
                res = true;
            }
        }
            
        return res;
    }
}