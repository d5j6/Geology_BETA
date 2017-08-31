using UnityEngine;
using System.Collections;

public enum UnderlyingState { Underlying, OnTop }
public enum LoadQueries { RightAndLeft, Outer, Inner }

public enum SlicingSphereState { Closed, Opening, Opened, Closing, Disappearing, Disappeared }

public class OptimizedSphereController1 : MonoBehaviour {

    float _layerBrightness = 1f;
    public float LayerBrightness
    {
        get
        {
            return _layerBrightness;
        }
        set
        {
            _layerBrightness = value;
            if (outerSphere != null)
            {
                outerSphere.material.SetFloat("_Brightness", _layerBrightness);
            }
            if (innerSphere != null)
            {
                innerSphere.material.SetFloat("_Brightness", _layerBrightness);
            }
            if (leftSlice != null)
            {
                leftSlice.material.SetFloat("_Brightness", _layerBrightness);
            }
            if (rightSlice != null)
            {
                rightSlice.material.SetFloat("_Brightness", _layerBrightness);
            }
        }
    }

    float __darkSideBrightness = 0.32f;
    public float DarkSideBrightness
    {
        get
        {
            return __darkSideBrightness;
        }
        set
        {
            __darkSideBrightness = value;
            if (outerSphere != null)
            {
                outerSphere.material.SetFloat("_SecondTextureColorMultiplier", __darkSideBrightness);
            }
        }
    }

    public void SetSurfaceDarkSideBrighter()
    {

    }

    public bool closedOnStart = false;

    public UnderlyingState underlyingState = UnderlyingState.Underlying;
    public SlicingSphereState state = SlicingSphereState.Closed;

    private Vector3 currentTransformRotation = Vector3.zero;
    private float currentTextureRotation = 0f;

    private Transform _lookingAt;
    public Quaternion LookRotation;
    private Vector3 _direction;

    public float OpenedOnAnglePreview = 0;

    void Update()
    {
        OpenedOnAnglePreview = _openedOnAngle;
    }

    private float _openedOnAngle = 0;

    public string Prefix;

    private float _mySize;
    public float MySize
    {
        get { return _mySize; }
        set { _mySize = value; }
    }

    public Vector3 MyWorldPosition
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    public float OpenedOnAngle
    {
        get { return _openedOnAngle; }
        set
        {
            _openedOnAngle = value;
            if (outerSphere != null) outerSphere.material.SetFloat("_Closed", 1f - value);
            if (innerSphere != null) innerSphere.material.SetFloat("_Closed", 1f - value);
            if (leftSlice != null) leftSlice.material.SetFloat("_Closed", 1f - value);
            if (rightSlice != null) rightSlice.material.SetFloat("_Closed", 1f - value);
        }
    }

    private float _outerDiameter = 1f;
    public float OuterDiameter
    {
        get { return _outerDiameter; }
        set
        {
            _outerDiameter = value;
            if (outerSphere != null) outerSphere.material.SetFloat("_Diameter", value);
            if (leftSlice != null) leftSlice.material.SetFloat("_DiameterOuter", value);
            if (rightSlice != null) rightSlice.material.SetFloat("_DiameterOuter", value);
        }
    }
    private float _innerDiameter = 1f;
    public float InnerDiameter
    {
        get { return _innerDiameter; }
        set
        {
            _innerDiameter = value;
            if (innerSphere != null) innerSphere.material.SetFloat("_Diameter", value);
            if (leftSlice != null) leftSlice.material.SetFloat("_DiameterInner", value);
            if (rightSlice != null) rightSlice.material.SetFloat("_DiameterInner", value);
        }
    }

    public MeshRenderer outerSphere;
    public MeshRenderer innerSphere;
    public MeshRenderer leftSlice;
    public MeshRenderer rightSlice;

    bool outerEnabled = true;
    bool innerEnabled = true;
    bool leftEnabled = true;
    bool rightEnabled = true;

    System.Action callbackToUpperScript;
    System.Action callbackToUpperScriptMultiple;
    System.Action callbackWhenPlaneCreated;

    public void Hide()
    {
        outerEnabled = false;
        innerEnabled = false;
        leftEnabled = false;
        rightEnabled = false;

        if (outerSphere != null) outerSphere.enabled = false;
        if (innerSphere != null) innerSphere.enabled = false;
        if (leftSlice != null) leftSlice.enabled = false;
        if (rightSlice != null) rightSlice.enabled = false;
    }

    public void Show()
    {
        outerEnabled = true;
        innerEnabled = true;
        leftEnabled = true;
        rightEnabled = true;

        if (outerSphere != null) outerSphere.enabled = true;
        if (innerSphere != null) innerSphere.enabled = true;
        if (leftSlice != null) leftSlice.enabled = true;
        if (rightSlice != null) rightSlice.enabled = true;
    }

    public void LookAt(Transform targetTransform)
    {
        _lookingAt = targetTransform;
    }

    public void StopLookingAt()
    {
        _lookingAt = null;
    }

    public void rotate(float angle)
    {
        if (state == SlicingSphereState.Closed)
        {
            currentTransformRotation.y += angle * Time.deltaTime;
            transform.rotation = Quaternion.Euler(currentTransformRotation);
        }
        else
        {
            currentTextureRotation += angle * Time.deltaTime;
            if (outerSphere != null) outerSphere.material.SetFloat("_RotationAngle", currentTextureRotation);
            if (innerSphere != null) innerSphere.material.SetFloat("_RotationAngle", currentTextureRotation);
        }
    }

    public void DestroyAllPlanes()
    {
        if (innerSphere != null)
        {
            Destroy(innerSphere.gameObject);
            innerSphere = null;
        }
        if (leftSlice != null)
        {
            Destroy(leftSlice.gameObject);
            leftSlice = null;
        }
        if (rightSlice != null)
        {
            Destroy(rightSlice.gameObject);
            rightSlice = null;
        }
        if (outerSphere != null)
        {
            Destroy(outerSphere.gameObject);
            outerSphere = null;
        }
    }

    public void DestroyInnerPlane()
    {
        if (innerSphere != null)
        {
            Destroy(innerSphere.gameObject);
            innerSphere = null;
        }
    }

    public void goToState(SlicingSphereState newState)
    {
        switch (newState)
        {
            case SlicingSphereState.Closed:
                if (innerSphere != null)
                {
                    Destroy(innerSphere.gameObject);
                    innerSphere = null;
                }
                if (leftSlice != null)
                {
                    Destroy(leftSlice.gameObject);
                    leftSlice = null;
                }
                if (rightSlice != null)
                {
                    Destroy(rightSlice.gameObject);
                    rightSlice = null;
                }

                if (underlyingState == UnderlyingState.Underlying)
                {
                    if (outerSphere != null)
                    {
                        Debug.Log("Trying to destroy outer plane");
                        Destroy(outerSphere.gameObject);
                        outerSphere = null;
                    }
                }
                break;
            case SlicingSphereState.Opened:
                if (underlyingState == UnderlyingState.Underlying)
                {
                    if (innerSphere != null)
                    {
                        Destroy(innerSphere.gameObject);
                        innerSphere = null;
                    }
                    if (outerSphere != null)
                    {
                        Destroy(outerSphere.gameObject);
                        outerSphere = null;
                    }
                }
                break;
            case SlicingSphereState.Disappeared:
                DestroyAllPlanes();
                break;
        }

        state = newState;
    }

    public void disappear360(float time = 3.3f, LeanTweenType easeType = LeanTweenType.easeInOutQuad)
    {
        if ((state == SlicingSphereState.Closed) || (state == SlicingSphereState.Opened))
        {
            goToState(SlicingSphereState.Disappearing);

            float initialAngle;
            /*if (leftSlice != null)
            {
                initialAngle = leftSlice.material.GetFloat("_Closed");
            }
            else if (outerSphere != null)
            {
                initialAngle = outerSphere.material.GetFloat("_Closed");
            }
            else if (rightSlice != null)
            {
                initialAngle = rightSlice.material.GetFloat("_Closed");
            }
            else
            {
                initialAngle = innerSphere.material.GetFloat("_Closed");
            }*/

            initialAngle = OpenedOnAngle;

            float targetAngle = 1f;
            LeanTween.value(gameObject, initialAngle, targetAngle, time).setEase(easeType).setOnUpdate((float val) =>
            {
                OpenedOnAngle = val;
                /*if (outerSphere != null) outerSphere.material.SetFloat("_Closed", val);
                if (innerSphere != null) innerSphere.material.SetFloat("_Closed", val);
                if (leftSlice != null) leftSlice.material.SetFloat("_Closed", val);
                if (rightSlice != null) rightSlice.material.SetFloat("_Closed", val);

                if (_lookingAt != null)
                {
                    //currentTransformRotation.y -= ((360.0f - val * 360.0f) / 2f) * Time.deltaTime;
                    //transform.rotation = Quaternion.Euler(currentTransformRotation);
                }*/
            })
            .setOnComplete(() =>
            {
                Debug.Log("_openedOnAngle in the end of disappearing is: " + _openedOnAngle);

                leftSlice.enabled = false;
                rightSlice.enabled = false;
                goToState(SlicingSphereState.Disappeared);
            });
        }
    }

    public void openOnAngle(float angle, float time = 3.3f, LeanTweenType easeType = LeanTweenType.easeInOutQuad)
    {
        Debug.Log("Time of opening: " + time);
        if ((state == SlicingSphereState.Closed) || (state == SlicingSphereState.Disappeared) || (state == SlicingSphereState.Opened))
        {
            if (state == SlicingSphereState.Disappeared)
            {
                leftSlice.enabled = true;
                rightSlice.enabled = true;
            }

            goToState(SlicingSphereState.Opening);

            if (_lookingAt != null)
            {
                /*
                 * Если смотрим на что-то - поворачиваем все в нужную сторону и компенсируем поворот текстуры
                 */
                Vector3 to = _lookingAt.position;
                to.y = 0f;
                _direction = (to - transform.position).normalized;
                LookRotation = Quaternion.LookRotation(_direction);
                LookRotation = Quaternion.Euler(new Vector3(0f, LookRotation.eulerAngles.y - 90f - angle/2f, 0f));

                float diff = currentTransformRotation.y - LookRotation.eulerAngles.y;

                currentTransformRotation.y = LookRotation.eulerAngles.y;
                transform.rotation = LookRotation;
                currentTextureRotation += diff;
                if (outerSphere != null) outerSphere.material.SetFloat("_RotationAngle", currentTextureRotation);
                if (innerSphere != null) innerSphere.material.SetFloat("_RotationAngle", currentTextureRotation);
            }

            float initialAngle;
            /*if (leftSlice != null)
            {
                initialAngle = leftSlice.material.GetFloat("_Closed");
            }
            else if (outerSphere != null)
            {
                initialAngle = outerSphere.material.GetFloat("_Closed");
            }
            else if(rightSlice != null)
            {
                initialAngle = rightSlice.material.GetFloat("_Closed");
            }
            else if (innerSphere != null)
            {
                initialAngle = innerSphere.material.GetFloat("_Closed");
            }
            else
            {
                initialAngle = 1 - _openedOnAngle;
            }*/

            initialAngle = OpenedOnAngle;

            Debug.Log("_openedOnAngle in the start of opening is: " + _openedOnAngle);

            float targetAngle = 1f - angle / 360f;
            LeanTween.value(gameObject, initialAngle, targetAngle, time).setEase(easeType).setOnUpdate((float val) =>
            {
                OpenedOnAngle = val;

                /*if (outerSphere != null) outerSphere.material.SetFloat("_Closed", val);
                if (innerSphere != null) innerSphere.material.SetFloat("_Closed", val);
                if (leftSlice != null) leftSlice.material.SetFloat("_Closed", val);
                if (rightSlice != null) rightSlice.material.SetFloat("_Closed", val);

                if (_lookingAt != null)
                {
                    //currentTransformRotation.y -= ((360.0f - val * 360.0f) / 2f) * Time.deltaTime;
                    //transform.rotation = Quaternion.Euler(currentTransformRotation);
                }*/
            })
            .setOnComplete(() =>
            {
                goToState(SlicingSphereState.Opened);
            });
        }
    }

    public void close(float time = 3.3f, LeanTweenType easeType = LeanTweenType.easeInOutQuad)
    {
        //Debug.Log("Time of closing: " + time);
        Debug.Log(gameObject.name + ", state = " + state + ", underlyingState = " + underlyingState);
        if ((state == SlicingSphereState.Disappeared) || (state == SlicingSphereState.Opened))
        {
            if (state == SlicingSphereState.Disappeared)
            {
                leftSlice.enabled = true;
                rightSlice.enabled = true;
            }

            goToState(SlicingSphereState.Closing);
            float initialAngle;
            /*if (rightSlice != null)
            {
                initialAngle = rightSlice.material.GetFloat("_Closed");
                //Debug.Log("rightSlice closed: " + initialAngle);
            }
            else if (outerSphere != null)
            {
                initialAngle = outerSphere.material.GetFloat("_Closed");
                //Debug.Log("outerSphere closed: " + initialAngle);
            }
            else if (leftSlice != null)
            {
                initialAngle = leftSlice.material.GetFloat("_Closed");
                //Debug.Log("leftSlice closed: " + initialAngle);
            }
            else
            {
                initialAngle = innerSphere.material.GetFloat("_Closed");
                //Debug.Log("innerSphere closed: " + initialAngle);
            }*/

            initialAngle = OpenedOnAngle;

            LeanTween.value(gameObject, initialAngle, 0f, time).setEase(easeType).setOnUpdate((float val) =>
            {
                OpenedOnAngle = val;

                /*//Debug.Log("val = " + val);
                if (outerSphere != null) outerSphere.material.SetFloat("_Closed", val);
                if (innerSphere != null) innerSphere.material.SetFloat("_Closed", val);
                if (leftSlice != null) leftSlice.material.SetFloat("_Closed", val);
                if (rightSlice != null) rightSlice.material.SetFloat("_Closed", val);

                if (_lookingAt != null)
                {
                    //currentTransformRotation.y += ((360.0f - val * 360.0f) / 2f) * Time.deltaTime;
                    //transform.rotation = Quaternion.Euler(currentTransformRotation);
                }*/
            })
            .setOnComplete(() =>
            {
                goToState(SlicingSphereState.Closed);
            });
        }
        else if ((state == SlicingSphereState.Closed) && (underlyingState == UnderlyingState.Underlying))
        {
            Debug.Log("11133!!");
            LeanTween.value(gameObject, 0, 1f, time).setOnComplete(() =>
            {
                if (outerSphere != null)
                {
                    Debug.Log("Trying to destroy outer plane");
                    Destroy(outerSphere.gameObject);
                    outerSphere = null;
                }
            });
        }
    }

    public void Unload()
    {

    }

    public void CreatePlanes(LoadQueries query, System.Action callback)
    {
        switch (query)
        {
            case LoadQueries.RightAndLeft:
                if (leftSlice == null)
                {
                    callbackToUpperScriptMultiple = callback;
                    CreatePlane(SertainPlane.LeftAndRight);
                }
                else
                {
                    callback.Invoke();
                }
                break;
            case LoadQueries.Outer:
                Debug.Log("LoadQueries.Outer");
                if (outerSphere == null)
                {
                    Debug.Log("outerSphere == null");
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        if (transform.GetChild(i).gameObject.name.Contains("OuterPlane"))
                        {
                            Debug.Log("FOUNDED");
                            outerSphere = transform.GetChild(i).GetComponent<MeshRenderer>();
                            callback.Invoke();
                            break;
                        }
                    }

                    if (outerSphere == null)
                    {
                        callbackToUpperScript = callback;
                        CreatePlane(SertainPlane.Outer);
                    }
                }
                else
                {
                    callback.Invoke();
                }
                break;
            case LoadQueries.Inner:
                if (innerSphere == null)
                {
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        if (transform.GetChild(i).gameObject.name.Contains("InnerPlane"))
                        {
                            innerSphere = transform.GetChild(i).GetComponent<MeshRenderer>();
                            callback.Invoke();
                            break;
                        }
                    }

                    if (innerSphere == null)
                    {
                        callbackToUpperScript = callback;
                        CreatePlane(SertainPlane.Inner);
                    }
                }
                else
                {
                    callback.Invoke();
                }
                break;
        }
    }

    public void CreatePlane(SertainPlane neededPlane, System.Action onPlaneCreatedCallback = null)
    {
        callbackWhenPlaneCreated = onPlaneCreatedCallback;
        
        switch (neededPlane)
        {
            case SertainPlane.Outer:
                Loader.Instance.LoadAndIntsntiateGOPrefab(Prefix + "OuterPlane", onOuterPlaneInstantiated);
                /*if (outerSphere == null)
                {
                    InterfaceController.Instance.LoadAndIntsntiateGOPrefab(Prefix + "OuterPlane", onOuterPlaneInstantiated);
                }
                else
                {
                    if (callbackToUpperScript != null)
                    {
                        callbackToUpperScript.Invoke();
                        callbackToUpperScript = null;
                    }
                    if (callbackWhenPlaneCreated != null)
                    {
                        callbackWhenPlaneCreated.Invoke();
                        callbackWhenPlaneCreated = null;
                    }
                }*/
                break;
            case SertainPlane.Inner:
                Loader.Instance.LoadAndIntsntiateGOPrefab(Prefix + "InnerPlane", onInnerPlaneInstantiated);
                /*if (innerSphere == null)
                {
                    InterfaceController.Instance.LoadAndIntsntiateGOPrefab(Prefix + "InnerPlane", onInnerPlaneInstantiated);
                }
                else
                {
                    if (callbackToUpperScript != null)
                    {
                        callbackToUpperScript.Invoke();
                        callbackToUpperScript = null;
                    }
                    if (callbackWhenPlaneCreated != null)
                    {
                        callbackWhenPlaneCreated.Invoke();
                        callbackWhenPlaneCreated = null;
                    }
                }*/
                break;
            case SertainPlane.LeftAndRight:
                Loader.Instance.LoadInstantiateMultiplePrefabs(new string[] { Prefix + "LeftSlice", Prefix + "RightSlice" }, onLeftAndRightPlanesInstantiated, LoadMultiplePrefabsOptions.LoadAndInstantiate);
                /*if (leftSlice == null)
                {
                    InterfaceController.Instance.LoadInstantiateMultiplePrefabs(new string[] { Prefix + "LeftSlice", Prefix + "RightSlice" }, onLeftAndRightPlanesInstantiated, LoadMultiplePrefabsOptions.LoadAndInstantiate);
                }
                else
                {
                    if (callbackToUpperScript != null)
                    {
                        callbackToUpperScript.Invoke();
                        callbackToUpperScript = null;
                    }
                    if (callbackWhenPlaneCreated != null)
                    {
                        callbackWhenPlaneCreated.Invoke();
                        callbackWhenPlaneCreated = null;
                    }
                }*/
                break;
        }
    }

    void onOuterPlaneInstantiated(GameObject go)
    {
        outerSphere = go.GetComponent<MeshRenderer>();
        outerSphere.transform.parent = transform;
        outerSphere.transform.localPosition = Vector3.zero;
        outerSphere.transform.localScale = Vector3.one;
        outerSphere.transform.localRotation = Quaternion.identity;

        /*if (closedOnStart)
        {
            outerSphere.material.SetFloat("_Closed", 1.0f);
            state = SlicingSphereState.Opened;
        }
        else
        {
            outerSphere.material.SetFloat("_Closed", 0.0f);
            //outerSphere.material.SetFloat("_Closed", 1f - _openedOnAngle);
        }*/
        //outerSphere.material.SetFloat("_Closed", 0f);
        outerSphere.material.SetFloat("_Closed", 1 - _openedOnAngle);

        outerSphere.material.SetFloat("_Diameter", _outerDiameter);
        outerSphere.enabled = outerEnabled;
        outerSphere.material.SetFloat("_Brightness", _layerBrightness);

        outerSphere.GetComponent<MeshFilter>().mesh.bounds = new Bounds(Vector3.zero, Vector3.one);

        if (callbackToUpperScript != null)
        {
            callbackToUpperScript.Invoke();
            callbackToUpperScript = null;
        }

        if (callbackWhenPlaneCreated != null)
        {
            callbackWhenPlaneCreated.Invoke();
            callbackWhenPlaneCreated = null;
        }
    }

    void onInnerPlaneInstantiated(GameObject go)
    {
        innerSphere = go.GetComponent<MeshRenderer>();
        innerSphere.transform.parent = transform;
        innerSphere.transform.localPosition = Vector3.zero;
        innerSphere.transform.localScale = Vector3.one;
        innerSphere.transform.localRotation = Quaternion.identity;

        //innerSphere.material.SetFloat("_Closed", 0.5f);
        innerSphere.material.SetFloat("_Closed", 1f - _openedOnAngle);
        //innerSphere.material.SetFloat("_Closed", 1f);
        innerSphere.material.SetFloat("_Diameter", _innerDiameter);
        innerSphere.enabled = innerEnabled;
        innerSphere.material.SetFloat("_Brightness", _layerBrightness);

        innerSphere.GetComponent<MeshFilter>().mesh.bounds = new Bounds(Vector3.zero, Vector3.one);

        if (callbackToUpperScript != null)
        {
            callbackToUpperScript.Invoke();
            callbackToUpperScript = null;
        }

        if (callbackWhenPlaneCreated != null)
        {
            callbackWhenPlaneCreated.Invoke();
            callbackWhenPlaneCreated = null;
        }
    }

    void onLeftAndRightPlanesInstantiated(GameObject[] go)
    {
        Debug.Log("onLeftAndRightPlanesInstantiated");
        leftSlice = go[0].GetComponent<MeshRenderer>();
        leftSlice.transform.parent = transform;
        leftSlice.transform.localPosition = Vector3.zero;
        leftSlice.transform.localScale = Vector3.one;
        leftSlice.transform.localRotation = Quaternion.identity;
        rightSlice = go[1].GetComponent<MeshRenderer>();
        rightSlice.transform.parent = transform;
        rightSlice.transform.localPosition = Vector3.zero;
        rightSlice.transform.localScale = Vector3.one;
        rightSlice.transform.localRotation = Quaternion.identity;

        leftSlice.material.SetFloat("_Closed", 1f - _openedOnAngle);
        leftSlice.material.SetFloat("_DiameterOuter", _outerDiameter);
        leftSlice.material.SetFloat("_DiameterInner", _innerDiameter);
        leftSlice.enabled = leftEnabled;
        leftSlice.material.SetFloat("_Brightness", _layerBrightness);

        leftSlice.GetComponent<MeshFilter>().mesh.bounds = new Bounds(Vector3.zero, Vector3.one);

        rightSlice.material.SetFloat("_Closed", 1f - _openedOnAngle);
        rightSlice.material.SetFloat("_DiameterOuter", _outerDiameter);
        rightSlice.material.SetFloat("_DiameterInner", _innerDiameter);
        rightSlice.enabled = rightEnabled;
        rightSlice.material.SetFloat("_Brightness", _layerBrightness);

        rightSlice.GetComponent<MeshFilter>().mesh.bounds = new Bounds(Vector3.zero, Vector3.one);

        if (callbackToUpperScriptMultiple != null)
        {
            Debug.Log("callbackToUpperScriptMultiple Invoked");
            callbackToUpperScriptMultiple.Invoke();
            callbackToUpperScriptMultiple = null;
        }

        /*if (callbackToUpperScript != null)
        {
            Debug.Log("callbackToUpperScript Invoked");
            callbackToUpperScript.Invoke();
            callbackToUpperScript = null;
        }*/

        if (callbackWhenPlaneCreated != null)
        {
            callbackWhenPlaneCreated.Invoke();
            callbackWhenPlaneCreated = null;
        }
    }
}