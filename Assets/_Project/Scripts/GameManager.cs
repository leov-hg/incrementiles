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

    private GridManager _currentLevel;

    [SerializeField]private List<GridManager> _levels;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        
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
            _currentLevel = _levels[levelId];
        }
        else
        {
            int rdm = Random.Range(0, _levels.Count);
            _levels[rdm].gameObject.SetActive(true);
            _currentLevel = _levels[rdm];
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
        
        _currentLevel.gameObject.SetActive(false);
        PlayerPrefs.SetInt("LevelId", PlayerPrefs.GetInt("LevelId", 0) + 1);
    }

    public void OnFailEnter()
    {
        CameraController.Instance.SetCamera(3);
    }

    public void OnFailExit()
    {
        
    }
}
