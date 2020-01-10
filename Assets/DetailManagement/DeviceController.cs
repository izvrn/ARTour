using cakeslice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceController : MonoBehaviour
{

    private bool _added;

    [SerializeField] List<Transform> _deviceDetails;

    [SerializeField] public GameObject ButtonPrefab;

    private void SetDetailsList()
    {
        _added = true;
        foreach (var child in transform)
        {
            var reference = (Transform)child;
            if (reference.GetComponent<MeshRenderer>() != null)
            {
                reference.gameObject.AddComponent<DetailController>();
                _deviceDetails.Add(reference);
            }     
        }
    }

    void OnEnable()
    {
        if (!_added)
        {
            _deviceDetails = new List<Transform>();
            SetDetailsList();
        }       
    }
}
