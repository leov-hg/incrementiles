using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public SO_GridManager gridManagerReference;
    
    public List<Tile> tiles;
    public int width;
    public int height;

    private Tile[,] tilesArray;

    private void Awake()
    {
        gridManagerReference.gridManager = this;
        
        tilesArray = new Tile[width,height];
        
        int index = 0;
        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
            {
                tilesArray[i, j] = tiles[index];
                index++;
            }
        }
        
        print(tiles.IndexOf(tiles[0]));
        print(tiles.IndexOf(tiles[11]));
    }

    private void Start()
    {
        StartCoroutine(RevealLineCoroutine(0));
    }

    public bool AreNeighbors(Tile tile1, Tile tile2)
    {
        bool res = false;

        int tile1IndexInList = tiles.IndexOf(tile1);
        Vector2 tile1Pos = new Vector2Int(tile1IndexInList % width, Mathf.FloorToInt(tile1IndexInList / width));
        
        int tile2IndexInList = tiles.IndexOf(tile2);
        Vector2 tile2Pos = new Vector2Int(tile2IndexInList % width, Mathf.FloorToInt(tile2IndexInList / width));

        return Vector2.Distance(tile1Pos, tile2Pos) == 1;
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
            tilesArray[i,lineIndex].Discover();
            yield return new WaitForSeconds(0.02f);
        }
    }

    private IEnumerator RevealCoroutine()
    {
        foreach (Tile tile in tiles)
        {
            tile.Discover();
            yield return new WaitForSeconds(0.02f);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Expand(tiles[14]);
        }   
    }


    public void Expand(Tile tileToExpand)
    {
        int tileIndexInList = tiles.IndexOf(tileToExpand);
        Vector2Int tilePos = new Vector2Int(tileIndexInList % width, Mathf.FloorToInt(tileIndexInList / width));
        print(tileToExpand.name);
        print(tileIndexInList);
        print(tilePos);
        
        if (tilePos.x - 1 >= 0 && tilePos.y - 1 >= 0 && tilesArray[tilePos.x - 1, tilePos.y - 1].State == Tile.TileState.Hidden)
            tilesArray[tilePos.x - 1, tilePos.y - 1].Discover();
        if (tilePos.y - 1 >= 0 && tilesArray[tilePos.x, tilePos.y - 1].State == Tile.TileState.Hidden)
            tilesArray[tilePos.x , tilePos.y - 1].Discover();
        if (tilePos.x + 1 <= width - 1 && tilePos.y - 1 >= 0 && tilesArray[tilePos.x + 1, tilePos.y - 1].State == Tile.TileState.Hidden)
            tilesArray[tilePos.x + 1, tilePos.y - 1].Discover();
        
        if (tilePos.x - 1 >= 0 && tilesArray[tilePos.x - 1, tilePos.y].State == Tile.TileState.Hidden)
            tilesArray[tilePos.x - 1, tilePos.y].Discover();
        if (tilePos.x + 1 <= width - 1 && tilesArray[tilePos.x + 1, tilePos.y].State == Tile.TileState.Hidden)
            tilesArray[tilePos.x + 1, tilePos.y].Discover();
        
        if (tilePos.x - 1 >= 0 && tilePos.y + 1 <= height - 1 && tilesArray[tilePos.x - 1, tilePos.y + 1].State == Tile.TileState.Hidden)
            tilesArray[tilePos.x - 1, tilePos.y + 1].Discover();
        if (tilePos.y + 1 <= height - 1 && tilesArray[tilePos.x, tilePos.y + 1].State == Tile.TileState.Hidden)
            tilesArray[tilePos.x, tilePos.y + 1].Discover();
        if (tilePos.x + 1 <= width - 1 && tilePos.y + 1 <= height - 1 && tilesArray[tilePos.x + 1, tilePos.y + 1].State == Tile.TileState.Hidden)
            tilesArray[tilePos.x + 1, tilePos.y + 1].Discover();
    }
    
}