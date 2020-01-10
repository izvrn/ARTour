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

    public void OnTargetReco()
    {
        transform.DOScale(_localScale * _sizeScaler, _duration).SetLoops(-1, LoopType.Yoyo);
    }
    
    public void OnTargetLost()
    {
        transform.DOKill();
    }
}
