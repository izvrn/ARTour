using UnityEngine;
using Wikitude;
using Plane = UnityEngine.Plane;

public class ScaleController : MonoBehaviour
{
    private Camera _mainCamera;
    private TrackingController _controller;
    private Transform _activeObject;

    private Vector3 _touch1StartGroundPosition;
    private Vector3 _touch2StartGroundPosition;
    private Vector3 _startObjectScale;

    private bool _touched;
    private bool _controlsOn;

    private void Start()
    {
        _mainCamera = Camera.main;
        //_controller = FindObjectOfType<TrackingController>();
        //_controller.TrackerChanged.AddListener(OnTrackerChanged);
    }
    
    private void Update()
    {
        if (!_controlsOn)
            return;
        
        if (Input.touchCount >= 2) 
        {
            /* We need at least two touches to perform a scaling gesture */
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            /* If we're currently not scaling any augmentation, do a raycast for each touch position to find one. */
            if (!_touched)
            {
                SetScaleObject();
                
                if (_touched) 
                {
                    _touch1StartGroundPosition = GetGroundPosition(touch1.position);
                    _touch2StartGroundPosition = GetGroundPosition(touch2.position);
                    _startObjectScale = _activeObject.localScale;
                }
            }

            /* If we are scaling an augmentation, do a raycast for each touch position
             * against the ground plane and use the change in the magnitude of the vector between the hits
             * to scale the augmentation.
             */
            if (_touched) 
            {
                var touch1GroundPosition = GetGroundPosition(touch1.position);
                var touch2GroundPosition = GetGroundPosition(touch2.position);

                float startMagnitude = (_touch1StartGroundPosition - _touch2StartGroundPosition).magnitude;
                float currentMagnitude = (touch1GroundPosition - touch2GroundPosition).magnitude;

                _activeObject.localScale = _startObjectScale * (currentMagnitude / startMagnitude);
            }
        } 
        else 
        {
            /* If there are less than two touches on the screen, stop scaling the currently scaled augmentation,
             * if there is one.
             */
            _touched = false;
        }
    }
    
    private void SetScaleObject()
    {
        _touched = true;
    }
    
    private Vector3 GetGroundPosition(Vector2 touchPosition) 
    {
        var groundPlane = new Plane(Vector3.up, Vector3.zero);
        var touchRay = _mainCamera.ScreenPointToRay(touchPosition);
        
        float enter;
        
        if (groundPlane.Raycast(touchRay, out enter)) 
        {
            return touchRay.GetPoint(enter);
        }
        
        return Vector3.zero;
    }

    private void OnTrackerChanged(TrackerBehaviour trackerBehaviour)
    {
        _activeObject = trackerBehaviour.transform.GetChild(0).GetChild(0);
    }
    
    public void ToggleControls()
    {
        _controlsOn = !_controlsOn;
    }
}
