using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAnimation : MonoBehaviour
{
    [SerializeField] private List<Transform> _animationPoints;

    private Transform _currentTargetTransform;

    private bool _animationFinished = false;

    // Start is called before the first frame update
    void Awake()
    {
        SetTransform(_animationPoints[0]);
        StartCoroutine(Animation());
    }

    private void SetTransform (Transform t)
    {
        transform.position = t.position;
        transform.rotation = t.rotation;
    }

    private void SetTransform(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;
    }

    private IEnumerator Animation()
    {
        for (int i = 0; i < _animationPoints.Count; i++)
        {
            _currentTargetTransform = _animationPoints[i];
            yield return new WaitForSeconds(.8f);
        }
        yield return new WaitForSeconds(4f);
        _animationFinished = true;
        StopAllCoroutines();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_animationFinished)
        {
            SetTransform(Vector3.Lerp(transform.position, _currentTargetTransform.position, .8f * Time.deltaTime), 
                Quaternion.Lerp(transform.rotation, _currentTargetTransform.rotation, .8f * Time.deltaTime));
        }  
    }
}
