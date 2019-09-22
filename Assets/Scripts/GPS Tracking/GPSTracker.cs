using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GPSTracker : MonoBehaviour
{
    public static GPSTracker Instance { get; set; }

    private float _desiredAccuracyInMeters;
    private float _updateDistanceInMeters;
    private bool _serviceGranted;
    
    public bool Connected => _serviceGranted;
    public string Status { get; set; }

    private void Awake()
    {
        Instance = this;
        StatusChanged = new UnityEvent();
    }

    private IEnumerator Start()
    {
        while (!Input.location.isEnabledByUser)
        {
            Status = "Please, enable GPS";
            StatusChanged.Invoke();
            yield return null;
        }
        
        Input.location.Start(_desiredAccuracyInMeters, _updateDistanceInMeters);
        
        var initializationTimer = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && initializationTimer > 0)
        {
            yield return new WaitForSeconds(1f);
            initializationTimer--;
        }

        if (initializationTimer < 1)
        {
            Status = "Timed out. Restart application";
            StatusChanged.Invoke();
            yield break;
        }

        Status = "GPS Service granted!";
        StatusChanged.Invoke();
        
        Input.compass.enabled = true;
        _serviceGranted = true;
    }

    public Location CurrentLocation => new Location(Input.location.lastData);
    public UnityEvent StatusChanged { get; set; }
}
