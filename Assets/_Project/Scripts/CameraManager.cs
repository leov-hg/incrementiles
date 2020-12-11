using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using HomaGames.Internal.DataBank.BasicTypes;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public SO_GridManager gridManager;

    public IntData currentGridIndex;

    public void MoveToNewGrid()
    {
        StartCoroutine(MoveToNewGridCoroutine());
    }

    private IEnumerator MoveToNewGridCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        transform.DOMove(gridManager.gridManager.GetCurrentGrid().transform.position + new Vector3(0, 10, 0), 2f);
    }
}