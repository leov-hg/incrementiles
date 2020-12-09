using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public int width;
    public int height;

    [SerializeField] private List<Tile> _tilesList;
    private Tile[,] _tilesArray;

    public List<Tile> TilesList
    {
        get => _tilesList;
        set => _tilesList = value;
    }

    public Tile[,] TilesArray
    {
        get => _tilesArray;
        set => _tilesArray = value;
    }

    private void Awake()
    {
        _tilesArray = new Tile[width,height];

        _tilesList = GetComponentsInChildren<Tile>().ToList();

        int index = 0;
        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
            {
                _tilesArray[i, j] = _tilesList[index];
                index++;
            }
        }
    }
    
    
    public bool AreNeighbors(Tile tile1, Tile tile2)
    {
        if (ContainsBoth(tile1, tile2))
        {
            int tile1IndexInList = _tilesList.IndexOf(tile1);
            Vector2 tile1Pos = new Vector2Int(tile1IndexInList % width, Mathf.FloorToInt(tile1IndexInList / width));
    
            int tile2IndexInList = _tilesList.IndexOf(tile2);
            Vector2 tile2Pos = new Vector2Int(tile2IndexInList % width, Mathf.FloorToInt(tile2IndexInList / width));

            return Vector2.Distance(tile1Pos, tile2Pos) == 1; 
        }
        else
        {
            return false;
        }
    }

    public bool ContainsBoth(Tile tile1, Tile tile2)
    {
        return (_tilesList.Contains(tile1) && _tilesList.Contains(tile2));
    }

    public bool Contains(Tile tile)
    {
        return _tilesList.Contains(tile);
    }

    public void Expand(Tile tileToExpand)
    {
        int tileIndexInList = TilesList.IndexOf(tileToExpand);
        Vector2Int tilePos = new Vector2Int(tileIndexInList % width, Mathf.FloorToInt(tileIndexInList / width));
        
        //if (tilePos.x - 1 >= 0 && tilePos.y - 1 >= 0 && TilesArray[tilePos.x - 1, tilePos.y - 1].State == Tile.TileState.Hidden)
            //TilesArray[tilePos.x - 1, tilePos.y - 1].Discover();
        if (tilePos.y - 1 >= 0 && TilesArray[tilePos.x, tilePos.y - 1].State == Tile.TileState.Hidden)
            TilesArray[tilePos.x , tilePos.y - 1].Discover();
        //if (tilePos.x + 1 <= width - 1 && tilePos.y - 1 >= 0 && TilesArray[tilePos.x + 1, tilePos.y - 1].State == Tile.TileState.Hidden)
            //TilesArray[tilePos.x + 1, tilePos.y - 1].Discover();
    
        if (tilePos.x - 1 >= 0 && TilesArray[tilePos.x - 1, tilePos.y].State == Tile.TileState.Hidden)
            TilesArray[tilePos.x - 1, tilePos.y].Discover();
        if (tilePos.x + 1 <= width - 1 && TilesArray[tilePos.x + 1, tilePos.y].State == Tile.TileState.Hidden)
            TilesArray[tilePos.x + 1, tilePos.y].Discover();
    
        //if (tilePos.x - 1 >= 0 && tilePos.y + 1 <= height - 1 && TilesArray[tilePos.x - 1, tilePos.y + 1].State == Tile.TileState.Hidden)
            //TilesArray[tilePos.x - 1, tilePos.y + 1].Discover();
        if (tilePos.y + 1 <= height - 1 && TilesArray[tilePos.x, tilePos.y + 1].State == Tile.TileState.Hidden)
            TilesArray[tilePos.x, tilePos.y + 1].Discover();
        //if (tilePos.x + 1 <= width - 1 && tilePos.y + 1 <= height - 1 && TilesArray[tilePos.x + 1, tilePos.y + 1].State == Tile.TileState.Hidden)
            //TilesArray[tilePos.x + 1, tilePos.y + 1].Discover();
    }
}