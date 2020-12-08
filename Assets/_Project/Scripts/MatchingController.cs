using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MatchingController : MonoBehaviour
{
    public SO_GridManager gridManager;
    
    public LayerMask tileLayer;
    public float neighboringDistance;
    
    private Tile _startTile;
    [SerializeField] private List<Tile> _targetedTiles;
    
    private RaycastHit _raycastHit;

    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(_mainCamera.ScreenPointToRay(Input.mousePosition), out _raycastHit, Mathf.Infinity, tileLayer))
            {
                Tile hitTile = _raycastHit.transform.GetComponent<Tile>();
                
                if (hitTile != null && hitTile.State == Tile.TileState.Discovered)
                {
                    _startTile = hitTile;
                    
                    _targetedTiles.Add(_startTile);
                    _startTile.Select();
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

                if (hitTile != null && hitTile.Type == _startTile.Type && !_targetedTiles.Contains(hitTile) && IsNeighbor(hitTile) && hitTile.State == Tile.TileState.Discovered)
                {
                    _targetedTiles.Add(hitTile);
                    hitTile.Select();
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (_targetedTiles.Count > 1)
            {
                foreach (Tile tile in _targetedTiles)
                {
                    gridManager.gridManager.Expand(tile);
                }
            }
            else
            {
                foreach (Tile tile in _targetedTiles)
                {
                    tile.Deselect();
                }
            }

            _targetedTiles = new List<Tile>();
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