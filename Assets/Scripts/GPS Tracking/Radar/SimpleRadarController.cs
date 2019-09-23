using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleRadarController : MonoBehaviour
{
    [Header("RADAR SETTINGS")]
    [SerializeField] private int maxDistance;
    [SerializeField] private Text distanceText;
    
    [Header("RADAR GAMEOBEJCTS")]
    [SerializeField] private GameObject markerSample;
    [SerializeField] private GameObject compassGameObject;
    [SerializeField] private Image compass;
    
    private Location _userLocation;

    private MarkerScript _marker;
    private Image _markerImage;

    private int _length = 180;
    private bool _connected;
    private Vector3 _markerStartPosition;
    
    private GPSTrackingController _trackingController;
    
    private Queue<float> _compassData;
    private float _actualData;
    
    private void Awake()
    {
        _trackingController = GetComponent<GPSTrackingController>();
        
        _compassData = new Queue<float>(3);
        _compassData.Enqueue(Input.compass.trueHeading);

        StartCoroutine(AddValueInQueue());
        _actualData = Input.compass.trueHeading + 10f;
    }

    private void Start()
    {
        _trackingController.TrackerInitialized.AddListener(OnTrackerInitialized);
    }

    private void Update()
    {
        if (!GPSTracker.Instance.Connected || !_trackingController.Initialized) 
            return;
        
        compass.rectTransform.rotation = Quaternion.Lerp(compass.rectTransform.rotation, Quaternion.Euler(new Vector3(0, 0, _actualData)), Time.deltaTime * 1.3f);
  
        Debug.Log(_marker);
        
        _marker.DrawMarker(maxDistance, _length);
        _markerImage.rectTransform.rotation = Quaternion.identity;
    }

    private IEnumerator AddValueInQueue()
    {
        _actualData = Input.compass.trueHeading;
        
        yield return new WaitForSeconds(.5f);
        
        _actualData = Input.compass.trueHeading;
        StartCoroutine(AddValueInQueue());
    }

    public void OnTrackerInitialized()
    {
        var compassPosition = compassGameObject.transform.position;
        var a = Instantiate(markerSample, new Vector3(compassPosition.x, compassPosition.y, 0), Quaternion.identity, compassGameObject.transform);
        
        var markerScript = a.GetComponent<MarkerScript>();
        markerScript.LocationProvider = _trackingController.CurrentTrackerLocationProvider;
        _marker = markerScript;
        _markerImage = a.GetComponent<Image>();
    }
}
