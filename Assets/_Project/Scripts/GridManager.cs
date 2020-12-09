﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MyBox;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public SO_GridManager gridManagerReference;

    public Color colorLight;
    public Color colorDark;

    private List<Grid> _grids;

    private void Awake()
    {
        gridManagerReference.gridManager = this;

        _grids = GetComponentsInChildren<Grid>().ToList();
    }

    private void Start()
    {
        StartCoroutine(RevealLineCoroutine(0));
        InitGridsColor();
    }

    public void InitGridsColor()
    {
        foreach (Grid grid in _grids)
        {
            for (int i = 0; i < grid.TilesList.Count; i++)
            {
                if (i % 2 == 0)
                    grid.TilesList[i].SetColor(colorLight);
                else
                    grid.TilesList[i].SetColor(colorDark);
            }
        }
    }

    public bool AreNeighbors(Tile tile1, Tile tile2)
    {
        bool res = false;
        
        foreach (Grid grid in _grids)
        {
            if (grid.AreNeighbors(tile1, tile2))
            {
                res = true;
            }
        }

        return res;
    }

    [ButtonMethod]
    /*public void Reveal()
    {
        StartCoroutine(RevealCoroutine());
    }*/

    private IEnumerator RevealLineCoroutine(int lineIndex)
    {
        foreach (Grid grid in _grids)
        {
            for (int i = 0; i < grid.width; i++)
            {
                grid.TilesArray[i,lineIndex].Discover();
                yield return new WaitForSeconds(0.02f);
            }
        }

    }

    /*private IEnumerator RevealCoroutine()
    {
        foreach (Tile tile in grid1)
        {
            tile.Discover();
            yield return new WaitForSeconds(0.02f);
        }
    }*/


    public void Expand(Tile tileToExpand)
    {
        Grid localGrid = null;

        foreach (Grid grid in _grids)
        {
            if (grid.Contains(tileToExpand))
            {
                grid.Expand(tileToExpand);
            }
        }
    }
    
}