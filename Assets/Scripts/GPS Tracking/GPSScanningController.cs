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

    [Header("CURRENT TRACKER INFORMATION SETTINGS")] [SerializeField]
    private GameObject helperObject;
    [SerializeField] private Image markerPreview;

    private MovingController _movingController;
    private bool _initialized;
    
    private LocationProvider _currentLocationProvider;
    private TrackerBehaviour _currentTracker;

    [SerializeField] private Canvas arrowCanvas;
    
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

                _currentTracker = tracker;
                continue;
            }
            
            tracker.enabled = false;
        }
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

        informationText.text = _currentLocationProvider.Name;
        markerPreview.sprite = _currentLocationProvider.Preview;
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
        
        helperObject.SetActive(false);
        arrowCanvas.gameObject.SetActive(true);
    }

    public void OnTargetLost(RecognizedTarget target)
    {
        informationText.text = "Target: " + _currentTracker.name + " Lost";
        informationBackground.color = Color.red;
        
        helperObject.SetActive(true);
        arrowCanvas.gameObject.SetActive(false);
    }

    private IEnumerator Initialization()
    {
        yield return new WaitForSeconds(2f);
        
        SetTracker();
        TrackerInitialized.Invoke();
        _initialized = true;
    }
}
