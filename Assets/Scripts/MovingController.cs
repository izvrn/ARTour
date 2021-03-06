﻿using UnityEngine;
using UnityEngine.UI;

public class MovingController : MonoBehaviour
{
    public Transform ObjectTransform { get; set; }
    
    [Header("CONTROLS OBJECTS FOR TOGGLING")]
    [SerializeField] private GameObject controlsInfoGameObject;
    [SerializeField] private GameObject controlsGameObject;
    
    [Header("TEXT FIELDS")]
    [SerializeField] private Text positionText;
    [SerializeField] private Text rotationText;
    [SerializeField] private Text scaleText;

    [Header("CONTROLS SETTINGS")]
    [SerializeField] private float movementSpeed;

    private Vector3 _direction;
    
    private float _scale;
    private float _previousScale;
    private float _previousAngle;
    
    private bool _flag;
    
    private void Start()
    {
        ResetMoving();
    }
    
    private void Update()
    {
        if (!ObjectTransform)
            return;
        
        SetTextFields();
        
        if (Input.touchCount == 0) 
            ResetMoving();
        
        ObjectTransform.localPosition += Time.deltaTime * movementSpeed * _direction;

        if (!_flag)
            WriteLastTouchData(Input.touches);
        
        if (Input.touchCount == 2)
        {
            _flag = true;
            ScaleObject(Input.touches);
            return;
        }
        
        _flag = false;       
    }

    private void OnEnable()
    {
        if (!ObjectTransform)
            return;
        
        SetTextFields();
    }

    public void MoveObject(string path)
    {
        switch (path)
        {
            case "forward":
                _direction = Vector3.forward;
                break;
            case "back":
                _direction = Vector3.back;
                break;
            case "left":
                _direction = Vector3.left;
                break;
            case "right":
                _direction = Vector3.right;
                break;
            case "up":
                _direction = Vector3.up;
                break;
            case "down":
                _direction = Vector3.down;
                break;
        }
    }

    public void RotateObject(Slider s)
    {
        ObjectTransform.localRotation = Quaternion.Euler(new Vector3(0, s.value, 0));
    }
    
    private void ScaleObject (Touch[] touches)
    {
        if (touches.Length != 2) 
            return;

        ObjectTransform.localScale *= Mathf.Sqrt((touches[0].position.x - touches[1].position.x) * (touches[0].position.x - touches[1].position.x) +
            (touches[0].position.y - touches[1].position.y) * (touches[0].position.y - touches[1].position.y)) / _previousScale;
        WriteLastTouchData(Input.touches);
    }

    public void ResetMoving()
    {
        _direction = Vector3.zero;
    }

    public void Toggle()
    {
        controlsInfoGameObject.SetActive(!controlsInfoGameObject.activeSelf);
        controlsGameObject.SetActive(!controlsGameObject.activeSelf);
    }
    
    private void SetTextFields()
    {
        var localPosition = ObjectTransform.localPosition;
        var localRotation = ObjectTransform.localRotation;
        var localScale = ObjectTransform.localScale;
        
        positionText.text = "X: " + localPosition.x + "\r\nY: " + localPosition.y + "\r\nZ: " + localPosition.z;
        rotationText.text = "X: " + localRotation.x + "\r\nY: " + localRotation.y + "\r\nZ: " + localRotation.z;
        scaleText.text = "X: " + localScale.x + "\r\nY: " + localScale.y + "\r\nZ: " + localScale.z;
    }

    private void WriteLastTouchData(Touch[] touches)
    {
        _previousScale = Mathf.Sqrt((touches[0].position.x - touches[1].position.x) * (touches[0].position.x - touches[1].position.x) +
                                    (touches[0].position.y - touches[1].position.y) * (touches[0].position.y - touches[1].position.y));
    }
}
