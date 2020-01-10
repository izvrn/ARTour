using UnityEngine;
using Wikitude;
using Plane = UnityEngine.Plane;

public class MoveController : MonoBehaviour
{
    private Camera _mainCamera;
    private TrackingController _controller;
    private Transform _activeObject;

    private Vector3 _startObjectPosition;
    private Vector2 _startTouchPosition;
    private Vector2 _touchOffset;

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
        
        if (Input.touchCount > 0) 
        {
            Touch touch = Input.GetTouch(0);

            /* If we're currently not dragging any augmentation, set _touched true. */
            if (!_touched) 
            {
                SetMoveObject();
            }

            /* If we are dragging an augmentation, raycast against the ground plane to find how the augmentation should be moved. */
            if (_touched) 
            {
                var screenPosForRay = (touch.position - _startTouchPosition) + _touchOffset;
                Ray cameraRay = _mainCamera.ScreenPointToRay(screenPosForRay);
                Plane p = new Plane(Vector3.up, Vector3.zero);

                float enter;
                if (p.Raycast(cameraRay, out enter)) 
                {
                    var position = cameraRay.GetPoint(enter);

                    /* Clamp the new position within reasonable bounds and make sure the augmentation is firmly placed on the ground plane. */
                    position.x = Mathf.Clamp(position.x, -15.0f, 15.0f);
                    position.y = 0.0f;
                    position.z = Mathf.Clamp(position.z, -15.0f, 15.0f);

                    /* Lerp the position of the dragged augmentation so that the movement appears smoother */
                    _activeObject.localPosition = Vector3.Lerp(_activeObject.localPosition, position, 0.25f);
                }
            }
        } 
        else 
        {
            /* If there are no touches, stop dragging the currently dragged augmentation, if there is one. */
            _touched = false;
        }
    }
    
    private void SetMoveObject()
    {
        _touched = true;
        
        _startObjectPosition = _activeObject.localPosition;
        _startTouchPosition = Input.GetTouch(0).position;
        _touchOffset = _mainCamera.WorldToScreenPoint(_startObjectPosition);
    }

    private void OnTrackerChanged(TrackerBehaviour trackerBehaviour)
    {
        _activeObject = trackerBehaviour.transform;
    }

    public void ToggleControls()
    {
        _controlsOn = !_controlsOn;
    }
}
