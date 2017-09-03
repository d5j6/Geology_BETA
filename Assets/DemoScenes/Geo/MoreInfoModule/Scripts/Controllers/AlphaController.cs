using UnityEngine;
using System.Collections;
using TMPro;
using Vectrosity;

public class AlphaController : MonoBehaviour
{
    private TextMeshProUGUI _text;
    private SpriteRenderer _sprite;
    private VectorObject2D _line;
    private Material _material;

    [Header("What is?")]
    public bool _isText;
    public bool _isSprite;
    public bool _isLine;
    public bool _isMaterial;

    [Header("Alpha control")]
    public float durationIn = 1f;
    public float durationOut = 1f;
    [Range(0, 1)]
    public float targetAlpha = 1f;

    public bool _isShowed;

    public AlphaController[] callChain;

    [SerializeField]
    private DigitChanger digitChanger;

    void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _sprite = GetComponent<SpriteRenderer>();

        _line = GetComponent<VectorObject2D>();

        if (_isSprite)
        {
            _material = _sprite.material;
        }
        else if (_isMaterial)
        {
            _material = GetComponent<MeshRenderer>().material;
        }
    }

    public void FadeIn()
    {
        if (_isShowed)
        {
            return;
        }

        _isShowed = true;

        StartCoroutine(FadeInCoroutine());
    }

    private IEnumerator FadeInCoroutine()
    {
        if (_isText)
        {
            LeanTween.value(gameObject, ResetAlpha(_text.color), ChangeAlpha(_text.color, targetAlpha), durationIn).setOnUpdate((Color value) =>
            {
                _text.color = value;
            });
            
        }
        else if (_isSprite && !_isMaterial)
        {
            LeanTween.value(gameObject, ResetAlpha(_sprite.color), ChangeAlpha(_sprite.color, targetAlpha), durationIn).setOnUpdate((Color value) =>
            {
                _sprite.color = value;
            });
        }
        else if (_isLine)
        {
            _line.enabled = true;
        }
        else if (_isSprite && _isMaterial)
        {
            LeanTween.value(gameObject, 0f, targetAlpha, durationIn).setOnUpdate((float value) =>
            {
                _material.SetFloat("_VDiscard", value);
            });
        }
        else if (_isMaterial && !_isSprite)
        {
            LeanTween.value(gameObject, 0f, targetAlpha, durationIn).setOnUpdate((float value) =>
            {
                _material.color = ChangeAlpha(_material.color, value);
            });
        }

        if (digitChanger)
        {
            digitChanger.Activate();
        }

        yield return new WaitForSeconds(durationIn);

        foreach (AlphaController alphaController in callChain)
        {
            alphaController.FadeIn();
        }
    }

    public void FadeOut(bool isInstantly = false)
    {
        if (!_isShowed)
        {
            return;
        }

        _isShowed = false;

        if (isInstantly)
        {
            if (_isText)
            {
                _text.color = ResetAlpha(_text.color);
            }
            else if (_isSprite && !_isMaterial)
            {
                _sprite.color = ResetAlpha(_sprite.color);
            }
            else if (_isLine)
            {
                //_line.color = ResetAlpha(_line.color);
                _line.enabled = false;
            }
            else if (_isSprite && _isMaterial)
            {
                _material.SetFloat("_VDiscard", 0f);
            }
            else if (_isMaterial && !_isSprite)
            {
                _material.color = ResetAlpha(_material.color);
            }

            foreach (AlphaController alphaController in callChain)
            {
                alphaController.FadeOut(true);
            }
        }
        else
        {
            if (_isText)
            {
                LeanTween.value(gameObject, _text.color, ResetAlpha(_text.color), durationOut).setOnUpdate((Color value) =>
                {
                    _text.color = value;
                });

            }
            else if (_isSprite && !_isMaterial)
            {
                LeanTween.value(gameObject, _sprite.color, ResetAlpha(_sprite.color), durationOut).setOnUpdate((Color value) =>
                {
                    _sprite.color = value;
                });
            }
            else if (_isLine)
            {
                _line.enabled = false;
            }
            else if (_isSprite && _isMaterial)
            {
                LeanTween.value(gameObject, targetAlpha, 0f, durationOut).setOnUpdate((float value) =>
                {
                    _material.SetFloat("_VDiscard", value);
                });
            }
            else if (_isMaterial && !_isSprite)
            {
                LeanTween.value(gameObject, targetAlpha, 0f, durationOut).setOnUpdate((float value) =>
                {
                    _material.color = ChangeAlpha(_material.color, value);
                });
            }

            foreach (AlphaController alphaController in callChain)
            {
                alphaController.FadeOut();
            }
        }
    }

    private Color ResetAlpha(Color color)
    {
        color.a = 0;
        return color;
    }
    private Color ChangeAlpha(Color color, float alpha)
    {
        color.a = alpha;
        return color;
    }
}
