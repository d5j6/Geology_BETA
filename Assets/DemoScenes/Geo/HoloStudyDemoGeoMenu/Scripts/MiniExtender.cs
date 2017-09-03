using UnityEngine;
using HoloToolkit.Unity;

public class MiniExtender : Singleton<MiniExtender>
{
    public GameObject Obj1;
    public GameObject Obj2;
    Vector3 initialPoint1;
    Vector3 initialPoint2;
    Vector3 targetPoint1;
    Vector3 targetPoint2;

    bool extended = true;

    void Awake()
    {
        initialPoint1 = Obj1.transform.localPosition;
        initialPoint2 = Obj2.transform.localPosition;

        targetPoint1 = initialPoint1;
        targetPoint2 = initialPoint2;
        targetPoint1.x -= 0.5f;
        targetPoint2.x += 0.5f;
    }

    void Start()
    {
        Obj1.transform.localPosition = targetPoint1;
        Obj2.transform.localPosition = targetPoint2;
    }

    public void Extend()
    {
        if (!extended)
        {
            extended = true;
            LeanTween.value(gameObject, 0, 1, 1f).setEase(LeanTweenType.easeInOutCubic).setOnUpdate((float val) =>
            {
                Obj1.transform.localPosition = Vector3.Lerp(initialPoint1, targetPoint1, val);
                Obj2.transform.localPosition = Vector3.Lerp(initialPoint2, targetPoint2, val);
            });
        }
    }

    public void Join()
    {
        if (extended)
        {
            extended = false;
            LeanTween.value(gameObject, 1, 0, 1f).setEase(LeanTweenType.easeInOutCubic).setOnUpdate((float val) =>
            {
                Obj1.transform.localPosition = Vector3.Lerp(initialPoint1, targetPoint1, val);
                Obj2.transform.localPosition = Vector3.Lerp(initialPoint2, targetPoint2, val);
            });
        }
    }

    public void Toggle()
    {
        if (extended)
        {
            Join();
        }
        else
        {
            Extend();
        }
    }
}
