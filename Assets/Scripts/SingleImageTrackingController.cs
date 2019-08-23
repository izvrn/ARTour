using UnityEngine;
using UnityEngine.UI;
using Wikitude;

public class SingleImageTrackingController : BaseController
{
    [SerializeField] private Text informationText;
    [SerializeField] private Image informationBackground;

    [SerializeField] private GameObject instructionsGameObject;

    public void OnExtendedTrackingQualityChanged(ImageTarget target, ExtendedTrackingQuality oldQuality, ExtendedTrackingQuality newQuality)
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
    
    public void OnTargetRecognized(ImageTarget target)
    {
        instructionsGameObject.SetActive(false);
        informationText.text = "ObjectTarget: " + target.Name  + " Recognized";
        informationBackground.color = Color.green;
    }

    public void OnTargetLost(ImageTarget target)
    {
        informationText.text = "ObjectTarget: " + target.Name + " Lost";
        informationBackground.color = Color.red;
    }
}
