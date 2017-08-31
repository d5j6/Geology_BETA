using UnityEngine;
using TMPro;
using HoloToolkit.Unity;

public class StandsForHEROController : Singleton<StandsForHEROController>
{
    public TextMeshPro H;
    public TextMeshPro olo;
    public TextMeshPro E;
    public TextMeshPro arth;
    public TextMeshPro R;
    public TextMeshPro esearch;
    public TextMeshPro O;
    public TextMeshPro bserver;

    public Transform TargetH;
    public Transform TargetE;
    public Transform TargetR;
    public Transform TargetO;

    float tintTime = 0.789f;

    public void ShowOnProfessor(float [] timings, System.Action callback = null)
    {
        LeanTween.delayedCall(timings[0], () =>
        {
            Color c1 = H.color;
            Color c2 = olo.color;
            LeanTween.value(gameObject, 0, 1, tintTime).setEase(LeanTweenType.easeInOutCubic).setOnUpdate((float val) =>
            {
                c1.a = val;
                c2.a = val;
                H.color = c1;
                olo.color = c2;
            });
        });

        LeanTween.delayedCall(timings[1], () =>
        {
            Color c1 = E.color;
            Color c2 = arth.color;
            LeanTween.value(gameObject, 0, 1, tintTime).setEase(LeanTweenType.easeInOutCubic).setOnUpdate((float val) =>
            {
                c1.a = val;
                c2.a = val;
                E.color = c1;
                arth.color = c2;
            });
        });

        LeanTween.delayedCall(timings[2], () =>
        {
            Color c1 = R.color;
            Color c2 = esearch.color;
            LeanTween.value(gameObject, 0, 1, tintTime).setEase(LeanTweenType.easeInOutCubic).setOnUpdate((float val) =>
            {
                c1.a = val;
                c2.a = val;
                R.color = c1;
                esearch.color = c2;
            });
        });

        LeanTween.delayedCall(timings[3], () =>
        {
            Color c1 = O.color;
            Color c2 = bserver.color;
            LeanTween.value(gameObject, 0, 1, tintTime).setEase(LeanTweenType.easeInOutCubic).setOnUpdate((float val) =>
            {
                c1.a = val;
                c2.a = val;
                O.color = c1;
                bserver.color = c2;
            });
        });

        LeanTween.delayedCall(timings[4], () =>
        {
            Color c1 = olo.color;
            Color c2 = arth.color;
            Color c3 = esearch.color;
            Color c4 = bserver.color;
            LeanTween.value(gameObject, 1, 0, 1.56f).setEase(LeanTweenType.easeInOutCubic).setOnUpdate((float val) =>
            {
                c1.a = val;
                c2.a = val;
                c3.a = val;
                c4.a = val;
                olo.color = c1;
                arth.color = c2;
                esearch.color = c3;
                bserver.color = c4;
            }).setOnComplete(() =>
            {
                /*float y = -0.475f;
                float xPadding = 0.06f;
                Vector3 target1 = new Vector3(xPadding * -1.5f, y, 0f);
                Vector3 target2 = new Vector3(xPadding * -0.5f, y, 0f);
                Vector3 target3 = new Vector3(xPadding * 0.5f, y, 0f);
                Vector3 target4 = new Vector3(xPadding * 1.5f, y, 0f);*/
                LeanTween.moveLocal(H.transform.parent.gameObject, TargetH.localPosition, 1.56f);
                LeanTween.moveLocal(E.transform.parent.gameObject, TargetE.localPosition, 1.56f);
                LeanTween.moveLocal(R.transform.parent.gameObject, TargetR.localPosition, 1.56f);
                LeanTween.moveLocal(O.transform.parent.gameObject, TargetO.localPosition, 1.56f).setOnComplete(() =>
                {
                    LeanTween.delayedCall(1.36f, () =>
                    {
                        c1 = H.color;
                        c2 = E.color;
                        c3 = R.color;
                        c4 = O.color;
                        LeanTween.value(gameObject, 1, 0, tintTime).setEase(LeanTweenType.easeInOutCubic).setOnUpdate((float val) =>
                        {
                            c1.a = val;
                            c2.a = val;
                            c3.a = val;
                            c4.a = val;
                            H.color = c1;
                            E.color = c2;
                            R.color = c3;
                            O.color = c4;
                        }).setOnComplete(() =>
                        {
                            if (callback != null)
                            {
                                callback.Invoke();
                            }
                        });
                    });
                });
            });
        });
    }
}
