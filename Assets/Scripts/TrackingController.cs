using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Wikitude;

public class TrackingController : BaseController
{
    [SerializeField] private Text informationText;
    [SerializeField] private Image informationBackground;

    [SerializeField] private Text instructionsText;
    [SerializeField] private Image instructionsImage;

    [SerializeField] private GameObject instructionsGameObject;
    [SerializeField] private Dropdown trackersDropdown;

    private List<TrackerBehaviour> _trackers = new List<TrackerBehaviour>();
    private int _currentTracker;

    private int CurrentTracker
    {
        get => _currentTracker;
        set
        {
            _currentTracker = value;
            
            foreach (var tracker in _trackers)
            {
                tracker.enabled = false;
            }

            instructionsText.text = _trackers[_currentTracker].name;
            _trackers[_currentTracker].enabled = true;
            TrackerChanged.Invoke(_trackers[_currentTracker]);
        }
    }

    public TrackerChangedEvent TrackerChanged;

    private void Awake()
    {
        TrackerChanged = new TrackerChangedEvent();
    }

    private new void Start()
    {
        base.Start();
        
        var list = GameObject.FindGameObjectsWithTag("Tracker Behaviour");

        foreach (var item in list)
        {
            _trackers.Add(item.GetComponent<TrackerBehaviour>());
            trackersDropdown.options.Add(new Dropdown.OptionData(item.name));
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
        instructionsGameObject.SetActive(false);
        informationText.text = "ObjectTarget: " + target.Drawable + " Recognized";
        informationBackground.color = Color.green;
    }

    public void OnTargetLost(RecognizedTarget target)
    {
        informationText.text = "ObjectTarget: " + target.Drawable + " Lost";
        informationBackground.color = Color.red;
    }
    
    public void OnDropdownValueChanged(Dropdown dropdown)
    {
        CurrentTracker = dropdown.value;
    }
}

public class TrackerChangedEvent : UnityEvent<TrackerBehaviour>
{
    
}