using HomaGames.Internal.DataBank;
using HomaGames.Internal.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ContextObjects
{
    [MenuItem("GameObject/IRL_Team/Simple Button", false, 15)]
    static void AddSimpleButton()
    {
        Object newObject = PrefabUtility.InstantiatePrefab(ServiceProvider<ContextObjectsData>.Value.simpleButtonPrefab);
        GameObject gameObject = newObject as GameObject;
        SetNewObject(gameObject);

        gameObject.GetComponent<RectTransform>().SetAnchor(AnchorPresets.MiddleCenter);
        gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        gameObject.transform.localScale = Vector3.one;
    }

    [MenuItem("GameObject/IRL_Team/Shiny Button", false, 15)]
    static void AddShinyButton()
    {
        Object newObject = PrefabUtility.InstantiatePrefab(ServiceProvider<ContextObjectsData>.Value.shinyButtonPrefab);
        GameObject gameObject = newObject as GameObject;
        SetNewObject(gameObject);

        gameObject.GetComponent<RectTransform>().SetAnchor(AnchorPresets.MiddleCenter);
        gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        gameObject.transform.localScale = Vector3.one;
    }

    [MenuItem("GameObject/IRL_Team/Fullscreen Button", false, 15)]
    static void AddFullscreenButton()
    {
        Object newObject = PrefabUtility.InstantiatePrefab(ServiceProvider<ContextObjectsData>.Value.fullscreenButtonPrefab);
        GameObject gameObject = newObject as GameObject;
        SetNewObject(gameObject);

        gameObject.GetComponent<RectTransform>().SetAnchor(AnchorPresets.StretchAll);
        gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
        gameObject.transform.localScale = Vector3.one;
    }

    private static void SetNewObject(GameObject newObject)
    {
        Undo.RegisterCreatedObjectUndo(newObject, "Create Shiny Button");

        if (Selection.activeTransform)
        {
            newObject.transform.SetParent(Selection.activeTransform);
        }
        newObject.transform.SetAsLastSibling();
        Selection.SetActiveObjectWithContext(newObject, newObject);
    }
}
