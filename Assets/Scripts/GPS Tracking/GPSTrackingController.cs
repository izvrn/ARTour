using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using Wikitude;

public class GPSTrackingController : BaseController
{
    [Header("INFORMATION HEADER SETTINGS")]
    [SerializeField] private Text informationText;
    [SerializeField] private Image informationBackground;

    [Header("HORIZONTAL SCROLLRECT SETTINGS")] 
    [SerializeField] private HorizontalScrollSnap horizontalScrollSnap;
    [SerializeField] private GameObject itemPrefab;

    private List<TrackerBehaviour> _trackers;
    private List<LocationProvider> _trackerLocationProviders;
    private int _trackerCount;
    private MovingController _movingController;
    
    private TrackerBehaviour CurrentTracker { get; set; }

    private void SetTracker(string trackerName)
    {
        foreach (var tracker in _trackers)
        {
            if (tracker.name == trackerName)
            {
                tracker.enabled = true;
                _movingController.ObjectTransform = tracker.transform.GetChild(0).GetChild(0);
                informationText.text = "Tracker switched to " + tracker.name;

                CurrentTracker = tracker;
                continue;
            }
            
            tracker.enabled = false;
        }
    }

    public List<TrackerBehaviour> Trackers => _trackers;
    public List<LocationProvider> TrackerLocationProviders => _trackerLocationProviders;

    private void Awake()
    {
        _trackers = FindObjectsOfType<TrackerBehaviour>().ToList();
        _trackers.Sort(new TrackerBehaviourComparer());
        _trackerLocationProviders = new List<LocationProvider>();
        _movingController = GetComponent<MovingController>();
    }

    private new void Start()
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
        SetTracker(Settings.CurrentTrackerName);
    }
}
