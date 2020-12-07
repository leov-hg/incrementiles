using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEditor;
using UnityEngine.EventSystems;

public class ScrollManager : MonoBehaviour
{
    #region Singleton
    private static ScrollManager _instance;
    public static ScrollManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("ScrollManager");
                go.AddComponent<ScrollManager>();
            }
            return _instance;
        }
    }
    #endregion

    [SerializeField] private Camera camComponent;
    private Transform t;
    private Rigidbody rb;
    private new Animation animation;

    [Header("Zoom Variables")]
    [SerializeField] private float zoomSmoothSpeed = 1;
    [SerializeField] private float zoomStrenght = 1;
    public float zoomMax = 5;
    public float zoomMin = 10;
    [SerializeField] private float zoomLimitsSmoothSize = 1;
    private Vector2 touch0PrevPos;
    private Vector2 touch1PrevPos;
    private float prevMagnitude;
    private float currentMagnitude;
    private Touch touch0;
    private Touch touch1;
    private float difference;
    private float zoomTarget;
    private float actualZoom;

    [Header("Pan Variables")]
    [SerializeField] private float minimumDistanceToPan = 1;
    private Vector3 beforeTouchStart;
    private float beforeDistance;
    private Vector3 touchStart;
    private Vector3 direction = Vector3.zero;
    public float smoothSpeed = 1;

    [Header("Bool Checks")]
    public bool playerControl = true;
    public bool isScrolling = false;
    public bool isZooming = false;
    private bool canScroll = false;
    private bool checkBeforeStart = false;
    private bool twoTouches = false;

    private void Awake()
    {
        //Singleton
        _instance = this;

        //Event Subscribing

        t = transform;
        rb = GetComponent<Rigidbody>();
        animation = GetComponent<Animation>();

        actualZoom = zoomTarget = camComponent.orthographicSize;
    }

    private void LateUpdate()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject())
        {
            playerControl = false;
        }

        if (Input.GetMouseButtonUp(0))
        {
            playerControl = true;
        }

        #region Pan
        if (playerControl)
        {
            if (Input.GetMouseButtonDown(0))
            {
                beforeTouchStart = camComponent.ScreenToWorldPoint(Input.mousePosition);
            }

            if (Input.GetMouseButton(0) && !canScroll && checkBeforeStart)
            {
                beforeDistance = Vector3.Distance(beforeTouchStart, camComponent.ScreenToWorldPoint(Input.mousePosition));

                if (beforeDistance >= minimumDistanceToPan)
                {
                    touchStart = camComponent.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y * 3));

                    isScrolling = true;
                    canScroll = true;
                }
            }
        }
        if (Input.touchCount <= 1 && Input.GetMouseButton(0) && canScroll)
        {
            direction = touchStart - camComponent.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y * 3));
            rb.velocity = direction * smoothSpeed;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isScrolling = false;
            canScroll = false;
        }

        if (Input.touchCount == 0)
        {
            checkBeforeStart = true;
        }

        #endregion

        #region Zoom
        if (Input.touchCount == 2)
        {
            isZooming = true;
            canScroll = false;
            twoTouches = true;
            checkBeforeStart = false;

            touch0 = Input.GetTouch(0);
            touch1 = Input.GetTouch(1);

            touch0PrevPos = touch0.position - touch0.deltaPosition;
            touch1PrevPos = touch1.position - touch1.deltaPosition;

            prevMagnitude = (touch0PrevPos - touch1PrevPos).magnitude;
            currentMagnitude = (touch0.position - touch1.position).magnitude;

            difference = currentMagnitude - prevMagnitude;

            zoomTarget -= difference * zoomStrenght;
            zoomTarget = Mathf.Clamp(zoomTarget, zoomMax, zoomMin);

            if (zoomTarget >= zoomMin - zoomLimitsSmoothSize)
            {
                // zoomStrenght *= zoomMin - zoomTarget;

                //zoomTarget = zoomMin - zoomLimitsSmoothSize;
            }
            else if (zoomTarget <= zoomMax + zoomLimitsSmoothSize)
            {
                //zoomStrenght *= zoomTarget - zoomMax;
            }
        }
        else
        {
            if (zoomTarget >= zoomMin - zoomLimitsSmoothSize)
            {
                zoomTarget = zoomMin - zoomLimitsSmoothSize;
            }
            else if (zoomTarget <= zoomMax + zoomLimitsSmoothSize)
            {
                zoomTarget = zoomMax + zoomLimitsSmoothSize;
            }

            isZooming = false;
            twoTouches = false;
        }
        //For Editor
        zoomTarget -= Input.GetAxis("Mouse ScrollWheel") * 10;

        actualZoom = Mathf.SmoothStep(actualZoom, zoomTarget, Time.deltaTime * zoomSmoothSpeed);
        camComponent.orthographicSize = actualZoom;
        #endregion
#else
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                {
                    playerControl = false;
                }
            }
        }

        if(Input.GetMouseButtonUp(0))
        {
            playerControl = true;
        }

        #region Pan
            if (playerControl)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    beforeTouchStart = camComponent.ScreenToWorldPoint(Input.mousePosition);
                }

                if (Input.GetMouseButton(0) && !canScroll && checkBeforeStart)
                {
                    beforeDistance = Vector3.Distance(beforeTouchStart, camComponent.ScreenToWorldPoint(Input.mousePosition));

                    if (beforeDistance >= minimumDistanceToPan)
                    {
                        touchStart = camComponent.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y * 3));

                        isScrolling = true;
                        canScroll = true;
                    }
                }
            }

            if (Input.touchCount <= 1 && Input.GetMouseButton(0) && canScroll)
            {
                direction = touchStart - camComponent.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y * 3));
                rb.velocity = direction * smoothSpeed;
            }

            if (Input.GetMouseButtonUp(0))
            {
                isScrolling = false;
                canScroll = false;
            }

            if (Input.touchCount == 0)
            {
                checkBeforeStart = true;
            }

        #endregion

        #region Zoom
            if (Input.touchCount == 2)
            {
                isZooming = true;
                canScroll = false;
                twoTouches = true;
                checkBeforeStart = false;

                touch0 = Input.GetTouch(0);
                touch1 = Input.GetTouch(1);

                touch0PrevPos = touch0.position - touch0.deltaPosition;
                touch1PrevPos = touch1.position - touch1.deltaPosition;

                prevMagnitude = (touch0PrevPos - touch1PrevPos).magnitude;
                currentMagnitude = (touch0.position - touch1.position).magnitude;

                difference = currentMagnitude - prevMagnitude;

                zoomTarget -= difference * zoomStrenght;
                zoomTarget = Mathf.Clamp(zoomTarget, zoomMax, zoomMin);

                if (zoomTarget >= zoomMin - zoomLimitsSmoothSize)
                {
                    // zoomStrenght *= zoomMin - zoomTarget;

                    //zoomTarget = zoomMin - zoomLimitsSmoothSize;
                }
                else if (zoomTarget <= zoomMax + zoomLimitsSmoothSize)
                {
                    //zoomStrenght *= zoomTarget - zoomMax;
                }
            }
            else
            {
                if (zoomTarget >= zoomMin - zoomLimitsSmoothSize)
                {
                    zoomTarget = zoomMin - zoomLimitsSmoothSize;
                }
                else if (zoomTarget <= zoomMax + zoomLimitsSmoothSize)
                {
                    zoomTarget = zoomMax + zoomLimitsSmoothSize;
                }

                isZooming = false;
                twoTouches = false;
            }
            //For Editor
            zoomTarget -= Input.GetAxis("Mouse ScrollWheel");

            actualZoom = Mathf.SmoothStep(actualZoom, zoomTarget, Time.deltaTime * zoomSmoothSpeed);
            camComponent.orthographicSize = actualZoom;
        #endregion
#endif
    }

    public void GoToSnap(Vector3 targetPos, float timeToSnap = 1)
    {
        StartCoroutine(Snap(targetPos, timeToSnap));
    }
    private IEnumerator Snap(Vector3 targetPos, float timeToSnap)
    {
        Vector3 beforePos = t.position;
        float currentSnapTime = 0;

        while (currentSnapTime < timeToSnap)
        {
            currentSnapTime += Time.deltaTime;
            t.position = Vector3.Lerp(beforePos, new Vector3(targetPos.x, t.position.y, targetPos.z), (currentSnapTime / timeToSnap) * (currentSnapTime / timeToSnap) * (3f - 2f * (currentSnapTime / timeToSnap)));

            yield return null;
        }
    }

    public void GoToZoom(float zoomTarget, float timeToZoom = 1)
    {
        this.zoomTarget = zoomTarget;
    }

    public void EndLevel()
    {
        animation.Play();
    }

    public void StartLevel()
    {
        camComponent.orthographicSize = zoomMin;
    }
}