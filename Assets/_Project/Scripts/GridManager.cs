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

    public bool AreNeighbors(Tile tile1, Tile tile2)
    {
        bool res = false;

        int tile1IndexInList = tiles.IndexOf(tile1);
        Vector2 tile1Pos = new Vector2(Mathf.FloorToInt(tile1IndexInList / width), tile1IndexInList % width);
        
        int tile2IndexInList = tiles.IndexOf(tile2);
        Vector2 tile2Pos = new Vector2(Mathf.FloorToInt(tile2IndexInList / width), tile2IndexInList % width);
        
        print(tile1IndexInList +"____" +tile2IndexInList);
        print(tile1Pos + " ____ " +tile2Pos);

        return Vector2.Distance(tile1Pos, tile2Pos) == 1;
    }

    [ButtonMethod]
    public void reveal()
    {
        StartCoroutine(revealCoroutine());
    }

    private IEnumerator revealCoroutine()
    {
        foreach (Tile tile in tiles)
        {
            tile.Discover();
            yield return new WaitForSeconds(0.02f);
        }
    }
    
    
}