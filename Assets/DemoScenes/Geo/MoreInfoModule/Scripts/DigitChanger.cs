using UnityEngine;
using System.Collections;
using TMPro;

public class DigitChanger : MonoBehaviour
{
    public float fromDigit = 0f;
    public float toDigit = 1f;

    public float duration = 1f;

    public string postfix = "";

    public bool isInt;

    private TextMeshProUGUI _text;

    void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    public void Activate(bool isReverse = false)
    {
        float from = fromDigit;
        float to = toDigit;

        if (isReverse)
        {
            from = toDigit;
            to = fromDigit;
        }
        
        LeanTween.value(gameObject, from, to, duration).setOnUpdate((float value) =>
        {
            value = (isInt) ? (int)value : value;
            _text.SetText("{0} " + postfix, value);
        });
    }
}
