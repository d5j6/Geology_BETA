using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteChanger : MonoBehaviour
{
    [SerializeField]
    private Sprite[] _sprites;

    public float delay = 1f;

    private SpriteRenderer _sprite;

    private int _currentSpriteIndex = 0;

    private Coroutine _changeSpriteCoroutine;

    void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
        //Activate();
    }

    public void Activate()
    {
        _changeSpriteCoroutine = StartCoroutine(ChangeSpriteCoroutine());
    }

    private IEnumerator ChangeSpriteCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);

            _sprite.sprite = _sprites[_currentSpriteIndex];

            _currentSpriteIndex = (_currentSpriteIndex == _sprites.Length - 1) ? 0 : _currentSpriteIndex + 1;
        }
    }

    public void Deactivate()
    {
        StopCoroutine(_changeSpriteCoroutine);
    }

    void OnEnable()
    {
        //Activate();
    }
}
