using HoloToolkit.Unity.SpatialMapping;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableTemplate : Singleton<PlaceableTemplate> {

    public Material PlaceableMaterial;
    public Material NotPlaceableMaterial;
    public GameObject placeableObject;

    private int layerMask = 1 << 31;

    [Range(-0.08f, 0.08f)]
    public float valueToCompareRaycastHits = 0.05f;

    private BoxCollider boxCollider;

    public bool isPlaced { get; private set; }

    void Start()
    {
        boxCollider = placeableObject.GetComponent<BoxCollider>();
        isPlaced = false;
    }

    void Update () {

        isPlaced = CanBePlaced();

        if (isPlaced)
        {
            Debug.Log("True");
            placeableObject.GetComponent<Renderer>().sharedMaterial = PlaceableMaterial;
        }
        else
        {
            Debug.Log("False");
            placeableObject.GetComponent<Renderer>().sharedMaterial = NotPlaceableMaterial;
        }
	}


    private bool CanBePlaced()
    {
        Vector3 rayCastDirection = -(this.gameObject.transform.forward);
        RaycastHit hitInfo;

        Vector3[] corners = CornersCounter();

        for (int i = 0; i < corners.Length; i++)
        {
            corners[i] = placeableObject.transform.TransformVector(corners[i]) + gameObject.transform.position;
        }

        if (!Physics.Raycast(corners[0], rayCastDirection, out hitInfo, 4f, layerMask))
        {
            return false;
        }

        Debug.DrawRay(corners[0], rayCastDirection, Color.cyan, hitInfo.distance);

        for (int i = 1; i < corners.Length; i++)
        {
            RaycastHit hitInfoCorner;
            if (Physics.Raycast(corners[i], rayCastDirection, out hitInfoCorner, 4f, layerMask))
            {
                Debug.DrawRay(corners[i], rayCastDirection, Color.green, hitInfoCorner.distance);
                if (!CheckForDifference(hitInfo.distance, hitInfoCorner.distance))
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        // OK

        if (hitInfo.collider.gameObject.GetComponent<SurfacePlane>().PlaneType != PlaneTypes.Wall)
        {
            return false;
        }

        if (hitInfo.collider.gameObject.transform.localScale.x >= 2.4f && hitInfo.collider.gameObject.transform.localScale.y >= 0.8f)
        {
            return true;
        }
        else
        {
            return false;
        }  
    }

    private bool CheckForDifference(float distanceFromCenter,
                                    float distanceFromCorner)
    {
        float result = Mathf.Abs(distanceFromCenter - distanceFromCorner);
        return (result <= valueToCompareRaycastHits);
    }

    private Vector3[] CornersCounter()
    {
        Vector3 extents = boxCollider.size / 2;

        float minX = boxCollider.center.x - extents.x;
        float maxX = boxCollider.center.x + extents.x;
        float minY = boxCollider.center.y - extents.y;
        float maxY = boxCollider.center.y + extents.y;
        float minZ = boxCollider.center.z - extents.z;
        float maxZ = boxCollider.center.z + extents.z;

        Vector3 center = new Vector3(boxCollider.center.x,
                                     boxCollider.center.y,
                                     maxZ);

        Vector3 corner0 = new Vector3(minX, minY, maxZ);
        Vector3 corner1 = new Vector3(minX, maxY, maxZ);
        Vector3 corner2 = new Vector3(maxX, minY, maxZ);
        Vector3 corner3 = new Vector3(maxX, maxY, maxZ);

        return new Vector3[] { center, corner0, corner1, corner2, corner3 };
    }
}
