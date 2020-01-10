using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadarController : MonoBehaviour
{
    /*
    [Header("RADAR SETTINGS")]
    [SerializeField] private int maxDistance;
    [SerializeField] private Text distanceText;
    
    [Header("RADAR GAMEOBEJCTS")]
    [SerializeField] private GameObject markerSample;
    [SerializeField] private GameObject compassGameObject;
    [SerializeField] private Image compass;
    
    private Location _userLocation;

    private List<MarkerScript> _markers;
    private List<Image> _markerImages;

    private int _length = 180;
    private bool _connected;
    private Vector3 _markerStartPosition;
    
    private GPSTrackingController _trackingController;
    
    private Queue<float> _compassData;
    private float _actualData;
    
    private void Awake()
    {
        _trackingController = GetComponent<GPSTrackingController>();
        _markers = new List<MarkerScript>();
        _markerImages = new List<Image>();
        
        _compassData = new Queue<float>(3);
        _compassData.Enqueue(Input.compass.trueHeading);

        StartCoroutine(AddValueInQueue());
        _actualData = Input.compass.trueHeading + 10f;
    }

    private void Start()
    {
        foreach (var locationProvider in _trackingController.LocationProviders)
        {
            var compassPosition = compassGameObject.transform.position;
            var a = Instantiate(markerSample, new Vector3(compassPosition.x, compassPosition.y, 0), Quaternion.identity, compassGameObject.transform);
            
            var markerScript = a.GetComponent<MarkerScript>();
            
            markerScript.LocationProvider = locationProvider;
            _markers.Add(markerScript);
            _markerImages.Add(a.GetComponent<Image>());
        }
    }
    
    private void Update()
    {
        if (!GPSTracker.Instance.Connected) 
            return;
        
        compass.rectTransform.rotation = Quaternion.Lerp(compass.rectTransform.rotation, Quaternion.Euler(new Vector3(0, 0, _actualData)), Time.deltaTime * 1.3f);
        DrawMarkers();

        foreach (var markerImage in _markerImages)
        {
            markerImage.rectTransform.rotation = Quaternion.identity;
        }
    }
    
    private void DrawMarkers()
    {
        foreach (var marker in _markers)
        {
            marker.DrawMarker(maxDistance, _length);
        }
    }

    private IEnumerator AddValueInQueue()
    {
        _actualData = Input.compass.trueHeading;
        
        yield return new WaitForSeconds(.5f);
        
        _actualData = Input.compass.trueHeading;
        StartCoroutine(AddValueInQueue());
    }
    */
}
