using UnityEngine;
using System.Collections;
using HoloToolkit.Unity;

public class CommonGeoSceneRack : Singleton<CommonGeoSceneRack> {

    public GameObject[] RackedObjects;
    Transform[] RackedObjectsParents;
    Interpolator interpolator;

    public void ReMove(SpatialAwareness.NearestOpenedSpaceFindingResult target, System.Action callback = null)
    {
        Vector3 delta = target.Position - transform.position;

        for (int i = 0; i < RackedObjects.Length; i++)
        {
            interpolator = RackedObjects[i].GetComponent<Interpolator>();
            if (interpolator == null)
            {
                interpolator = RackedObjects[i].AddComponent<Interpolator>();
            }
            interpolator.PositionPerSecond = 2f;
            interpolator.RotationDegreesPerSecond = 90f;
            interpolator.ScalePerSecond = 1f;
            interpolator.SetTargetPosition(RackedObjects[i].transform.position + delta);
        }

        /*Debug.Log("target.Position = " + target.Position);
        Debug.Log("target.Sides = " + target.Sides);
        Debug.Log("target.Rotation = " + target.Rotation);
        RackedObjectsParents = new Transform[RackedObjects.Length];
        for (int i = 0; i < RackedObjects.Length; i++)
        {
            RackedObjectsParents[i] = RackedObjects[i].transform.parent;
            RackedObjects[i].transform.parent = transform;
        }

        interpolator = GetComponent<Interpolator>();
        if (interpolator == null)
        {
            interpolator = gameObject.AddComponent<Interpolator>();
        }

        interpolator.PositionPerSecond = 2f;
        interpolator.RotationDegreesPerSecond = 90f;
        interpolator.ScalePerSecond = 1f;
        interpolator.SetTargetPosition(target.Position);
        interpolator.SetTargetRotation(target.Rotation);
        interpolator.SetTargetLocalScale(target.Sides);*/

        LeanTween.delayedCall(1f, () =>
        {
            if (callback != null)
            {
                callback.Invoke();
            }

            onStopReMoving();
        });
    }

    public void onStopReMoving()
    {
        Debug.Log("Stop ReMoving");
        /*for (int i = 0; i < RackedObjects.Length; i++)
        {
            RackedObjects[i].transform.parent = RackedObjectsParents[i];
        }*/

        /*if (interpolator != null)
        {
            Destroy(interpolator);
        }*/
    }
}
