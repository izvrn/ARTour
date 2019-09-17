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

    private MovingController _movingController;

    private int CurrentTracker
    {
        get
        {
            return _currentTracker;
            
            for (var i = 0; i < _trackers.Count; i++)
            {
                if (_trackers[i].enabled)
                    return i;
            }

            return 0;
        }
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
            _movingController.ObjectTransform = _trackers[CurrentTracker].transform.GetChild(0).GetChild(0);
            informationText.text = "Tracker switched to " + _trackers[CurrentTracker].name;
        }
    }
    
    private void Awake()
    {
        _trackers = FindObjectsOfType<TrackerBehaviour>().ToList();
        _trackers.Sort(new TrackerBehaviourComparer());

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
                informationText.text = "Extended Tracking Quality on Target " + _trackers[CurrentTracker].name + " : Bad";
                informationBackground.color = Color.red;
                break;
            case ExtendedTrackingQuality.Average:
                informationText.text = "Extended Tracking Quality on Target " + _trackers[CurrentTracker].name + " : Average";
                informationBackground.color = Color.yellow;
                break;
            case ExtendedTrackingQuality.Good:
                informationText.text = "Extended Tracking Quality on Target " + _trackers[CurrentTracker].name + " : Good";
                informationBackground.color = Color.green;
                break;
        }
    }

    public void OnTargetRecognized(RecognizedTarget target)
    {
        informationText.text = "Target: " + _trackers[CurrentTracker].name + " Recognized";
        informationBackground.color = Color.green;
    }

    public void OnTargetLost(RecognizedTarget target)
    {
        informationText.text = "Target: " + _trackers[CurrentTracker].name + " Lost";
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
    }

    private IEnumerator Initialization()
    {
        yield return new WaitForSeconds(2f);

        TrackersInitialization();
        
        CurrentTracker = 0;
        horizontalScrollSnap.CurrentPage = 0;
        _movingController.ObjectTransform = _trackers[0].transform.GetChild(0).GetChild(0);
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