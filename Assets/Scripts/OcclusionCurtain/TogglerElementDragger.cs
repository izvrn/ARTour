using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TogglerElementDragger : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Image occluderPanelImage;
    public DragEvent dragEvent;
    
    private float _lerpParameter = 0.4f;
    private float _timeToHide = 4f;
    private float _hideOffset = 50;
    private float _speed = 40f;
    private float _touchOffset = 15f;
    
    private float _timer;
    
    private bool _dragging;
    private bool _moving;

    private bool _hidden;
    private RectTransform _rect;

    private float Width => _rect.rect.width;
    private float Height => _rect.rect.height;

    private void Awake()
    {
        if (dragEvent == null)
            dragEvent = new DragEvent();

        _rect = GetComponent<RectTransform>();
    }

    private void Start()
    {
        _timer = _timeToHide;
        
        var anchoredPosition = _rect.anchoredPosition;
        anchoredPosition = new Vector2(LeftBorder, anchoredPosition.y);
        _rect.anchoredPosition = anchoredPosition;
        
        SwipeManager.Instance.swipeRight.AddListener(OnSwipeRight);
        SwipeManager.Instance.swipeLeft.AddListener(OnSwipeLeft);
    }
    
    private void Update()
    {
        var anchoredPosition = _rect.anchoredPosition;
        
        if (_dragging)
        {
            _timer = _timeToHide;
            
            float value; 
            
            if (Input.touchCount > 0)
            {
                value = Input.GetTouch(0).position.x;
            }
            else
            {
                value = Input.mousePosition.x;
            }
            
            var clampedValue = Mathf.Clamp(value, LeftBorder, RightBorder);
            var desiredPosition = Vector2.Lerp(anchoredPosition, new Vector2(clampedValue, anchoredPosition.y), _lerpParameter);
            _rect.anchoredPosition = desiredPosition;
                
            dragEvent.Invoke(clampedValue, _lerpParameter);
        }
        else
        {
            if (!_hidden && !_moving && _timer < 0)
            {
                StartCoroutine(Hide());
            }
            
            if (!_hidden && !_moving && anchoredPosition.x < Width + _hideOffset)
                _timer -= Time.deltaTime;
        }
    }

    private void OnSwipeRight()
    {
        if (_hidden && !_moving)
            StartCoroutine(Show());
    }
    
    private void OnSwipeLeft()
    {
        if (!_hidden && !_moving)
            StartCoroutine(Hide());
    }

    private IEnumerator Hide()
    {
        _moving = true;

        var anchoredPosition = _rect.anchoredPosition;
        
        while (anchoredPosition.x > - Width / 2 && !_dragging)
        {
            anchoredPosition = _rect.anchoredPosition;
            var desiredPos = new Vector2(anchoredPosition.x - _speed, anchoredPosition.y);
            _rect.anchoredPosition = Vector2.Lerp(anchoredPosition, desiredPos, _lerpParameter);
            
            dragEvent.Invoke(Mathf.Clamp(desiredPos.x, 0, Screen.width), _lerpParameter);
            
            yield return new WaitForSeconds(Time.deltaTime);
        }

        _timer = _timeToHide;
        _moving = false;
        _hidden = true;
    }
    
    private IEnumerator Show()
    {
        _moving = true;
        var anchoredPosition = _rect.anchoredPosition;
        
        while (anchoredPosition.x < Width / 2 - 10 && !_dragging)
        {
            anchoredPosition = _rect.anchoredPosition;
            var desiredPos = new Vector2(anchoredPosition.x + _speed, anchoredPosition.y);
            _rect.anchoredPosition = Vector2.Lerp(anchoredPosition, desiredPos, _lerpParameter);
            
            dragEvent.Invoke(Mathf.Clamp(desiredPos.x, 0, Screen.width), _lerpParameter);
            
            yield return new WaitForSeconds(Time.deltaTime);
        }
        
        _timer = _timeToHide;
        _moving = false;
        _hidden = false;
    }

    private void OnRectTransformDimensionsChange()
    {
        if (!_rect)
            return;

        var xClamped = Mathf.Clamp(Screen.width * occluderPanelImage.fillAmount, LeftBorder, RightBorder);
        _rect.anchoredPosition = new Vector2(xClamped, _rect.anchoredPosition.y);
        
        dragEvent.Invoke(-1f, 1f);
    }

    public void OnPointerDown(PointerEventData eventData) 
    {
        _dragging = true;
    }

    public void OnPointerUp(PointerEventData eventData) 
    {
        _dragging = false;
    }

    private float LeftBorder => Width / 2 + 10;
    private float RightBorder => Screen.width - Width / 2 - 10;
}

public class DragEvent : UnityEvent<float, float>
{
    
}
