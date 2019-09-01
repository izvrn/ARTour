using System;
using DG.Tweening;
using UnityEngine;

public class PointerScaler : MonoBehaviour
{
    private float _sizeScaler = 1.5f;
    private float _duration = 0.8f;

    private Vector3 _localScale;

    private void Start()
    {
        _localScale = transform.localScale;
    }

    private void OnEnable()
    {
        StartAnimation();
    }

    private void StartAnimation()
    {
        transform.DOScale(_localScale * _sizeScaler, _duration).SetLoops(-1, LoopType.Yoyo);
    }
}
