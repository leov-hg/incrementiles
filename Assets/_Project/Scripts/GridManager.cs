using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public SO_GridManager gridManagerReference;
    
    public List<Tile> grid1;
    public List<Tile> grid2;
    public int width;
    public int height;

    private Tile[,] grid1Array;
    private Tile[,] grid2Array;

    private void Awake()
    {
        gridManagerReference.gridManager = this;
        
        grid1Array = new Tile[width,height];
        grid2Array = new Tile[width,height];
        
        int index = 0;
        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
            {
                grid1Array[i, j] = grid1[index];
                index++;
            }
        }
        
        index = 0;
        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
            {
                grid2Array[i, j] = grid2[index];
                index++;
            }
        }
    }

    private void Start()
    {
        StartCoroutine(RevealLineCoroutine(0));
    }

    public bool AreNeighbors(Tile tile1, Tile tile2)
    {
        if (grid1.Contains(tile1) && grid1.Contains(tile2))
        {
            int tile1IndexInList = grid1.IndexOf(tile1);
            Vector2 tile1Pos = new Vector2Int(tile1IndexInList % width, Mathf.FloorToInt(tile1IndexInList / width));
        
            int tile2IndexInList = grid1.IndexOf(tile2);
            Vector2 tile2Pos = new Vector2Int(tile2IndexInList % width, Mathf.FloorToInt(tile2IndexInList / width));

            return Vector2.Distance(tile1Pos, tile2Pos) == 1;
        }
        else if (grid2.Contains(tile1) && grid2.Contains(tile2))
        {
            int tile1IndexInList = grid2.IndexOf(tile1);
            Vector2 tile1Pos = new Vector2Int(tile1IndexInList % width, Mathf.FloorToInt(tile1IndexInList / width));
        
            int tile2IndexInList = grid2.IndexOf(tile2);
            Vector2 tile2Pos = new Vector2Int(tile2IndexInList % width, Mathf.FloorToInt(tile2IndexInList / width));

            return Vector2.Distance(tile1Pos, tile2Pos) == 1;
        }
        else
        {
            return false;
        }

    }

    [ButtonMethod]
    public void Reveal()
    {
        StartCoroutine(RevealCoroutine());
    }

    private IEnumerator RevealLineCoroutine(int lineIndex)
    {
        for (int i = 0; i < width; i++)
        {
            grid1Array[i,lineIndex].Discover();
            yield return new WaitForSeconds(0.02f);
        }
        
        for (int i = 0; i < width; i++)
        {
            grid2Array[i,lineIndex].Discover();
            yield return new WaitForSeconds(0.02f);
        }
    }

    private IEnumerator RevealCoroutine()
    {
        foreach (Tile tile in grid1)
        {
            tile.Discover();
            yield return new WaitForSeconds(0.02f);
        }
        
        foreach (Tile tile in grid2)
        {
            tile.Discover();
            yield return new WaitForSeconds(0.02f);
        }
    }


    public void Expand(Tile tileToExpand)
    {
        List<Tile> localTileList = new List<Tile>();
        Tile[,] localTileArray = new Tile[,]{};
        
        if (grid1.Contains(tileToExpand))
        {
            localTileList = grid1;
            localTileArray = grid1Array;
        }
        else if (grid2.Contains(tileToExpand))
        {
            localTileList = grid2;
            localTileArray = grid2Array;
        }

        int tileIndexInList = localTileList.IndexOf(tileToExpand);
        Vector2Int tilePos = new Vector2Int(tileIndexInList % width, Mathf.FloorToInt(tileIndexInList / width));

        if (tilePos.x - 1 >= 0 && tilePos.y - 1 >= 0 && localTileArray[tilePos.x - 1, tilePos.y - 1].State == Tile.TileState.Hidden)
            localTileArray[tilePos.x - 1, tilePos.y - 1].Discover();
        if (tilePos.y - 1 >= 0 && localTileArray[tilePos.x, tilePos.y - 1].State == Tile.TileState.Hidden)
            localTileArray[tilePos.x , tilePos.y - 1].Discover();
        if (tilePos.x + 1 <= width - 1 && tilePos.y - 1 >= 0 && localTileArray[tilePos.x + 1, tilePos.y - 1].State == Tile.TileState.Hidden)
            localTileArray[tilePos.x + 1, tilePos.y - 1].Discover();
        
        if (tilePos.x - 1 >= 0 && localTileArray[tilePos.x - 1, tilePos.y].State == Tile.TileState.Hidden)
            localTileArray[tilePos.x - 1, tilePos.y].Discover();
        if (tilePos.x + 1 <= width - 1 && localTileArray[tilePos.x + 1, tilePos.y].State == Tile.TileState.Hidden)
            localTileArray[tilePos.x + 1, tilePos.y].Discover();
        
        if (tilePos.x - 1 >= 0 && tilePos.y + 1 <= height - 1 && localTileArray[tilePos.x - 1, tilePos.y + 1].State == Tile.TileState.Hidden)
            localTileArray[tilePos.x - 1, tilePos.y + 1].Discover();
        if (tilePos.y + 1 <= height - 1 && localTileArray[tilePos.x, tilePos.y + 1].State == Tile.TileState.Hidden)
            localTileArray[tilePos.x, tilePos.y + 1].Discover();
        if (tilePos.x + 1 <= width - 1 && tilePos.y + 1 <= height - 1 && localTileArray[tilePos.x + 1, tilePos.y + 1].State == Tile.TileState.Hidden)
            localTileArray[tilePos.x + 1, tilePos.y + 1].Discover();
    }
    
}