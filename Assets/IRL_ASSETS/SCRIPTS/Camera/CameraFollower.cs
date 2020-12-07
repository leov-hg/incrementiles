using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    #region Singleton
    public static CameraFollower I;
    #endregion

    [System.Flags]
    public enum Axes : int
    {
        None = 0x00,
        X = 0x01,
        Y = 0x02,
        Z = 0x04
    }

    #region Configuration Variables
    [SerializeField] private bool onAwake = false;

    [Separator("Components References", true)]
    [SerializeField] private List<Transform> targets = new List<Transform>();
    [SerializeField] private Camera cam;

    [Separator("Movements Settings", true)]
    [EnumFlags("Moving Axes")] public Axes movingAxes = Axes.X | Axes.Y | Axes.Z;
    [SerializeField] public Vector3 offset;
    [SerializeField] public float smoothSpeed = 1;

    [Separator("Rotations Settings", true)]
    [SerializeField] private bool camLookingTarget = false;
    [ConditionalField(nameof(camLookingTarget))] [SerializeField] private Vector3 lookingOffset;
    [ConditionalField(nameof(camLookingTarget))] [SerializeField] private float lookingSmoothSpeed = 1;
    [Space(10)]
    [SerializeField] private bool canRotateX = false;
    [SerializeField] private bool canRotateY = false;
    [SerializeField] private bool canRotateZ = false;
    [ConditionalField(nameof(canRotateX))] [SerializeField] private float angleX = 0;
    [ConditionalField(nameof(canRotateY))] [SerializeField] private float angleY = 0;
    [ConditionalField(nameof(canRotateZ))] [SerializeField] private float angleZ = 0;

    [Separator("Zoom Settings", true)]
    [SerializeField] private bool zoomFollow = false;
    [ConditionalField(nameof(zoomFollow))] [SerializeField] private bool scaleFollow = false;
    [ConditionalField(nameof(zoomFollow))] [SerializeField] private float minZoom = 1;
    [ConditionalField(nameof(zoomFollow))] [SerializeField] private float maxZoom = 1;
    [ConditionalField(nameof(zoomFollow))] [SerializeField] private float zoomLimiter = 50;
    [ConditionalField(nameof(zoomFollow))] [SerializeField] private float zoomSmoothSpeed = 1;
    #endregion

    #region Private Variables
    private bool follow = false;
    private Vector3 centerPoint;
    private bool fixedZoomActive = false;
    private float fixedZoom;
    #endregion

    #region BookKeeping Variables
    private float initialOffsetZ;
    private float initialPosY;
    private Vector3 targetPoint;
    private float initialFov;

    private Vector3 targetA;
    private Vector3 targetB;
    #endregion

    private void Awake()
    {
        #region Singleton
        I = this;
        #endregion

        initialOffsetZ = offset.z;
        initialPosY = transform.position.y;
        initialFov = cam.fieldOfView;

        if (onAwake)
        {
            StartFollow();
        }
    }

    public void AddTargets(Transform target, bool overrideSame = false)
    {
        if (overrideSame)
        {
            if(!targets.Contains(target))
                targets.Add(target);
        }
        else
        {
            targets.Add(target);
        }
    }
    public void AddTargets(Transform[] target, bool overrideSame = false)
    {
        this.targets.AddRange(targets);
    }

    public void RemoveTargets(Transform target)
    {
        targets.Remove(target);
    }
    public void RemoveTargets(Transform[] targets)
    {
        for (int i = 0; i < targets.Length; i++)
        {
            this.targets.Remove(targets[i]);
        }
    }

    public void SetTargets(Transform target)
    {
        this.targets.Clear();
        this.targets.Add(target);
    }
    public void SetTargets(Transform[] targets)
    {
        this.targets.Clear();
        for (int i = 0; i < targets.Length; i++)
        {
            this.targets.Add(targets[i]);
        }
    }

    public void SetFixedZoom(float value)
    {
        fixedZoomActive = true;
        fixedZoom = value;
    }

    public void StartFollow(float delay = 0)
    {
        StartCoroutine(FollowStateDelay(delay, true));
    }
    public void StopFollow(float delay = 0)
    {
        StartCoroutine(FollowStateDelay(delay, false));
    }

    private IEnumerator FollowStateDelay(float delay, bool state)
    {
        yield return new WaitForSeconds(delay);

        follow = state;
    }

    [ButtonMethod]
    private void GetCurrentOffset()
    {
        centerPoint = Vector3.zero;
        int count = 0;
        for (int i = 0; i < targets.Count; i++)
        {
            centerPoint += targets[i].transform.position;
            count++;
        }

        centerPoint /= count;

        if (targets.Count > 0)
        {
            offset = transform.position - centerPoint;
        }
        else
        {
            offset = transform.position;
        }
    }

    public void SetPoint(Vector3 targetPos)
    {
        targetPoint = targetPos;
    }
    public void SetInstantPoint(Vector3 targetPos)
    {
        transform.position = new Vector3(targetPos.x + offset.x, targetPos.y + offset.y, targetPos.z + offset.z);
    }

    private void LateUpdate()
    {
        if (targets.Count == 0 || !follow)
        {
            return;
        }

        Move();
        Rotating();

        if (zoomFollow)
        {
            Zoom();
        }
    }

    private void Zoom()
    {
        float averageScale = 0;
        for (int i = 0; i < targets.Count; i++)
        {
            averageScale += targets[i].lossyScale.magnitude;
        }

        float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance() * averageScale / zoomLimiter);

        //cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime * zoomSmoothSpeed);

        cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, -cam.transform.forward * (fixedZoomActive ? fixedZoom : newZoom), Time.deltaTime * zoomSmoothSpeed);
    }

    private void Move()
    {
        int x = -1;
        int y = -1;
        int z = -1;
        if ((movingAxes & Axes.X) != Axes.None)
        {
            x = 1;
        }
        if ((movingAxes & Axes.Y) != Axes.None)
        {
            y = 1;
        }
        if ((movingAxes & Axes.Z) != Axes.None)
        {
            z = 1;
        }

        centerPoint = GetCenterPoint();

        transform.position = Vector3.Lerp(transform.position, new Vector3(x > 0 ? centerPoint.x + offset.x : transform.position.x,
            y > 0 ? centerPoint.y + offset.y : transform.position.y,
            z > 0 ? centerPoint.z + offset.z : transform.position.z), Time.deltaTime * smoothSpeed);
    }

    private void Rotating()
    {
        if (camLookingTarget)
        {
            Vector3 target = (centerPoint - cam.transform.position).normalized;
            cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, Quaternion.LookRotation(target), Time.deltaTime * lookingSmoothSpeed);
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(canRotateX ? angleX : 0, canRotateY ? angleY : 0, canRotateZ ? angleZ : 0), Time.deltaTime * smoothSpeed);
        }
    }

    private float GetGreatestDistance()
    {
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }

        return bounds.size.x;
    }

    private Vector3 GetCenterPoint()
    {
        if (targets.Count == 1)
        {
            return targets[0].position;
        }

        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }

        return bounds.center;
    }
}
