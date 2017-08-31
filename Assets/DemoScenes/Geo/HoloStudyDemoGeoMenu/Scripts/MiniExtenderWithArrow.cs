using UnityEngine;
using HoloToolkit.Unity;

public class MiniExtenderWithArrow : Singleton<MiniExtenderWithArrow>
{
    public GameObject Obj1;
    public GameObject Obj2;
    public GameObject Arrow;
    private Vector3 initialPoint1;
    private Vector3 initialPoint2;
    private Vector3 targetPoint1;
    private Vector3 targetPoint2;
    
    [HideInInspector]
    public bool Extended = true;
    private Vector3 initialScale;
    private float initialXScale;
    private float initialXPos = -1.6f;
    private float targetXPos = -0.8f;

    protected override void Awake()
    {
        base.Awake();

        initialPoint1 = Obj1.transform.localPosition;
        initialPoint2 = Obj2.transform.localPosition;
        initialXScale = Arrow.transform.localScale.x;
        initialScale = Arrow.transform.localScale;

        targetPoint1 = initialPoint1;
        targetPoint2 = initialPoint2;
        targetPoint1.x -= 1.05f;
        targetPoint2.x += 1.05f;
    }

    private void Start()
    {
        Obj1.transform.localPosition = targetPoint1;
        Obj2.transform.localPosition = targetPoint2;
        Vector3 pos = Arrow.transform.localPosition;
        pos.x = targetXPos;
        Arrow.transform.localPosition = pos;

        Join();
    }

    public void Extend()
    {
        if (!Extended)
        {
            Extended = true;
            Vector3 sc = initialScale;
            Vector3 pos = Arrow.transform.localPosition;
            LeanTween.value(Arrow, 0f, 1f, 1f).setEase(LeanTweenType.easeInOutCubic).setOnUpdate((float val) =>
            {
                Obj1.transform.localPosition = Vector3.Lerp(initialPoint1, targetPoint1, val);
                Obj2.transform.localPosition = Vector3.Lerp(initialPoint2, targetPoint2, val);
                sc.x = Mathf.Lerp(initialXScale, initialXScale*-1, val);
                Arrow.transform.localScale = sc;
                pos.x = Mathf.Lerp(initialXPos, targetXPos, val);
                Arrow.transform.localPosition = pos;
            });
        }
    }

    public void Join()
    {
        if (Extended)
        {
            Extended = false;
            Vector3 sc = initialScale;
            Vector3 pos = Arrow.transform.localPosition;
            LeanTween.value(Arrow, 1f, 0f, 1f).setEase(LeanTweenType.easeInOutCubic).setOnUpdate((float val) =>
            {
                Obj1.transform.localPosition = Vector3.Lerp(initialPoint1, targetPoint1, val);
                Obj2.transform.localPosition = Vector3.Lerp(initialPoint2, targetPoint2, val);
                sc.x = Mathf.Lerp(initialXScale, initialXScale * -1, val);
                Arrow.transform.localScale = sc;
                pos.x = Mathf.Lerp(initialXPos, targetXPos, val);
                Arrow.transform.localPosition = pos;
            });
        }
    }

    public void Toggle()
    {
        if (Extended)
        {
            Join();
        }
        else
        {
            Extend();
        }
    }
}
