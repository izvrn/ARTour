using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GPSTracker : MonoBehaviour
{
    [SerializeField] private bool useFakeGPS;
    [SerializeField] private Location fakeCoordinates;
    public static GPSTracker Instance { get; set; }

    private float _desiredAccuracyInMeters = 10f;
    private float _updateDistanceInMeters = 1f;
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
        if (useFakeGPS)
        {
            Status = "Fake GPS Service granted!";
            StatusChanged.Invoke();
            _serviceGranted = true;
            yield break;
        }
        
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

    public Location CurrentLocation => useFakeGPS ? fakeCoordinates : new Location(Input.location.lastData);
    public UnityEvent StatusChanged { get; set; }
}
