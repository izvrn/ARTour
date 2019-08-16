using UnityEngine;
using UnityEngine.UI;
using Wikitude;

public class SingleObjectTrackingController : SampleController
{
    [SerializeField] private ObjectTracker tracker;
    
    [SerializeField] private Text informationText;
    [SerializeField] private Image informationBackground;

    [SerializeField] private GameObject instructionsGameObject;
    [SerializeField] private GameObject stopExtendedTrackingButton;

    public void OnExtendedTrackingQualityChanged(RecognizedTarget target, ExtendedTrackingQuality oldQuality, ExtendedTrackingQuality newQuality)
    {
        switch (newQuality)
        {
            case ExtendedTrackingQuality.Bad:
                informationText.text = "Extended Tracking Quality on Target " + target.Drawable.name + " : Bad";
                informationBackground.color = Color.red;
                break;
            case ExtendedTrackingQuality.Average:
                informationText.text = "Extended Tracking Quality on Target " + target.Drawable.name + " : Average";
                informationBackground.color = Color.yellow;
                break;
            case ExtendedTrackingQuality.Good:
                informationText.text = "Extended Tracking Quality on Target " + target.Drawable.name + " : Good";
                informationBackground.color = Color.green;
                break;
        }
    }
    
    public void OnTargetRecognized(RecognizedTarget target)
    {
        instructionsGameObject.SetActive(false);
        stopExtendedTrackingButton.SetActive(true);
        informationText.text = "ObjectTarget: " + target.Drawable.name  + " Recognized";
        informationBackground.color = Color.green;
    }

    public void OnTargetLost(RecognizedTarget target)
    {
        informationText.text = "ObjectTarget: " + target.Drawable.name + " Lost";
        informationBackground.color = Color.red;
    }
    
    public void OnStopExtendedTrackingButtonPressed() {
        tracker.StopExtendedTracking();
        stopExtendedTrackingButton.SetActive(false);
        informationText.text = "";
    }
}
