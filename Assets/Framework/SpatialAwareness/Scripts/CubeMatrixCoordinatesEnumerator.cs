using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CubeMatrixCoordinatesEnumerable
{
    int currentCubeLevel = 1;

    int currentXOffset = 0;
    int currentYOffset = 0;
    int currentZOffset = 0;

    public void Reset()
    {
        currentCubeLevel = 1;
        currentXOffset = 0;
        currentYOffset = 0;
        currentZOffset = 0;
    }

    public CubeMatrixCoordinatesEnumerable()
    {
        Reset();
    }

    public Vector3 GetNext()
    {
        /*while (currentCubeLevel <= limit)
        {
            for (; currentXOffset <= currentCubeLevel; currentXOffset++)
            {
                for (; currentYOffset <= currentCubeLevel; currentYOffset++)
                {
                    for (; currentZOffset <= currentCubeLevel; currentZOffset++)
                    {
                        if ((currentCubeLevel == Mathf.Abs(currentXOffset)) || (currentCubeLevel == Mathf.Abs(currentYOffset)) || (currentCubeLevel == Mathf.Abs(currentZOffset)))
                        {
                            Debug.Log("currentXOffset = " + currentXOffset);
                            Debug.Log("currentYOffset = " + currentYOffset);
                            Debug.Log("currentZOffset = " + currentZOffset);

                            Vector3 result = new Vector3(currentXOffset, currentYOffset, currentZOffset);

                            if (currentZOffset <= currentCubeLevel)
                            {
                                if (currentYOffset <= currentCubeLevel)
                                {
                                    if (currentXOffset <= currentCubeLevel)
                                    {
                                        currentXOffset++;
                                    }
                                    else
                                    {
                                        currentCubeLevel++;
                                        currentXOffset = currentCubeLevel * -1;
                                        currentYOffset = currentCubeLevel * -1;
                                        currentZOffset = currentCubeLevel * -1;
                                    }
                                }
                                else
                                {
                                    currentYOffset++;
                                }
                            }
                            else
                            {
                                currentZOffset++;
                            }

                            return result;
                        }
                    }
                }
            }

        }*/
        
        while (!((Mathf.Abs(currentXOffset) == currentCubeLevel) || (Mathf.Abs(currentYOffset) == currentCubeLevel) || (Mathf.Abs(currentZOffset) == currentCubeLevel)))
        {
            iterate();
        }

        Vector3 result = new Vector3(currentXOffset, currentYOffset, currentZOffset);

        iterate();

        return result;
    }

    void iterate()
    {
        if (currentZOffset >= currentCubeLevel)
        {
            currentZOffset = 0;
            if (currentYOffset >= currentCubeLevel)
            {
                currentYOffset = 0;
                if (currentXOffset >= currentCubeLevel)
                {
                    currentCubeLevel++;
                    currentXOffset = 0;
                    currentYOffset = 0;
                    currentZOffset = 0;
                }
                else
                {
                    if (currentXOffset >= 0)
                    {
                        currentXOffset++;
                        currentXOffset *= -1;
                    }
                    else
                    {
                        currentXOffset *= -1;
                    }
                }
            }
            else
            {
                if (currentYOffset >= 0)
                {
                    currentYOffset++;
                    currentYOffset *= -1;
                }
                else
                {
                    currentYOffset *= -1;
                }
            }
        }
        else
        {
            if (currentZOffset >= 0)
            {
                currentZOffset++;
                currentZOffset *= -1;
            }
            else
            {
                currentZOffset *= -1;
            }
        }
    }
}
