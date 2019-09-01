using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using Wikitude;

public class TrackingController : BaseController
{
    [Header("INFORMATION HEADER SETTINGS")]
    [SerializeField] private Text informationText;
    [SerializeField] private Image informationBackground;

    [Header("HORIZONTAL SCROLLRECT SETTINGS")] 
    [SerializeField] private HorizontalScrollSnap horizontalScrollSnap;
    [SerializeField] private GameObject itemPrefab;

    private List<TrackerBehaviour> _trackers;
    private int _trackerCount;
    private int _currentTracker;

    private int CurrentTracker
    {
        set
        {
            if (_currentTracker == value || _trackers.Count < 1)
                return;
            
            _currentTracker = value;
            
            foreach (var tracker in _trackers)
            {
                tracker.enabled = false;
            }
            
            _trackers[_currentTracker].enabled = true;
            trackerChanged.Invoke(_trackers[_currentTracker]);
        }
    }

    public TrackerChangedEvent trackerChanged;

    private void Awake()
    {
        trackerChanged = new TrackerChangedEvent();
        _trackers = FindObjectsOfType<TrackerBehaviour>().ToList();
        _trackers.Sort(new TrackerBehaviourComparer());
    }

    private new void Start()
    {
        base.Start();
        TrackersInitialization();

        CurrentTracker = 0;
    }

    public void OnExtendedTrackingQualityChanged(RecognizedTarget target, ExtendedTrackingQuality oldQuality,
        ExtendedTrackingQuality newQuality)
    {
        switch (newQuality)
        {
            case ExtendedTrackingQuality.Bad:
                informationText.text = "Extended Tracking Quality on Target " + _trackers[_currentTracker].name + " : Bad";
                informationBackground.color = Color.red;
                break;
            case ExtendedTrackingQuality.Average:
                informationText.text = "Extended Tracking Quality on Target " + _trackers[_currentTracker].name + " : Average";
                informationBackground.color = Color.yellow;
                break;
            case ExtendedTrackingQuality.Good:
                informationText.text = "Extended Tracking Quality on Target " + _trackers[_currentTracker].name + " : Good";
                informationBackground.color = Color.green;
                break;
        }
    }

    public void OnTargetRecognized(RecognizedTarget target)
    {
        informationText.text = "Target: " + _trackers[_currentTracker].name + " Recognized";
        informationBackground.color = Color.green;
    }

    public void OnTargetLost(RecognizedTarget target)
    {
        informationText.text = "Target: " + _trackers[_currentTracker].name + " Lost";
        informationBackground.color = Color.red;
    }

    public void OnScrollRectPageChanged(int page)
    {
        CurrentTracker = page;
    }
    
    private void TrackersInitialization()
    {
        horizontalScrollSnap.RemoveAllChildren(out var childrenRemoved);
        
        foreach (var tracker in _trackers)
        {
            var scrollsnapItem = Instantiate(itemPrefab);
            scrollsnapItem.transform.GetChild(0).GetComponent<Text>().text = tracker.name;
            horizontalScrollSnap.AddChild(scrollsnapItem);
        }

        CurrentTracker = 0;
    }
}

public class TrackerChangedEvent : UnityEvent<TrackerBehaviour>
{
    
}

public class TrackerBehaviourComparer : IComparer<TrackerBehaviour>
{
    public int Compare(TrackerBehaviour x, TrackerBehaviour y)
    {
        if (x.name.CompareTo(y.name) != 0)
        {
            return x.name.CompareTo(y.name);
        }
        else
        {
            return 0;
        }
    }
}