using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchingController : MonoBehaviour
{
    public LayerMask tileLayer;
    
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
                
                if (hitTile != null)
                {
                    _startTile = hitTile;
                    _targetedTiles.Add(hitTile);
                }
            }
            else
            {
                _startTile = null;
            }
        }

        if (Input.GetMouseButton(0) && _startTile != null)
        {
            if (Physics.Raycast(_mainCamera.ScreenPointToRay(Input.mousePosition), out _raycastHit, Mathf.Infinity, tileLayer))
            {
                Tile hitTile = _raycastHit.transform.GetComponent<Tile>();
                
                if (hitTile != null && hitTile.type == _startTile.type && !_targetedTiles.Contains(hitTile) && isInDistance(hitTile.gameObject))
                {
                    _targetedTiles.Add(hitTile);
                }
            }
        }

        if (Input.GetMouseButtonUp(0)) //Todo : validation de l'action. S'il y a plus d'une tile dans la liste, alors Expand()
        {
            _targetedTiles = new List<Tile>();
        }
    }

    private bool isInDistance(GameObject hitObject)
    {
        bool res = false;

        foreach (Tile tile in _targetedTiles)
        {
            if(Vector3.Distance(hitObject.transform.position, tile.transform.position) <= 2)
            {
                res = true;
            }
        }

        return res;
    }
}