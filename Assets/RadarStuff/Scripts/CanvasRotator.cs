using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasRotator : MonoBehaviour
{
    private GameObject _camera;

    void Awake()
    {
        _camera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    void Update()
    {
        if (gameObject.active)
        {
            transform.LookAt(_camera.transform);
        }
    }
}
