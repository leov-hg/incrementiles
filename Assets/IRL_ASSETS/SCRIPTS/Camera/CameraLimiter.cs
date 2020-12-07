using HomaGames.Internal.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraLimiter : MonoBehaviour
{
    [SerializeField] private LayerMask waterPlaneMask;
    private RaycastHit hit;

    [SerializeField] private Transform[] frustumToWorld = new Transform[4];

    private void Update()
    {
        if (frustumToWorld.ContainsNull())
        {
            return;
        }

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.0f, 0.0f, 0));
        Ray ray2 = Camera.main.ViewportPointToRay(new Vector3(1.0f, 0.0f, 0));
        Ray ray3 = Camera.main.ViewportPointToRay(new Vector3(0.0f, 1.0f, 0));
        Ray ray4 = Camera.main.ViewportPointToRay(new Vector3(1.0f, 1.0f, 0));

        if (Physics.Raycast(ray, out hit, 1000, waterPlaneMask, QueryTriggerInteraction.UseGlobal))
        {
            frustumToWorld[0].position = hit.point;
        }
        if (Physics.Raycast(ray2, out hit, 1000, waterPlaneMask, QueryTriggerInteraction.UseGlobal))
        {
            frustumToWorld[1].position = hit.point;
        }
        if (Physics.Raycast(ray3, out hit, 1000, waterPlaneMask, QueryTriggerInteraction.UseGlobal))
        {
            frustumToWorld[2].position = hit.point;
        }
        if (Physics.Raycast(ray4, out hit, 1000, waterPlaneMask, QueryTriggerInteraction.UseGlobal))
        {
            frustumToWorld[3].position = hit.point;
        }
    }
}
