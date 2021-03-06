﻿using System.Collections;
using UnityEngine;

public class GPSTracker : MonoBehaviour
{
    public static GPSTracker Instance { get; set; }

    private float _desiredAccuracyInMeters;
    private float _updateDistanceInMeters;
    private float _updateTime;

    private bool _serviceGranted;

    private TrackingController _trackingController;

    public int CurrentTarget { get; set; } = -1;
    public bool Connected => _serviceGranted;

    private void Awake()
    {
        if (!Instance)
            Instance = this;
        
        _trackingController = GetComponent<TrackingController>();
    }

    private IEnumerator Start()
    {
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("GPS is off");
            yield break;
        }
        
        Input.location.Start(_desiredAccuracyInMeters, _updateDistanceInMeters);
        
        int initializationTimer = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && initializationTimer > 0)
        {
            yield return new WaitForSeconds(1f);
            initializationTimer--;
        }

        if (initializationTimer < 1)
        {
            Debug.Log("Timed out");
            yield break;
        }

        Input.compass.enabled = true;
        _serviceGranted = true;
    }
    
    private void Update()
    {
        
    }

    private IEnumerator UpdateDistance()
    {
        yield return new WaitForSeconds(_updateTime);
    }
    

    public Location CurrentLocation => new Location(Input.location.lastData);
}
