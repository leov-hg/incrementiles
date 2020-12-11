using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HomaGames.Internal.DataBank.BasicTypes;
using HomaGames.Internal.GameState;
using MyBox;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public StateMachineDescription stateMachine;
    public State winState;
    
    public SO_GridManager gridManagerReference;
    public SO_Car car;
    public Color colorLight;
    public Color colorDark;
    public IntData currentGridIndex;

    private List<Grid> _grids;


    private void Awake()
    {
        gridManagerReference.gridManager = this;

        _grids = GetComponentsInChildren<Grid>().ToList();

        currentGridIndex.Value = 0;
    }

    private void Start()
    {
        RevealLineCoroutine(0);
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

    public void NextGrid()
    {
        if (currentGridIndex.Value < _grids.Count - 1)
        {
            currentGridIndex.Value++;
            car.car.MoveTo(_grids[currentGridIndex.Value].checkPoint.position);
            RevealLineCoroutine(0);
        }
        else
        {
            stateMachine.StartTransition(winState);
        }
    }

    public bool AreNeighbors(Tile tile1, Tile tile2)
    {
        bool res = false;
        
        if (_grids[currentGridIndex.Value].AreNeighbors(tile1, tile2))
        {
            res = true;
        }

        return res;
    }

    [ButtonMethod]
    /*public void Reveal()
    {
        StartCoroutine(RevealCoroutine());
    }*/

    private void RevealLineCoroutine(int lineIndex)
    {
        for (int i = 0; i < _grids[currentGridIndex.Value].width; i++)
        {
            _grids[currentGridIndex.Value].TilesArray[i,lineIndex].Discover();
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


    public void ValidateExpand()
    {
        _grids[currentGridIndex.Value].ValidateExpand();
    }

    public void StopPrevisualisation()
    {
        _grids[currentGridIndex.Value].StopPrevisualisation();
    }
    
    public void Expand(List<Tile> tilesToExpand, int comboAmount)
    {
        _grids[currentGridIndex.Value].Expand(tilesToExpand, comboAmount);
    }
}