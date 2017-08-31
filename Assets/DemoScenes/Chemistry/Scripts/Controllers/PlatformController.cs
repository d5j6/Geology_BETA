using UnityEngine;
using System.Collections;
using DG.Tweening;

public class PlatformController : MonoBehaviour
{
    public Transform target;

    private Vector3 realPosition;

    private Coroutine followCoroutine;

    public float followSpeed;

    [SerializeField]
    private AnimationCurve _platformFlyCurve;

    void Awake()
    {
        followCoroutine = StartCoroutine(FollowToTargetCoroutine());
    }

    void OnEnable()
    {
        followCoroutine = StartCoroutine(FollowToTargetCoroutine());
    }

    IEnumerator FollowToTargetCoroutine()
    {
        while(true)
        {
            yield return null;

            realPosition = Vector3.Lerp(transform.position, target.position, followSpeed * Time.deltaTime);

            Vector3 realWithDeltaY = realPosition;
            realWithDeltaY.y += _platformFlyCurve.Evaluate(Time.time);

            transform.position = realWithDeltaY;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3(0f, target.eulerAngles.y, 0f)), followSpeed * Time.deltaTime);
        }
    }
}
