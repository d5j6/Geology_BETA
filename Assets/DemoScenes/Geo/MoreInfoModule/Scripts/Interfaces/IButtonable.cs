using UnityEngine;

public interface IButtonable
{
    void OnGazeOver(RaycastHit hitInfo);
    void OnTap(RaycastHit hitInfo);
    void OnGazeLeave();
}