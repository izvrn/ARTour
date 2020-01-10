using System;
using System.Collections;
using AHRS;
using UnityEngine;

public class GyroscopeController : MonoBehaviour
{
    [SerializeField] private Transform objectTransform;
    
    private MadgwickAHRS _ahrs;
    private bool _locationServiceAccessGranted;

    private Quaternion _lastQ;

    private void Awake()
    {
        _ahrs = new MadgwickAHRS(1 / 256f, .1f);
        _lastQ = Quaternion.identity;
    }

    private void Start()
    {
        StartCoroutine(StartLocationService());
    }

    private void Update()
    {
        if (_locationServiceAccessGranted)
        {
            print(Input.acceleration * 9.81f);
            UpdateAHRS();
            
            var calcQ = new Quaternion(_ahrs.Quaternion[1], _ahrs.Quaternion[2], _ahrs.Quaternion[3], _ahrs.Quaternion[0]);
            objectTransform.rotation = Quaternion.Inverse(calcQ) * _lastQ;
            objectTransform.Translate(Input.acceleration);

            _lastQ = calcQ;
        }
    }

    private void OnDestroy()
    {
        StopLocationService();
    }

    private IEnumerator StartLocationService()
    {
        // Wait for Unity Remote Initialization
        yield return new WaitForSeconds(4f);
        
        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
            yield break;

        // Start service before querying location
        Input.location.Start();

        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            print("Timed out");
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            print("Unable to determine device location");
            yield break;
        }

        print("Location Granted");
        _locationServiceAccessGranted = true;
        Input.gyro.enabled = true;
        Input.compass.enabled = true;
    }

    private void StopLocationService()
    {
        _locationServiceAccessGranted = false;
        Input.location.Stop();
        Input.gyro.enabled = false;
        Input.compass.enabled = false;
    }

    private void UpdateAHRS()
    {
        _ahrs.Update(
            //GYROSCOPE
            Input.gyro.rotationRate.x,
            Input.gyro.rotationRate.y,
            Input.gyro.rotationRate.z,
            
            //ACCELEROMETER
            Input.acceleration.x,
            Input.acceleration.y,
            Input.acceleration.z,
            
            //MAGNETOMETER
            Input.compass.rawVector.x, 
            Input.compass.rawVector.y,
            Input.compass.rawVector.z
            
            );
    }
}
