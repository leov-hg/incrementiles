using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchingController : MonoBehaviour
{
    public LayerMask tileLayer;
    
    private Tile startTile;
    private Tile currentTargetedTile;

    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            
            if (Physics.Raycast(_mainCamera.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, tileLayer))
            {
                Tile hitTile = hit.transform.GetComponent<Tile>();
                
                if (hitTile != null)
                {
                    startTile = hitTile;
                }
            }
            else
            {
                startTile = null;
            }
        }
    }
}
