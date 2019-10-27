using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using Wikitude;

public class InstantTrackingController : BaseController
{
    [Header("INFORMATION HEADER SETTINGS")]
    [SerializeField] private Image informationBackground;
    [SerializeField] private Text informationText;

    [Header("HORIZONTAL SCROLLRECT SETTINGS")] 
    [SerializeField] private HorizontalScrollSnap horizontalScrollSnap;
    [SerializeField] private GameObject itemPrefab;

    private List<TrackerBehaviour> _trackers = new List<TrackerBehaviour>();
    private int _currentTracker;

    private int CurrentTracker
    {
        set
        {
            _currentTracker = value;
            
            if (_trackers.Count < 1)
                return;
            
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
    }

    private new void Start()
    {
        base.Start();
        
        var list = GameObject.FindGameObjectsWithTag("Tracker Behaviour");

        foreach (var item in list)
        {
            _trackers.Add(item.GetComponent<TrackerBehaviour>());
            
            var trackerItem = Instantiate(itemPrefab);
            trackerItem.transform.GetChild(0).GetComponent<Text>().text = item.name;
            horizontalScrollSnap.AddChild(trackerItem);
        }
        
        CurrentTracker = 0;
    }

    public void OnExtendedTrackingQualityChanged(RecognizedTarget target, ExtendedTrackingQuality oldQuality,
        ExtendedTrackingQuality newQuality)
    {
        switch (newQuality)
        {
            case ExtendedTrackingQuality.Bad:
                informationText.text = "Extended Tracking Quality on Target " + target.Drawable + " : Bad";
                informationBackground.color = Color.red;
                break;
            case ExtendedTrackingQuality.Average:
                informationText.text = "Extended Tracking Quality on Target " + target.Drawable + " : Average";
                informationBackground.color = Color.yellow;
                break;
            case ExtendedTrackingQuality.Good:
                informationText.text = "Extended Tracking Quality on Target " + target.Drawable + " : Good";
                informationBackground.color = Color.green;
                break;
        }
    }

    public void OnTargetRecognized(RecognizedTarget target)
    {
        informationText.text = "ObjectTarget: " + target.Drawable + " Recognized";
        informationBackground.color = Color.green;
    }

    public void OnTargetLost(RecognizedTarget target)
    {
        informationText.text = "ObjectTarget: " + target.Drawable + " Lost";
        informationBackground.color = Color.red;
    }

    public void OnScrollRectPageChanged(int page)
    {
        CurrentTracker = page;
        Debug.Log(page);
    }
}
