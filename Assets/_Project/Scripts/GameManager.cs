using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public GameObject levelsParent;
    
    public GameObject menuCanvas;
    public GameObject gameCanvas;
    public GameObject winCanvas;

    [SerializeField]private List<GridManager> _levels;

    private void Awake()
    {
        menuCanvas.SetActive(false);
        gameCanvas.SetActive(false);
        winCanvas.SetActive(false);

        foreach (GridManager gridManager in levelsParent.GetComponentsInChildren<GridManager>().ToList())
        {
            _levels.Add(gridManager);
            gridManager.gameObject.SetActive(false);
        }
    }

    public void OnMenuEnter()
    {
        CameraController.Instance.SetCamera(0);
        menuCanvas.SetActive(true);
        SpawnLevel();
    }

    public void OnMenuExit()
    {
        menuCanvas.SetActive(false);
    }

    private void SpawnLevel()
    {
        int levelId = PlayerPrefs.GetInt("LevelId", 0);

        if (levelId <= _levels.Count - 1)
        {
            _levels[levelId].gameObject.SetActive(true);
        }
        else
        {
            _levels[Random.Range(0, _levels.Count)].gameObject.SetActive(true);
        }
    }
    
    
    
    
    
    
    
    

    public void OnGameEnter()
    {
        CameraController.Instance.SetCamera(1);
        gameCanvas.SetActive(true);
    }

    public void OnGameExit()
    {
        gameCanvas.SetActive(false);
    }

    public void OnWinEnter()
    {
        CameraController.Instance.SetCamera(2);
        winCanvas.SetActive(true);
    }

    public void OnWinExit()
    {
        winCanvas.SetActive(false);

        int LevelId = PlayerPrefs.GetInt("LevelId", 0);
        _levels[LevelId].gameObject.SetActive(false);
        PlayerPrefs.SetInt("LevelId", LevelId + 1);
    }

    public void OnFailEnter()
    {
        CameraController.Instance.SetCamera(3);
    }

    public void OnFailExit()
    {
        
    }
}
