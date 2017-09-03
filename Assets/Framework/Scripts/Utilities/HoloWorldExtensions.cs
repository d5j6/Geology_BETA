using UnityEngine;
using System.Collections;
using TMPro;

namespace HoloWorldExtensions
{
    public static class HoloWorldExtensions
    {
        private static Color _holoWorldColor = Color.white;

        public static bool about(this float v, float val, float accuracy = 0.01f)
        {
            if ((v >= val - accuracy) && (v <= val + accuracy))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void setRenderers(this GameObject target, bool enabled)
        {
            Renderer[] renderers = target.GetComponentsInChildren<Renderer>();
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].enabled = enabled;
            }

            TextMeshProUGUI[] textRenderers = target.GetComponentsInChildren<TextMeshProUGUI>();
            for (int i = 0; i < textRenderers.Length; i++)
            {
                textRenderers[i].enabled = enabled;
            }
        }

        public static void setRenderersAlpha(this GameObject target, string colorName, float targetColor)
        {
            Renderer[] renderers = target.GetComponentsInChildren<Renderer>();
            _holoWorldColor.a = targetColor;
            Color c;
            for (int i = 0; i < renderers.Length; i++)
            {
                c = renderers[i].sharedMaterial.GetColor(colorName);
                c.a = targetColor;
                renderers[i].sharedMaterial.SetColor(colorName, c);
            }
        }

        public static void setRenderersAlphaCarefully(this GameObject target, string colorName, float targetColor, string alternativeColorName = "")
        {
            Renderer[] renderers = target.GetComponentsInChildren<Renderer>();
            Color c;
            for (int i = 0; i < renderers.Length; i++)
            {
                if (renderers[i].sharedMaterial.HasProperty(colorName))
                {
                    c = renderers[i].sharedMaterial.GetColor(colorName);
                    c.a = targetColor;
                    renderers[i].sharedMaterial.SetColor(colorName, c);
                }
                else if (alternativeColorName != "")
                {
                    c = renderers[i].sharedMaterial.GetColor(alternativeColorName);
                    c.a = targetColor;
                    renderers[i].sharedMaterial.SetColor(alternativeColorName, c);
                }
            }
        }

        public static void setRenderersModeToTransparent(this GameObject target)
        {
            Renderer[] renderers = target.GetComponentsInChildren<Renderer>();
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].material.SetFloat("_Mode", 3.0f);
                renderers[i].material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                renderers[i].material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                renderers[i].material.SetInt("_ZWrite", 0);
                renderers[i].material.DisableKeyword("_ALPHATEST_ON");
                renderers[i].material.EnableKeyword("_ALPHABLEND_ON");
                renderers[i].material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                renderers[i].material.renderQueue = 3000;
            }
        }

        public static void setRenderersModeToOpaque(this GameObject target)
        {
            Renderer[] renderers = target.GetComponentsInChildren<Renderer>();
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].material.SetFloat("_Mode", 0.0f);
                renderers[i].material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                renderers[i].material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                renderers[i].material.SetInt("_ZWrite", 1);
                renderers[i].material.DisableKeyword("_ALPHATEST_ON");
                renderers[i].material.EnableKeyword("_ALPHABLEND_ON");
                renderers[i].material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                renderers[i].material.renderQueue = 2000;
            }
        }

        public static void setRenderersFloat(this GameObject target, string floatName, float targetFloat)
        {
            Renderer[] renderers = target.GetComponentsInChildren<Renderer>();
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].sharedMaterial.SetFloat(floatName, targetFloat);
            }
        }

        public static void setActivated(this GameObject target, bool enabled)
        {
            int gosCount = target.transform.childCount;
            for (int i = 0; i < gosCount; i++)
            {
                target.transform.GetChild(i).gameObject.SetActive(enabled);
            }
        }
    }
}