using UnityEngine;
using System;
using DebugFeatures;

/// <summary>
/// Класс отвечает за базовое понимание окружающего пространства. Пока что это заключается в том, что он может начать отслеживать то, застрял ли он в стене.
/// </summary>
public class SpatialAwaredObject : MonoBehaviour
{
    public enum SpatialAwaredObjectMobility { Static, SemiSelfMoving, MovingOnItsOwn }
    public SpatialAwaredObjectMobility Mobility = SpatialAwaredObjectMobility.SemiSelfMoving;

    public float CheckingStukOnTheWallInterval = 1.5f;
    bool CheckingStukOnTheWall = false;
    float currentCheckingStukOnTheWallInterval;
    float counter = 0;
    string OnStuckOnTheWall = "OnStuckOnTheWall";
    string OnNotStuckingInTheWall = "OnNotStuckingInTheWall";

    public GameObject SpatialMessagesRecieverObject;
    public GameObject SpatialErrorCube;
    int spatialErrorCubeState = 0;
    int spatialErrorCubeWantedState = 0;

    public bool DebugMode = false;
    float debugModeCubeBrightness = 0.2f;

    void Start()
    {
        if (DebugMode)
        {
            SpatialErrorCube.SetActive(true);
            SpatialErrorCube.GetComponent<Renderer>().material.SetFloat("_TotalAlpha", debugModeCubeBrightness);
        }
        else
        {
            SpatialErrorCube.SetActive(false);
        }

        if (CheckingStukOnTheWall)
        {
            StartCheckingStukOnTheWall();
        }
    }

    public void StartCheckingStukOnTheWall()
    {
        if (DebugMode)
        {
            Debug.Log("StartCheckingStukOnTheWall");
        }
        CheckingStukOnTheWall = true;
        counter = 0;
        randomizeNextInterval();
        if (DebugMode)
        {
            Debug.Log("currentCheckingStukOnTheWallInterval = " + currentCheckingStukOnTheWallInterval);
        }
    }
    public void StopCheckingStukOnTheWall()
    {
        if (DebugMode)
        {
            Debug.Log("StopCheckingStukOnTheWall");
        }
        CheckingStukOnTheWall = false;
    }

    void randomizeNextInterval()
    {
        currentCheckingStukOnTheWallInterval = CheckingStukOnTheWallInterval + UnityEngine.Random.Range(-0.5f, 0.5f) * CheckingStukOnTheWallInterval;
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckingStukOnTheWall)
        {
            counter += Time.deltaTime;

            if (counter >= currentCheckingStukOnTheWallInterval)
            {
                counter = 0;
                randomizeNextInterval();

                checkIfStackInTheWall();
            }
        }
    }

    //Принудительный вызыв проверки застревания в стене
    public void ForceCheckIfStackInTheWall()
    {
        checkIfStackInTheWall();
    }

    void checkIfStackInTheWall()
    {
        RaycastHit hitInfo;
        if (DebugMode)
        {
            Debug.Log("Drawing Debug Spheres");
            transform.position.Mark().SetColor(new Color(1, 0, 1, 1)).Min();
            //transform.position.Mark().SetColor(new Color(1, 1, 0, 1)).Min();
            (transform.position + (Camera.main.transform.position - transform.position).normalized* Vector3.Distance(Camera.main.transform.position, transform.position)).Mark().SetColor(new Color(1, 1, 0, 1)).Min();
        }
        bool hit = Physics.Raycast(transform.position, (Camera.main.transform.position - transform.position).normalized, out hitInfo, Vector3.Distance(Camera.main.transform.position, transform.position), LayerMask.GetMask("SpatialMappingLayer"));
        if (hit)
        {
            showSpatialErrorCube();
            if (SpatialMessagesRecieverObject != null)
            {
                object box = new object[] { gameObject, hitInfo };
                SpatialMessagesRecieverObject.SendMessage(OnStuckOnTheWall, box);
            }
        }
        else
        {
            hideSpatialErrorCube();
            if (SpatialMessagesRecieverObject != null)
            {
                SpatialMessagesRecieverObject.SendMessage(OnNotStuckingInTheWall, gameObject);
            }
        }
    }

    void showSpatialErrorCube()
    {
        if (SpatialErrorCube != null)
        {
            spatialErrorCubeWantedState = 1;
            if (spatialErrorCubeState == 0)
            {
                spatialErrorCubeState = 1;
                float startValue = 0;
                if (DebugMode)
                {
                    startValue = debugModeCubeBrightness;
                }
                LeanTween.value(gameObject, startValue, 1, 1f).setEase(LeanTweenType.easeInOutCubic).setOnStart(() =>
                {
                    SpatialErrorCube.SetActive(true);
                }).setOnUpdate((float val) =>
                {
                    SpatialErrorCube.GetComponent<Renderer>().material.SetFloat("_TotalAlpha", val);
                }).setOnComplete(() =>
                {
                    spatialErrorCubeState = 2;
                    if (spatialErrorCubeWantedState == 0)
                    {
                        hideSpatialErrorCube();
                    }
                });
            }
        }
    }

    void hideSpatialErrorCube()
    {
        if (SpatialErrorCube != null)
        {
            spatialErrorCubeWantedState = 0;
            if (spatialErrorCubeState == 2)
            {
                spatialErrorCubeState = 3;
                float startValue = 0;
                if (DebugMode)
                {
                    startValue = debugModeCubeBrightness;
                }
                LeanTween.value(gameObject, 1, startValue, 1f).setEase(LeanTweenType.easeInOutCubic).setOnUpdate((float val) =>
                {
                    SpatialErrorCube.GetComponent<Renderer>().material.SetFloat("_TotalAlpha", val);
                }).setOnComplete(() =>
                {
                    SpatialErrorCube.SetActive(false);
                    spatialErrorCubeState = 0;
                    if (spatialErrorCubeWantedState == 1)
                    {
                        showSpatialErrorCube();
                    }
                });
            }
        }
    }
}
