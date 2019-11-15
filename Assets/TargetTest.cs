using UnityEngine;
using UnityEngine.UI;
using Wikitude;

public class TargetTest : MonoBehaviour
{
    public bool extendedTracking = false;
    [Header("INFORMATION HEADER SETTINGS")]
    [SerializeField] private Text informationText;
    [SerializeField] private Image informationBackground;
    [SerializeField] private GameObject resetExtendedTrackingButton;
    
    public void OnTargetRecognized(ObjectTarget target)
    {
        if (resetExtendedTrackingButton && extendedTracking) 
            resetExtendedTrackingButton.SetActive(true);
        
        informationText.text = "Target: " + target.Name + " Recognized; Scale: " + target.Scale;
        informationBackground.color = Color.green;
    }

    public void OnTargetLost(ObjectTarget target)
    {
        informationText.text = "Target: " + target.Name + " Lost; Scale: " + target.Scale;
        informationBackground.color = Color.red;
    }
    
    
}
