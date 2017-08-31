using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class CurvedSpaceController : MonoBehaviour {

    private TextMeshProUGUI _text;
    Color textColor;

    [Range(0f, 1f)]
    public float _mass = 0;
    [Range(0f, 10f)]
    public float _spreading = 0f;
    [Range(0f, 1f)]
    public float _centering = 0f;
    [Range(0f, 1f)]
    public float _thickness = 0f;
    public float Mass
    {
        get { return _mass; }
        set
        {
            _mass = value;
            for (int i = 0; i <= 10; i++)
            {
                xPlanes[i].SetFloat("_Coeff", (1.0f - Mathf.Abs(5.0f - i) / (5.0f + Spreading)) * _mass);
            }
            for (int i = 0; i <= 10; i++)
            {
                zPlanes[i].SetFloat("_Coeff", (1.0f - Mathf.Abs(5.0f - i) / (5.0f + Spreading)) * _mass);
            }
        }
    }
    public float Centering
    {
        get { return _centering; }
        set
        {
            _centering = value;
            for (int i = 0; i <= 10; i++)
            {
                xPlanes[i].SetFloat("_Centering", _centering);
            }
            for (int i = 0; i <= 10; i++)
            {
                zPlanes[i].SetFloat("_Centering", _centering);
            }
        }
    }
    public float Thickness
    {
        get { return _thickness; }
        set
        {
            _thickness = value;
            for (int i = 0; i <= 10; i++)
            {
                xPlanes[i].SetFloat("_Thickness", _thickness);
            }
            for (int i = 0; i <= 10; i++)
            {
                zPlanes[i].SetFloat("_Thickness", _thickness);
            }
        }
    }
    public float Spreading
    {
        get { return _spreading; }
        set
        {
            _spreading = value;
            for (int i = 0; i <= 10; i++)
            {
                xPlanes[i].SetFloat("_Spreading", _spreading);
            }
            for (int i = 0; i <= 10; i++)
            {
                zPlanes[i].SetFloat("_Spreading", _spreading);
            }
        }
    }

    private long _labelMass = 0;
    public long LabelMass
    {
        get { return _labelMass; }
        set
        {
            _labelMass = value;
            _text.text = "Mass = " + _labelMass + " billions billions tonns";
        }
    }
    
    List<Material> xPlanes = new List<Material>();
    List<Material> zPlanes = new List<Material>();

    void Awake()
    {
        _text = transform.Find("CanvasHolder").Find("Canvas").Find("Text").gameObject.GetComponent<TextMeshProUGUI>();
        textColor = _text.color;
        textColor.a = 0;
        _text.color = textColor;
        LabelMass = 0;

        xPlanes.Add(transform.Find("Plane_20").gameObject.GetComponent<Renderer>().material);
        xPlanes.Add(transform.Find("Plane_21").gameObject.GetComponent<Renderer>().material);
        xPlanes.Add(transform.Find("Plane_22").gameObject.GetComponent<Renderer>().material);
        xPlanes.Add(transform.Find("Plane_23").gameObject.GetComponent<Renderer>().material);
        xPlanes.Add(transform.Find("Plane_24").gameObject.GetComponent<Renderer>().material);
        xPlanes.Add(transform.Find("Plane_25").gameObject.GetComponent<Renderer>().material);
        xPlanes.Add(transform.Find("Plane_26").gameObject.GetComponent<Renderer>().material);
        xPlanes.Add(transform.Find("Plane_27").gameObject.GetComponent<Renderer>().material);
        xPlanes.Add(transform.Find("Plane_28").gameObject.GetComponent<Renderer>().material);
        xPlanes.Add(transform.Find("Plane_29").gameObject.GetComponent<Renderer>().material);
        xPlanes.Add(transform.Find("Plane_30").gameObject.GetComponent<Renderer>().material);

        zPlanes.Add(transform.Find("Plane_00").gameObject.GetComponent<Renderer>().material);
        zPlanes.Add(transform.Find("Plane_01").gameObject.GetComponent<Renderer>().material);
        zPlanes.Add(transform.Find("Plane_02").gameObject.GetComponent<Renderer>().material);
        zPlanes.Add(transform.Find("Plane_03").gameObject.GetComponent<Renderer>().material);
        zPlanes.Add(transform.Find("Plane_04").gameObject.GetComponent<Renderer>().material);
        zPlanes.Add(transform.Find("Plane_05").gameObject.GetComponent<Renderer>().material);
        zPlanes.Add(transform.Find("Plane_06").gameObject.GetComponent<Renderer>().material);
        zPlanes.Add(transform.Find("Plane_07").gameObject.GetComponent<Renderer>().material);
        zPlanes.Add(transform.Find("Plane_08").gameObject.GetComponent<Renderer>().material);
        zPlanes.Add(transform.Find("Plane_09").gameObject.GetComponent<Renderer>().material);
        zPlanes.Add(transform.Find("Plane_10").gameObject.GetComponent<Renderer>().material);
    }

    public void ShowText(float t = 0.98871f)
    {
        LeanTween.value(gameObject, 0, 1, t).setOnUpdate((float val) =>
        {
            textColor.a = val;
            _text.color = textColor;
        });
    }

    public void HideText(float t = 0.98871f)
    {
        LeanTween.value(gameObject, 1, 0, t).setOnUpdate((float val) =>
        {
            textColor.a = val;
            _text.color = textColor;
        });
    }
}