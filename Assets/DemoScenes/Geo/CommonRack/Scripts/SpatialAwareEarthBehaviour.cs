using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DebugFeatures;

/// <summary>
/// Когда какая-то часть застревает в стене, если это статическая часть - немедленно ищем место получше. Иначе - ждем немного, и, если ничего не изменилось, ищем место получше.
/// </summary>
public class SpatialAwareEarthBehaviour : MonoBehaviour
{
    public CommonGeoSceneRack Rack;
    public GameObject BoundsObject;
    public float TimeToWaitForSelfResolvingStucking = 3f;

    Vector3 initialScale;

    public List<SpatialAwaredObject> spatialAwaredObjects = new List<SpatialAwaredObject>();

    void Start()
    {
        /*if (SceneStateMachine.Instance.state.EarthState)
        {
            initialScale = Rack.transform.localScale / SlicedEarthPolygon.Instance.transform.localScale.x;
        }
        else
        {
            initialScale = Rack.transform.localScale / PiePolygon.Instance.transform.localScale.x;
        }*/
        SceneStateMachine.Instance.StateChangeEnded += onStateChanged;
        HoloStudyDemoGeoMenuController.Instance.OnMenuShowed += onMenuShowed;
        HoloStudyDemoGeoMenuController.Instance.OnMenuClosed += onMenuClosed;
    }

    void OnDestroy()
    {
        SceneStateMachine.Instance.StateChangeEnded -= onStateChanged;
        HoloStudyDemoGeoMenuController.Instance.OnMenuShowed -= onMenuShowed;
        HoloStudyDemoGeoMenuController.Instance.OnMenuClosed -= onMenuClosed;
    }

    void onMenuShowed()
    {
        for (int i = 0; i < spatialAwaredObjects.Count; i++)
        {
            //spatialAwaredObjects[i].StartCheckingStukOnTheWall();
        }
    }

    void onMenuClosed()
    {
        for (int i = 0; i < spatialAwaredObjects.Count; i++)
        {
            spatialAwaredObjects[i].StopCheckingStukOnTheWall();
        }
    }

    void onStateChanged()
    {
        if (SceneStateMachine.Instance.IsEarthContainingState())
        {
            initialScale = Rack.transform.localScale / SlicedEarthPolygon.Instance.transform.localScale.x;
        }
        else
        {
            initialScale = Rack.transform.localScale / PiePolygon.Instance.transform.localScale.x;
        }
        /*
        if ((SceneStateMachine.Instance.state == SceneStateMachine.Instance.defaultViewState) && (SceneStateMachine.Instance.state == SceneStateMachine.Instance.beginningVoidState))
        {
            for (int i = 0; i < spatialAwaredObjects.Count; i++)
            {
                spatialAwaredObjects[i].StopCheckingStukOnTheWall();
            }
        }
        else
        {
            for (int i = 0; i < spatialAwaredObjects.Count; i++)
            {
                spatialAwaredObjects[i].StartCheckingStukOnTheWall();
            }
        }*/
    }

	void OnStuckOnTheWall(object box) {
        DebugFeaturesExtensions.CleanAllMarks();

        object[] arr = box as object[];
        GameObject sender = arr[0] as GameObject;
        RaycastHit hitInfo = (RaycastHit)arr[1];

        if (sender.GetComponent<SpatialAwaredObject>().Mobility == SpatialAwaredObject.SpatialAwaredObjectMobility.Static)
        {
            requestFinding(sender, hitInfo);
        }
        else
        {
            StartCoroutine(WaitForSelfResolvingStucking(TimeToWaitForSelfResolvingStucking, sender, hitInfo));
        }
	}
    
    IEnumerator WaitForSelfResolvingStucking(float timeToWait, GameObject sender, RaycastHit hitInfo)
    {
        float counter = 0f;

        while (counter < timeToWait)
        {
            counter += Time.deltaTime;
            yield return null;
        }
        
        requestFinding(sender, hitInfo);
    }

    //Только 1 запрос на поиск может быть отправлен за раз
    bool canRequestFinding = true;
    void requestFinding(GameObject sender, RaycastHit hitInfo)
    {
        Debug.Log("requestFinding!!");
        if (canRequestFinding)
        {
            canRequestFinding = false;

            for (int i = 0; i < spatialAwaredObjects.Count; i++)
            {
                spatialAwaredObjects[i].StopCheckingStukOnTheWall();
            }
            
            if (SceneStateMachine.Instance.IsEarthContainingState())
            {
                Rack.transform.position = SlicedEarthPolygon.Instance.transform.position;
                Rack.transform.rotation = SlicedEarthPolygon.Instance.transform.rotation;
                Rack.transform.localScale = SlicedEarthPolygon.Instance.transform.localScale.x * initialScale;
            }
            else
            {
                Rack.transform.position = PiePolygon.Instance.transform.position;
                Rack.transform.rotation = PiePolygon.Instance.transform.rotation;
                Rack.transform.localScale = PiePolygon.Instance.transform.localScale.x * initialScale;
            }

            hitInfo.point = TryToGetBetterHitPoint(hitInfo.point);

            Vector3 realStartingPoint = BoundsObject.transform.position + (hitInfo.point - sender.transform.position);
            //realStartingPoint.Mark().Blue();
            SpatialAwareness.Instance.FindNearestOpenedSpace(realStartingPoint, BoundsObject.transform.localScale, BoundsObject.transform.rotation, true, 0.35f);
            SpatialAwareness.Instance.OnNearestOpenedSpaceFindingComplete += onNearestOpenedSpaceFounded;
        }
    }

    Vector3 TryToGetBetterHitPoint(Vector3 hitPoint)
    {
        Vector3 result = hitPoint;

        RaycastHit hitInfo;
        bool hit = Physics.Raycast(Camera.main.transform.position, (hitPoint - Camera.main.transform.position).normalized, out hitInfo, Vector3.Distance(hitPoint, Camera.main.transform.position), LayerMask.GetMask("SpatialMappingLayer"));
        if (hit)
        {
            result = hitInfo.point;
            //result.Mark().Red();
        }

        return result;
    }

    void onNearestOpenedSpaceFounded(SpatialAwareness.NearestOpenedSpaceFindingResult result)
    {
        if (result.Succeded)
        {
            GetComponent<CommonGeoSceneRack>().ReMove(result, () =>
            {
                canRequestFinding = true;
                for (int i = 0; i < spatialAwaredObjects.Count; i++)
                {
                    spatialAwaredObjects[i].StartCheckingStukOnTheWall();
                }
            });
        }
        else
        {
            canRequestFinding = true;
            for (int i = 0; i < spatialAwaredObjects.Count; i++)
            {
                spatialAwaredObjects[i].StartCheckingStukOnTheWall();
            }
        }
    }

    void OnNotStuckingInTheWall(GameObject sender)
    {
        StopCoroutine("WaitForSelfResolvingStucking");
    }
}
