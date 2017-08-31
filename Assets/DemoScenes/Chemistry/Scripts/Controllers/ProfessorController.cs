using UnityEngine;
using System.Collections;
using DG.Tweening;

public class ProfessorController : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private Transform _target;

    public void ActivateGestures()
    {
        float weight = _animator.GetLayerWeight(1);
        DOTween.To(() => { return weight; }, (x) => { weight = x; }, 1f, 1f).OnUpdate(() =>
        {
            _animator.SetLayerWeight(1, weight);
        }).Play();
    }

    public void DeactivateGestures()
    {
        float weight = _animator.GetLayerWeight(1);
        DOTween.To(() => { return weight; }, (x) => { weight = x; }, 0f, 1f).OnUpdate(() =>
        {
            _animator.SetLayerWeight(1, weight);
        }).Play();
    }

    public Transform GetTarget()
    {
        return _target;
    }
}
