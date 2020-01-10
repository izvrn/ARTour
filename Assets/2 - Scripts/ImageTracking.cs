using UnityEngine;
using UnityEngine.UI;
using Wikitude;

public class ImageTracking : BaseController
{
    public bool extendedTracking = false;

    private MovingController _movingController;
    
    [Header("INFORMATION HEADER SETTINGS")]
    [SerializeField] private Text informationText;
    [SerializeField] private Image informationBackground;
    [SerializeField] private GameObject resetExtendedTrackingButton;
    
    [SerializeField] private Text versionText;

    private void Start()
    {
        if (!versionText)
            versionText = GameObject.FindGameObjectWithTag("VersionText").GetComponent<Text>();

        versionText.text = Application.version;
        
        _movingController = GetComponent<MovingController>();

        if (!extendedTracking && resetExtendedTrackingButton)
        {
            resetExtendedTrackingButton.SetActive(false);
        }
    }

    public void OnTargetRecognized(ImageTarget target)
    {
        if (resetExtendedTrackingButton && extendedTracking) 
            resetExtendedTrackingButton.SetActive(true);

        //_movingController.ObjectTransform = target.Drawable.transform;
        informationText.text = "Target: " + target.Name + " Recognized";
        informationBackground.color = Color.green;
    }

    public void OnTargetLost(ImageTarget target)
    {
        informationText.text = "Target: " + target.Name + " Lost";
        informationBackground.color = Color.red;
    }
    
    public void ResetExtendedTracking(ImageTracker tracker)
    {
        tracker.StopExtendedTracking();
        resetExtendedTrackingButton.SetActive(false);
        
        informationText.text = "Extended Tracking Stopped";
        informationBackground.color = Color.yellow;
    }
    
    public void OnExtendedTrackingQualityChanged(ImageTarget target, ExtendedTrackingQuality oldQuality,
        ExtendedTrackingQuality newQuality)
    {
        switch (newQuality)
        {
            case ExtendedTrackingQuality.Bad:
                informationText.text = "Extended Tracking Quality on Target " + target.Name + " : Bad";
                informationBackground.color = Color.red;
                break;
            case ExtendedTrackingQuality.Average:
                informationText.text = "Extended Tracking Quality on Target " + target.Name + " : Average";
                informationBackground.color = Color.yellow;
                break;
            case ExtendedTrackingQuality.Good:
                informationText.text = "Extended Tracking Quality on Target " + target.Name + " : Good";
                informationBackground.color = Color.green;
                break;
        }
    }
}
