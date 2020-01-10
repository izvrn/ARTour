using UnityEngine;
using Wikitude;

public class TransformScaler : MonoBehaviour
{
    private const float DEFAULT_MARKER_SIZE = 205f; //size in mm
    private Transform _lastTransform;

    public void OnTrackerRecognized(ImageTarget target)
    {
        var size = target.PhysicalTargetHeight <= 0 ? DEFAULT_MARKER_SIZE : target.PhysicalTargetHeight;
        GetComponent<MovingController>().ObjectTransform.localPosition /= size / 1000f;
        GetComponent<MovingController>().ObjectTransform.localScale /= size / 1000f;
    }
    
    public void OnTrackerLost(ImageTarget target)
    {
        
        var size = target.PhysicalTargetHeight <= 0 ? DEFAULT_MARKER_SIZE : target.PhysicalTargetHeight;
        GetComponent<MovingController>().ObjectTransform.localPosition *= size / 1000f;
        GetComponent<MovingController>().ObjectTransform.localScale *= size / 1000f;
    }
}