using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Wikitude;

public class GPSScanningController : BaseController
{
    [Header("INFORMATION HEADER SETTINGS")]
    [SerializeField] private Text informationText;
    [SerializeField] private Image informationBackground;

    [Header("CURRENT TRACKER INFORMATION SETTINGS")] 
    [SerializeField] private GameObject scanHelper;
    [SerializeField] private Image markerPreview;

    private MovingController _movingController;
    private bool _initialized;
    
    private LocationProvider _currentLocationProvider;
    private TrackerBehaviour _currentTracker;
    
    public bool Initialized => _initialized;
    public UnityEvent TrackerInitialized { get; private set; }

    private void SetTracker()
    {
        var _trackers = FindObjectsOfType<TrackerBehaviour>().ToList();
        
        foreach (var tracker in _trackers)
        {
            if (tracker.name == _currentLocationProvider.Name)
            {
                tracker.enabled = true;
                _movingController.ObjectTransform = tracker.transform.GetChild(0).GetChild(0);
                informationText.text = "Tracker switched to " + tracker.name;

                _currentTracker = tracker;
                continue;
            }
            
            tracker.enabled = false;
        }

        markerPreview.sprite = _currentLocationProvider.Preview;
    }

    private void Awake()
    {
        TrackerInitialized = new UnityEvent();
        _movingController = GetComponent<MovingController>();
        _currentLocationProvider = Scenes.CurrentTracker;
    }

    protected override void Start()
    {
        base.Start();
        StartCoroutine(Initialization());
    }

    public void OnExtendedTrackingQualityChanged(RecognizedTarget target, ExtendedTrackingQuality oldQuality,
        ExtendedTrackingQuality newQuality)
    {
        switch (newQuality)
        {
            case ExtendedTrackingQuality.Bad:
                informationText.text = "Extended Tracking Quality on Target " + _currentTracker.name + " : Bad";
                informationBackground.color = Color.red;
                break;
            case ExtendedTrackingQuality.Average:
                informationText.text = "Extended Tracking Quality on Target " + _currentTracker.name + " : Average";
                informationBackground.color = Color.yellow;
                break;
            case ExtendedTrackingQuality.Good:
                informationText.text = "Extended Tracking Quality on Target " + _currentTracker.name + " : Good";
                informationBackground.color = Color.green;
                break;
        }
    }

    public void OnTargetRecognized(RecognizedTarget target)
    {
        informationText.text = "Target: " + _currentTracker.name + " Recognized";
        informationBackground.color = Color.green;
    }

    public void OnTargetLost(RecognizedTarget target)
    {
        informationText.text = "Target: " + _currentTracker.name + " Lost";
        informationBackground.color = Color.red;
    }

    private IEnumerator Initialization()
    {
        yield return new WaitForSeconds(2f);
        
        SetTracker();
        TrackerInitialized.Invoke();
        _initialized = true;
    }
}
