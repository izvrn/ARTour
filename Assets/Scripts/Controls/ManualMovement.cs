using UnityEngine;
using UnityEngine.UI;
using System;
using Wikitude;
using Plane = UnityEngine.Plane;

public class ManualMovement : MonoBehaviour
{
    [Header("UI SETTINGS")]
    [SerializeField] private Text positionText;
    [SerializeField] private Text rotationText;
    [SerializeField] private Text scaleText;

    [Header("CONTROLS SETTINGS")]
    [SerializeField] private TrackingController trackingController;
    [SerializeField] private Slider movementSlider;
    
    private Vector3 _direction;
    private float _scale;
    private float _previousScale;
    private float _previousAngle;
    private bool _flag;
    private bool _movement;

    private Camera _mainCamera;
    private Vector3 _startObjectPosition;
    private Vector2 _startTouchPosition;
    private Vector2 _touchOffset;

    private Transform _objectTransform;
    
    public void Start()
    {
        ResetMoving();
        movementSlider.value = .1f;
        _mainCamera = Camera.main;
        trackingController.trackerChanged.AddListener(OnTrackerChanged);
        
        Debug.LogError("This message will make the console appear in Development Builds");
    }
    
    public void Update()
    {
        if (!_objectTransform)
            return;
        
        //SetTextFields();
        //_objectTransform.localPosition += movementSlider.value * Time.deltaTime * _direction;

        if (Input.touchCount > 0)
        {
            if (Input.touchCount == 1)
            {
                if (!_movement)
                {
                    _startObjectPosition = _objectTransform.position;
                    _startTouchPosition = Input.GetTouch(0).position;
                    _touchOffset = _mainCamera.WorldToScreenPoint(_startObjectPosition);
                    _movement = true;
                }

                MoveObject();
            }
            else
            {
                //_flag = true;
                //ScaleObject(Input.touches);
            }
        }
        else
        {
            //ResetMoving();
            _movement = false;
        }

        //if (!_flag)
        //    WriteLastTouchData(Input.touches);
        
        //_flag = false;       
    }
    
    public void MoveObject(string path)
    {
        switch (path)
        {
            case "up":
                _direction = Vector3.up;
                break;
            case "down":
                _direction = Vector3.down;
                break;
        }
    }

    public void MoveObject()
    {
        Touch touch = Input.GetTouch(0);
        
        var screenPosForRay = (touch.position - _startTouchPosition) + _touchOffset;
        Ray cameraRay = _mainCamera.ScreenPointToRay(screenPosForRay);
        Plane p = new Plane(Vector3.up, Vector3.zero);

        float enter;
        if (p.Raycast(cameraRay, out enter)) 
        {
            var position = cameraRay.GetPoint(enter);

            /* Clamp the new position within reasonable bounds and make sure the augmentation is firmly placed on the ground plane. */
            //position.x = Mathf.Clamp(position.x, -15.0f, 15.0f);
            position.y = 0.0f;
            //position.z = Mathf.Clamp(position.z, -15.0f, 15.0f);

            /* Lerp the position of the dragged augmentation so that the movement appears smoother */
            _objectTransform.position = Vector3.Lerp(_objectTransform.position, position, 0.25f);
        }
    }
    
    public void RotateObject(Slider s)
    {
        if (!_objectTransform)
            return;
        
        _objectTransform.localRotation = Quaternion.Euler(new Vector3(0, s.value, 0));
    }
    
    private void ScaleObject(Touch[] touches)
    {
        if (touches.Length != 2) return;

        _objectTransform.localScale *= Mathf.Sqrt((touches[0].position.x - touches[1].position.x) * (touches[0].position.x - touches[1].position.x) +
                                                 (touches[0].position.y - touches[1].position.y) * (touches[0].position.y - touches[1].position.y)) / _previousScale;
        WriteLastTouchData(Input.touches);
    }
    
    public void ResetMoving()
    {
        _direction = Vector3.zero;
    }

    private void SetTextFields()
    {
        positionText.text = "Position : (" + Math.Round(_objectTransform.localPosition.x, 2)+ " , " + Math.Round(_objectTransform.localPosition.y, 2) + " , " + Math.Round(_objectTransform.localPosition.z, 2) + ")";
        scaleText.text = "Scale : (" + Math.Round(_objectTransform.localScale.x, 2) + ")";
        rotationText.text = "Rotation : (" + (int)_objectTransform.localRotation.eulerAngles.y + ")";
    }
    
    private void WriteLastTouchData(Touch[] touches)
    {
        _previousScale = Mathf.Sqrt((touches[0].position.x - touches[1].position.x) * (touches[0].position.x - touches[1].position.x) +
                                    (touches[0].position.y - touches[1].position.y) * (touches[0].position.y - touches[1].position.y));
    }
    
    public void ToggleControls(Button toggleButton)
    {
        var controlsTransform = toggleButton.transform.parent;

        foreach (Transform contrTrans in controlsTransform)
        {
            if (contrTrans != toggleButton.transform)
            {
                contrTrans.gameObject.SetActive(!contrTrans.gameObject.activeSelf);
            }
        }
    }

    private void OnTrackerChanged(TrackerBehaviour trackerBehaviour)
    {
        _objectTransform = trackerBehaviour.transform.GetChild(0);
    }
}
