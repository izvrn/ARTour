using UnityEngine;
using UnityEngine.UI;
using System;

public class ChairMovingScript : MonoBehaviour
{
    public Transform objectTransform;
    public Text posText;
    public Text rotText;
    public Text scaText;

    Vector3 _direction;
    float _scale;
    [SerializeField] float _speed;
    public void MoveObject(string path)
    {
        switch (path)
        {
            case "forward" :
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

    void SetTextFields()
    {
        posText.text = "Position : (" + Math.Round(objectTransform.localPosition.x, 2)+ " , " + Math.Round(objectTransform.localPosition.y, 2) + " , " + Math.Round(objectTransform.localPosition.z, 2) + ")";
        scaText.text = "Scale : (" + Math.Round(objectTransform.localScale.x, 2) + ")";
        rotText.text = "Rotation : (" + (int)objectTransform.localRotation.eulerAngles.y + ")";
    }

    public void ChangeScale (Touch[] touches)
    {
        if (touches.Length != 2) return;


        objectTransform.localScale *= Mathf.Sqrt((touches[0].position.x - touches[1].position.x) * (touches[0].position.x - touches[1].position.x) +
            (touches[0].position.y - touches[1].position.y) * (touches[0].position.y - touches[1].position.y)) / _previousScale;
        WriteLastTouchData(Input.touches);
    }

    public void SetRotation(Slider s)
    {
        objectTransform.localRotation = Quaternion.Euler(new Vector3(0, s.value, 0));
    }

    public void ResetMoving()
    {
        _direction = Vector3.zero;
    }

    // Start is called before the first frame update
    void Start()
    {
        ResetMoving();
        _speed = .1f;
    }

    void WriteLastTouchData(Touch[] touches)
    {
        _previousScale = Mathf.Sqrt((touches[0].position.x - touches[1].position.x) * (touches[0].position.x - touches[1].position.x) +
            (touches[0].position.y - touches[1].position.y) * (touches[0].position.y - touches[1].position.y));
    }

    

    bool _flag = false;

    float _previousScale;
    float _previousAngle;

    // Update is called once per frame
    void Update()
    {
        SetTextFields();
        if (Input.touchCount == 0) ResetMoving();
        objectTransform.localPosition += _direction * _speed * Time.deltaTime;

        if ( !_flag == true)
            WriteLastTouchData(Input.touches);
        if (Input.touchCount == 2)
        {
            _flag = true;
            ChangeScale(Input.touches);
            return;
        }
        _flag = false;       
    }

    public void Toggle()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
