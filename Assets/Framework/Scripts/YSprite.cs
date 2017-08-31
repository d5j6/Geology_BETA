using UnityEngine;

public class YSprite : MonoBehaviour
{
    private void Update()
    {
        Vector3 directionToTarget = Camera.main.transform.position - gameObject.transform.position;
        
        if (directionToTarget.sqrMagnitude < 0.001f)
        {
            return;
        }

        directionToTarget.y = 0.0f;

        gameObject.transform.rotation = Quaternion.LookRotation(-directionToTarget);
    }
}