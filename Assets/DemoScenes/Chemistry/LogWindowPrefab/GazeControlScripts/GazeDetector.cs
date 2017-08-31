using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GazeDetector : Singleton<GazeDetector>
{
    public delegate void FocusedChangedDelegate(GameObject previousObject, GameObject newObject);
    public event FocusedChangedDelegate FocusedObjectChanged;

    private float MaxGazeCollisionDistance = 10.0f;

    private RaycastHit hitInfo;
    public RaycastHit HitInfo
    {
        get { return hitInfo; }
    }

    public float LastHitDistance { get; private set; }
    public Vector3 HitPosition { get; private set; }
    public GameObject HitObject { get; private set; }
    public bool IsGazingAtObject { get; private set; }

    public PointerEventData UnityUIPointerEvent { get; private set; }

    public Vector3 GazeOrigin { get; private set; }
    public Vector3 GazeNormal { get; private set; }
    public Transform GazeTransform { get; set; }

    public LayerMask[] RaycastLayerMasks = new LayerMask[] { Physics.DefaultRaycastLayers };
    private List<RaycastResult> rayCastResults = new List<RaycastResult>();

    protected override void Awake()
    {
        base.Awake();
        if (RaycastLayerMasks == null || RaycastLayerMasks.Length == 0)
        {
            RaycastLayerMasks = new LayerMask[] { Physics.DefaultRaycastLayers };
        }
    }

    // Use this for initialization
    private void Start()
    {
        FocusedObjectChanged += (a, b) => { Debug.Log("Focused object changed"); };

        if (GazeTransform == null)
            if (Camera.main != null)
                GazeTransform = Camera.main.transform;
            else
                Debug.LogError("Gaze Manager was not given a GazeTransform and no main camera exists to default to.");
    }

    // Update is called once per frame
    private void Update()
    {
        if (GazeTransform == null)
        {
            return;
        }

        UpdateGazeInfo();
        
        GameObject previousFocusObject = RayCastPhysics();
        
        if (EventSystem.current != null)
            RayCastUnityUI();
        
        if (previousFocusObject != HitObject && FocusedObjectChanged != null)
            FocusedObjectChanged(previousFocusObject, HitObject);
    }

    private void UpdateGazeInfo()
    {
        GazeOrigin = GazeTransform.position;
        GazeNormal = GazeTransform.forward;
    }

    private GameObject RayCastPhysics()
    {
        GameObject previousFocusObject = HitObject;

        // If there is only one priority, don't prioritize
        if (RaycastLayerMasks.Length == 1)
            IsGazingAtObject = Physics.Raycast(GazeOrigin, GazeNormal, out hitInfo, MaxGazeCollisionDistance, RaycastLayerMasks[0]);
        else
        {
            // Raycast across all layers and prioritize
            RaycastHit? hit = PrioritizeHits(Physics.RaycastAll(new Ray(GazeOrigin, GazeNormal), MaxGazeCollisionDistance, -1));
            
            if (IsGazingAtObject = hit.HasValue)
                hitInfo = hit.Value;
        }

        if (IsGazingAtObject)
        {
            Debug.Log("Physics object has been detected!");
            HitObject = HitInfo.collider.gameObject;
            HitPosition = HitInfo.point;
            LastHitDistance = HitInfo.distance;
        }
        else
        {
            HitObject = null;
            HitPosition = GazeOrigin + (GazeNormal * LastHitDistance);
        }

        return previousFocusObject;
    }

    private void RayCastUnityUI()
    {
        if (UnityUIPointerEvent == null)
        {
            UnityUIPointerEvent = new PointerEventData(EventSystem.current);
        }

        // 2D cursor position
        Vector2 cursorScreenPos = Camera.main.WorldToScreenPoint(HitPosition);
        UnityUIPointerEvent.delta = cursorScreenPos - UnityUIPointerEvent.position;
        UnityUIPointerEvent.position = cursorScreenPos;

        // Graphics raycast
        rayCastResults.Clear();
        EventSystem.current.RaycastAll(UnityUIPointerEvent, rayCastResults);
        RaycastResult uiRaycastResult = FindClosestRaycastHitInLayermasks(rayCastResults, RaycastLayerMasks);
        UnityUIPointerEvent.pointerCurrentRaycast = uiRaycastResult;

        // If we have a raycast result, check if we need to overwrite the 3D raycast info
        if (uiRaycastResult.gameObject != null)
        {
            // Add the near clip distance since this is where the raycast is from
            float uiRaycastDistance = uiRaycastResult.distance + Camera.main.nearClipPlane;

            bool superseded3DObject = false;
            if (IsGazingAtObject)
            {
                // Check layer prioritization
                if (RaycastLayerMasks.Length > 1)
                {
                    // Get the index in the prioritized layer masks
                    int uiLayerIndex = FindLayerListIndex(uiRaycastResult.gameObject.layer, RaycastLayerMasks);
                    int threeDLayerIndex = FindLayerListIndex(hitInfo.collider.gameObject.layer, RaycastLayerMasks);

                    if (threeDLayerIndex > uiLayerIndex)
                    {
                        superseded3DObject = true;
                    }
                    else if (threeDLayerIndex == uiLayerIndex)
                    {
                        if (hitInfo.distance > uiRaycastDistance)
                        {
                            superseded3DObject = true;
                        }
                    }
                }
                else
                {
                    if (hitInfo.distance > uiRaycastDistance)
                    {
                        superseded3DObject = true;
                    }
                }
            }
            
            if (!IsGazingAtObject || superseded3DObject)
            {
                Debug.Log("UI object has been detected!");

                IsGazingAtObject = true;
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(uiRaycastResult.screenPosition.x, uiRaycastResult.screenPosition.y, uiRaycastDistance));
                hitInfo = new RaycastHit()
                {
                    distance = uiRaycastDistance,
                    normal = -Camera.main.transform.forward,
                    point = worldPos
                };

                HitObject = uiRaycastResult.gameObject;
                HitPosition = HitInfo.point;
                LastHitDistance = HitInfo.distance;
            }
        }
    }
    private RaycastHit? PrioritizeHits(RaycastHit[] hits)
    {
        if (hits.Length == 0)
            return null;
        
        for (int layerMaskIdx = 0; layerMaskIdx < RaycastLayerMasks.Length; layerMaskIdx++)
        {
            RaycastHit? minHit = null;

            for (int hitIdx = 0; hitIdx < hits.Length; hitIdx++)
            {
                RaycastHit hit = hits[hitIdx];
                if (IsLayerInLayerMask(hit.transform.gameObject.layer, RaycastLayerMasks[layerMaskIdx]) &&
                    (minHit == null || hit.distance < minHit.Value.distance))
                    minHit = hit;
            }

            if (minHit != null)
                return minHit;
        }

        return null;
    }

    private bool IsLayerInLayerMask(int layer, int layerMask)
    {
        return ((1 << layer) & layerMask) != 0;
    }

    private RaycastResult FindClosestRaycastHitInLayermasks(List<RaycastResult> candidates, LayerMask[] layerMaskList)
    {
        int combinedLayerMask = 0;

        for (int i = 0; i < layerMaskList.Length; i++)
            combinedLayerMask = combinedLayerMask | layerMaskList[i].value;

        RaycastResult? minHit = null;
        for (var i = 0; i < candidates.Count; ++i)
        {
            if (candidates[i].gameObject == null || !IsLayerInLayerMask(candidates[i].gameObject.layer, combinedLayerMask))
                continue;
            if (minHit == null || candidates[i].distance < minHit.Value.distance)
                minHit = candidates[i];
        }

        return minHit ?? new RaycastResult();
    }

    private int FindLayerListIndex(int layer, LayerMask[] layerMaskList)
    {
        for (int i = 0; i < layerMaskList.Length; i++)
            if (IsLayerInLayerMask(layer, layerMaskList[i].value))
                return i;

        return -1;
    }
}