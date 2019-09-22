using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Wikitude;

public class GPSTrackingController : BaseController
{
    [Header("INFORMATION HEADER SETTINGS")]
    [SerializeField] private Text informationText;
    [SerializeField] private Image informationBackground;

    [Header("CURRENT TRACKER INFORMATION SETTINGS")] 
    [SerializeField] private GameObject scanHelper;
    [SerializeField] private Image markerPreview;

    [Header("GPS TRACKING SETTINGS")] 
    [SerializeField] private float gpsUpdateTime = 1f;
    [SerializeField] private float trackingDistance = 30f;

    private List<TrackerBehaviour> _trackers;
    private MovingController _movingController;

    private float _currentDistance;
    private bool _initialized;
    
    private TrackerBehaviour CurrentTracker { get; set; }
    private LocationProvider TrackerLocationProvider { get; set; }
    public List<LocationProvider> LocationProviders { get; set; }

    private void SetTracker()
    {
        foreach (var tracker in _trackers)
        {
            var locationProvider = tracker.GetComponent<LocationProvider>();
            
            if (tracker.name == Scenes.GetParameter("CurrentTrackerName"))
            {
                tracker.enabled = true;
                _movingController.ObjectTransform = tracker.transform.GetChild(0).GetChild(0);
                informationText.text = "Tracker switched to " + tracker.name;

                CurrentTracker = tracker;
                TrackerLocationProvider = locationProvider;
                
                _movingController.ObjectTransform = tracker.transform.GetChild(0).GetChild(0);
                informationText.text = "Tracker switched to " + tracker.name;
                
                LocationProviders.Add(locationProvider);
                continue;
            }
            
            LocationProviders.Add(locationProvider);
            tracker.enabled = false;
        }

        markerPreview.sprite = TrackerLocationProvider.Preview;
    }

    private void Awake()
    {
        _trackers = FindObjectsOfType<TrackerBehaviour>().ToList();
        _trackers.Sort(new TrackerBehaviourComparer());
        LocationProviders = new List<LocationProvider>();
        _movingController = GetComponent<MovingController>();
    }

    protected override void Start()
    {
        base.Start();
        
        GPSTracker.Instance.StatusChanged.AddListener(OnGPSStatusChanged);
        StartCoroutine(Initialization());
        StartCoroutine(DistanceUpdate());
        
        //TODO: Stop Wikitude Camera Processing for battery saving
    }

    protected override void Update()
    {
        base.Update();
        
        if (!GPSTracker.Instance.Connected || !_initialized)
            return;

        if (_currentDistance < trackingDistance)
        {
            scanHelper.SetActive(true);
            //TODO: Start Wikitude Camera Processing
        }
        else
        {
            scanHelper.SetActive(false);
        }
    }

    public void OnExtendedTrackingQualityChanged(RecognizedTarget target, ExtendedTrackingQuality oldQuality,
        ExtendedTrackingQuality newQuality)
    {
        switch (newQuality)
        {
            case ExtendedTrackingQuality.Bad:
                informationText.text = "Extended Tracking Quality on Target " + CurrentTracker.name + " : Bad";
                informationBackground.color = Color.red;
                break;
            case ExtendedTrackingQuality.Average:
                informationText.text = "Extended Tracking Quality on Target " + CurrentTracker.name + " : Average";
                informationBackground.color = Color.yellow;
                break;
            case ExtendedTrackingQuality.Good:
                informationText.text = "Extended Tracking Quality on Target " + CurrentTracker.name + " : Good";
                informationBackground.color = Color.green;
                break;
        }
    }

    public void OnTargetRecognized(RecognizedTarget target)
    {
        informationText.text = "Target: " + CurrentTracker.name + " Recognized";
        informationBackground.color = Color.green;
    }

    public void OnTargetLost(RecognizedTarget target)
    {
        informationText.text = "Target: " + CurrentTracker.name + " Lost";
        informationBackground.color = Color.red;
    }

    private IEnumerator Initialization()
    {
        yield return new WaitForSeconds(2f);
        SetTracker();
        _initialized = true;
    }

    private IEnumerator DistanceUpdate()
    {
        while (!_initialized)
            yield return null;

        while (true)
        {
            _currentDistance = GPSTracker.Instance.CurrentLocation.DistanceTo(TrackerLocationProvider.Location);
            informationText.text = "Distance to " + TrackerLocationProvider.Name + ": " + Mathf.Round(_currentDistance) + " meters";
            yield return new WaitForSeconds(gpsUpdateTime);
        }
    }
    
    private void OnGPSStatusChanged()
    {
        informationText.text = GPSTracker.Instance.Status;
    }
}
