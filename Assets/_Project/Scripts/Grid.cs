using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using Event = HomaGames.Internal.DataBank.Event;

public class Grid : MonoBehaviour
{
    public Event OnGridComplete;

    public SO_Car carRef;
    public int width;
    public GameObject confettis;
    public Transform checkPoint;
    public Color grayOutColor;

    private int nbDiscoveredTiles;
    
    
    private int _height;

    private List<Tile> _tilesList;
    private Tile[,] _tilesArray;

    private List<Tile> _newExpandedTiles = new List<Tile>();
    private float colorLerpValue;
    [SerializeField] private List<Tile> _roadTiles = new List<Tile>();

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

    public void OnTileDiscover()
    {
        nbDiscoveredTiles++;
        if (nbDiscoveredTiles >= _tilesList.Count)
        {
            confettis.SetActive(true);
            
            //Gray out all tile and State = hidden
            foreach (Tile tile in _tilesList)
            {
                if (tile.Type != Tile.TileType.Road)
                {
                    tile.State = Tile.TileState.Hidden;
                    tile._currentDisplayModelMesh.material.DOColor(grayOutColor, 0.1f);
                }

                tile._tileBaseMeshRenderer.material.DOColor(grayOutColor, 0.1f);
            }
            
            
            OnGridComplete?.Invoke();
        }
    }

    private List<Tile> GetAllRoadTiles()
    {
        List<Tile> res = new List<Tile>();
        
        foreach (Tile tile in _tilesList)
        {
            if (tile.Type == Tile.TileType.Road)
            {
                res.Add(tile);
            }
        }

        return res;
    }

    private void Awake()
    {
        _tilesList = GetComponentsInChildren<Tile>().ToList();
        
        _height = _tilesList.Count / width;
        
        _tilesArray = new Tile[width,_height];

        int index = 0;
        for (int j = 0; j < _height; j++)
        {
            for (int i = 0; i < width; i++)
            {
                _tilesArray[i, j] = _tilesList[index];
                _tilesList[index].OnDiscover += OnTileDiscover;
                index++;
            }
        }

        _roadTiles = GetAllRoadTiles();
    }

    public void MoveCarToFurthestRoad()
    {

        int i = 0;
        while (_roadTiles[i].State == Tile.TileState.Discovered)
        {
            i++;
            if (i == _roadTiles.Count)
                break;
        }
        
        carRef.car.MoveTo(_roadTiles[i - 1].transform.position);
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

    public void StopPrevisualisation()
    {
        foreach (Tile tile in _newExpandedTiles)
        {
            tile.StopPrevisualisation();
        }
        _newExpandedTiles = new List<Tile>();
    }

    public void StartPrevisualisation()
    {
        foreach (Tile tile in _newExpandedTiles)
            tile.StartPrevisualisation();
    }

    public void Expand(List<Tile> tilesToExpand, int comboAmount)
    {
        _newExpandedTiles = new  List<Tile>();
        foreach (Tile tile in tilesToExpand)
        {
            if (!_tilesList.Contains(tile)) return;
            
            
            int tileIndexInList = TilesList.IndexOf(tile);
            Vector2Int tilePos = new Vector2Int(tileIndexInList % width, Mathf.FloorToInt(tileIndexInList / width));

            int expandAmount = Mathf.Clamp(Mathf.FloorToInt(comboAmount / 2), 1, 9999);

            for (int i = 1; i <= expandAmount; i++)
            {
                //if (tilePos.x - 1 >= 0 && tilePos.y - 1 >= 0 && TilesArray[tilePos.x - 1, tilePos.y - 1].State == Tile.TileState.Hidden)
                //TilesArray[tilePos.x - 1, tilePos.y - 1].Discover();
                if (tilePos.y - i >= 0 && !_newExpandedTiles.Contains(TilesArray[tilePos.x , tilePos.y - i]))
                    _newExpandedTiles.Add(TilesArray[tilePos.x , tilePos.y - i]);
                //if (tilePos.x + 1 <= width - 1 && tilePos.y - 1 >= 0 && TilesArray[tilePos.x + 1, tilePos.y - 1].State == Tile.TileState.Hidden)
                //TilesArray[tilePos.x + 1, tilePos.y - 1].Discover();

                if (tilePos.x - i >= 0 && !_newExpandedTiles.Contains(TilesArray[tilePos.x - i, tilePos.y]))
                    _newExpandedTiles.Add(TilesArray[tilePos.x - i, tilePos.y]);
                if (tilePos.x + i <= width - 1 && !_newExpandedTiles.Contains(TilesArray[tilePos.x + i, tilePos.y]))
                    _newExpandedTiles.Add(TilesArray[tilePos.x + i, tilePos.y]);

                //if (tilePos.x - 1 >= 0 && tilePos.y + 1 <= height - 1 && TilesArray[tilePos.x - 1, tilePos.y + 1].State == Tile.TileState.Hidden)
                //TilesArray[tilePos.x - 1, tilePos.y + 1].Discover();
                if (tilePos.y + i <= _height - 1 && !_newExpandedTiles.Contains(TilesArray[tilePos.x, tilePos.y + i]))
                    _newExpandedTiles.Add(TilesArray[tilePos.x, tilePos.y + i]);
                //if (tilePos.x + 1 <= width - 1 && tilePos.y + 1 <= height - 1 && TilesArray[tilePos.x + 1, tilePos.y + 1].State == Tile.TileState.Hidden)
                //TilesArray[tilePos.x + 1, tilePos.y + 1].Discover();
            }
        }
        StartPrevisualisation();
    }

    public void ValidateExpand()
    {
        foreach (Tile tile in _newExpandedTiles)
        {
            if (tile.State == Tile.TileState.Hidden)
            {
                tile.Discover();
            }
        }
        
        StopPrevisualisation();
        MoveCarToFurthestRoad();
    }

    public void Reset()
    {
        confettis.SetActive(false);
        foreach (Tile tile in _tilesList)
        {
            tile.Hide();
            tile.spawnParticles.SetActive(false);
        }
        
        nbDiscoveredTiles = 0;
    }
}