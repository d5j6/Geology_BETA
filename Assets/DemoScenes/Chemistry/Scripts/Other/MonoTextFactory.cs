using UnityEngine;
using System.Collections;
using TMPro;

public class MonoTextFactory : MonoBehaviour
{
    public void CreateText(string objectName, string text, float width, float height, float fontSize, Vector3 localScale, Vector3 localPos, Quaternion localRotation, Transform parentObject = null)
    {
        TextMeshPro nameText;
        nameText = new GameObject(objectName).AddComponent<TextMeshPro>();

        if(parentObject != null)
        {
            nameText.transform.SetParent(parentObject);
        }

        nameText.transform.localScale = localScale;
        nameText.transform.localPosition = localPos;
        nameText.transform.localRotation = localRotation;
        nameText.text = text;

        nameText.fontSize = fontSize;
        nameText.alignment = TextAlignmentOptions.Center;

        TextContainer tc = nameText.gameObject.GetComponent<TextContainer>();
        tc.width = width;
        tc.height = height;
    }
}
