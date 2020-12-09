using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MyBox;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public SO_GridManager gridManagerReference;

    private List<Grid> _grids;

    private void Awake()
    {
        gridManagerReference.gridManager = this;

        _grids = GetComponentsInChildren<Grid>().ToList();
    }

    private void Start()
    {
        StartCoroutine(RevealLineCoroutine(0));
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
                localGrid = grid;
            }
        }

        if (localGrid != null)
        {
            int tileIndexInList = localGrid.TilesList.IndexOf(tileToExpand);
            Vector2Int tilePos = new Vector2Int(tileIndexInList % localGrid.width, Mathf.FloorToInt(tileIndexInList / localGrid.width));
            
            if (tilePos.x - 1 >= 0 && tilePos.y - 1 >= 0 && localGrid.TilesArray[tilePos.x - 1, tilePos.y - 1].State == Tile.TileState.Hidden)
                localGrid.TilesArray[tilePos.x - 1, tilePos.y - 1].Discover();
            if (tilePos.y - 1 >= 0 && localGrid.TilesArray[tilePos.x, tilePos.y - 1].State == Tile.TileState.Hidden)
                localGrid.TilesArray[tilePos.x , tilePos.y - 1].Discover();
            if (tilePos.x + 1 <= localGrid.width - 1 && tilePos.y - 1 >= 0 && localGrid.TilesArray[tilePos.x + 1, tilePos.y - 1].State == Tile.TileState.Hidden)
                localGrid.TilesArray[tilePos.x + 1, tilePos.y - 1].Discover();
        
            if (tilePos.x - 1 >= 0 && localGrid.TilesArray[tilePos.x - 1, tilePos.y].State == Tile.TileState.Hidden)
                localGrid.TilesArray[tilePos.x - 1, tilePos.y].Discover();
            if (tilePos.x + 1 <= localGrid.width - 1 && localGrid.TilesArray[tilePos.x + 1, tilePos.y].State == Tile.TileState.Hidden)
                localGrid.TilesArray[tilePos.x + 1, tilePos.y].Discover();
        
            if (tilePos.x - 1 >= 0 && tilePos.y + 1 <= localGrid.height - 1 && localGrid.TilesArray[tilePos.x - 1, tilePos.y + 1].State == Tile.TileState.Hidden)
                localGrid.TilesArray[tilePos.x - 1, tilePos.y + 1].Discover();
            if (tilePos.y + 1 <= localGrid.height - 1 && localGrid.TilesArray[tilePos.x, tilePos.y + 1].State == Tile.TileState.Hidden)
                localGrid.TilesArray[tilePos.x, tilePos.y + 1].Discover();
            if (tilePos.x + 1 <= localGrid.width - 1 && tilePos.y + 1 <= localGrid.height - 1 && localGrid.TilesArray[tilePos.x + 1, tilePos.y + 1].State == Tile.TileState.Hidden)
                localGrid.TilesArray[tilePos.x + 1, tilePos.y + 1].Discover();
        }
    }
    
}