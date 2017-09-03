using UnityEngine;
using System.Collections;

public class SpriteController : MonoBehaviour, IFadeble
{
    private SpriteRenderer _sprite;

    private bool _isShow;

    public float duration = 1f;

    private SpriteChanger _spriteChanger;

    void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _spriteChanger = GetComponent<SpriteChanger>();
    }

    public void FadeIn()
    {
        if (_isShow)
        {
            return;
        }

        _isShow = true;

        LeanTween.cancel(gameObject);

        if (_spriteChanger)
        {
            _spriteChanger.Activate();
        }

        Debug.Log(gameObject.name);
        LeanTween.value(gameObject, _sprite.color.a, 1f, duration).setOnUpdate((float value) =>
        {
            Color newColor = _sprite.color;
            newColor.a = value;

            _sprite.color = newColor;
        });
    }

    public void FadeOut()
    {
        if (!_isShow)
        {
            return;
        }

        _isShow = false;

        LeanTween.cancel(gameObject);

        if (_spriteChanger)
        {
            _spriteChanger.Deactivate();
        }

        LeanTween.value(gameObject, _sprite.color.a, 0f, duration).setOnUpdate((float value) =>
        {
            Color newColor = _sprite.color;
            newColor.a = value;

            _sprite.color = newColor;
        });
    }
}
