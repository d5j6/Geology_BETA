using UnityEngine;
using System;
using System.Collections;
using HoloToolkit.Unity;
using DebugFeatures;

public class SpatialAwareness : Singleton<SpatialAwareness>
{
    //Максимальное количество итераций во время которых выполняется поиск, после этого система бросает это занятие
    int maxNearestOpenedSpaceFindingOperationIterationsNumber = 300;

    public struct NearestOpenedSpaceFindingResult
    {
        public bool Succeded;
        public Vector3 Sides;
        public Vector3 Position;
        public Quaternion Rotation;
    }

    public Action<NearestOpenedSpaceFindingResult> OnNearestOpenedSpaceFindingComplete;

	public void FindNearestOpenedSpace(Vector3 Near, Vector3 Extents, Quaternion Rotation, bool SpatialMeshesOnly = true, float StepSize = 0.3f)
    {
        StartCoroutine(FindingNearestOpenedSpaceRoutine(Near, Extents, Rotation, SpatialMeshesOnly, StepSize));
    }

    IEnumerator FindingNearestOpenedSpaceRoutine(Vector3 Near, Vector3 Extents, Quaternion Rotation, bool SpatialMeshesOnly, float StepSize)
    {
        Collider[] hitColliders;
        int targetLayer = Physics.AllLayers;
        if (SpatialMeshesOnly)
        {
            targetLayer = LayerMask.GetMask("SpatialMappingLayer");
        }

        CubeMatrixCoordinatesEnumerable positions = new CubeMatrixCoordinatesEnumerable();
        Vector3 currentPos = Vector3.zero;

        int counterOfTryes = 0;
        yield return null;
        while (counterOfTryes < maxNearestOpenedSpaceFindingOperationIterationsNumber)
        {
            counterOfTryes++;

            currentPos = positions.GetNext() * StepSize + Near;
            ///currentPos.Mark().Green().Min();
            hitColliders = Physics.OverlapBox(currentPos, Extents / 2f, Rotation, targetLayer, QueryTriggerInteraction.Ignore);

            if (hitColliders.Length == 0)
            {
                break;
            }
            else
            {
                yield return null;
            }
        }

        NearestOpenedSpaceFindingResult result = new NearestOpenedSpaceFindingResult();
        if (counterOfTryes == maxNearestOpenedSpaceFindingOperationIterationsNumber)
        {
            result.Succeded = false;
        }
        else
        {
            result.Succeded = true;
            result.Sides = Extents;
            result.Rotation = Rotation;
            result.Position = currentPos;
        }

        if (OnNearestOpenedSpaceFindingComplete != null)
        {
            OnNearestOpenedSpaceFindingComplete.Invoke(result);
            OnNearestOpenedSpaceFindingComplete = null;
        }
    }
}
