using UnityEngine;
using UnityEngine.UI;

public class MovingScript : MonoBehaviour
{
    [SerializeField] private Transform objectTransform;
    [SerializeField] private Text positionText;
    [SerializeField] private Text rotationText;
    [SerializeField] private Text scaleText;

    [SerializeField] private GameObject textsObject;
    
    [SerializeField] private float movementSpeed;

    private Vector3 _direction;
    
    private float _scale;
    private float _previousScale;
    private float _previousAngle;
    
    private bool _flag;
    
    private void Start()
    {
        ResetMoving();
        movementSpeed = .1f;
    }
    
    private void Update()
    {
        SetTextFields();
        if (Input.touchCount == 0) ResetMoving();
        objectTransform.localPosition += Time.deltaTime * movementSpeed * _direction;

        if ( !_flag == true)
            WriteLastTouchData(Input.touches);
        if (Input.touchCount == 2)
        {
            _flag = true;
            ScaleObject(Input.touches);
            return;
        }
        _flag = false;       
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

    private void ScaleObject (Touch[] touches)
    {
        if (touches.Length != 2) 
            return;

        objectTransform.localScale *= Mathf.Sqrt((touches[0].position.x - touches[1].position.x) * (touches[0].position.x - touches[1].position.x) +
            (touches[0].position.y - touches[1].position.y) * (touches[0].position.y - touches[1].position.y)) / _previousScale;
        WriteLastTouchData(Input.touches);
    }

    public void RotateObject(Slider s)
    {
        objectTransform.localRotation = Quaternion.Euler(new Vector3(0, s.value, 0));
    }

    public void ResetMoving()
    {
        _direction = Vector3.zero;
    }

    public void Toggle()
    {
        textsObject.SetActive(!gameObject.activeSelf);
        gameObject.SetActive(!gameObject.activeSelf);
    }
    
    private void SetTextFields()
    {
        var localPosition = objectTransform.localPosition;
        var localRotation = objectTransform.localRotation;
        var localScale = objectTransform.localScale;
        
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
