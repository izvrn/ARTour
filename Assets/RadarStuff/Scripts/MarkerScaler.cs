using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerScaler : MonoBehaviour
{
    private float _scale;
    void Awake()
    {
        _scale = transform.localScale.x;
    }

    void Update()
    {
        transform.localScale = _scale * Vector3.one * (1 - transform.localPosition.z);
    }
}
