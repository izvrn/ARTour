using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TogglerElementDragger : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public DragEvent DragEvent;

    private float _lerpParameter = 0.4f;
    private bool _dragging;
    private RectTransform _rect;

    private void Awake()
    {
        if (DragEvent == null)
            DragEvent = new DragEvent();

        _rect = GetComponent<RectTransform>();
    }

    private void Start()
    {
        var localPosition = transform.localPosition;
        localPosition = new Vector3(LeftBorder, localPosition.y, localPosition.z);
        transform.localPosition = localPosition;
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            if (_dragging)
            {
                var localPosition = transform.localPosition;
                var value = Input.GetTouch(0).position.x - Screen.width / 2f;
                var x = Mathf.Clamp(value, LeftBorder, RightBorder);
                var desiredPosition = Vector2.Lerp(localPosition, new Vector2(x, localPosition.y), _lerpParameter);
                localPosition = new Vector3(desiredPosition.x, desiredPosition.y, localPosition.z);
                transform.localPosition = localPosition;
                
                DragEvent.Invoke(Input.GetTouch(0).position.x, _lerpParameter);
            }
        }
        if (Input.GetMouseButton(0))
        {
            if (_dragging)
            {
                var localPosition = transform.localPosition;
                var value = Input.mousePosition.x - Screen.width / 2f;
                var x = Mathf.Clamp(value, LeftBorder, RightBorder);
                var desiredPosition = Vector2.Lerp(localPosition, new Vector2(x, localPosition.y), _lerpParameter);
                localPosition = new Vector3(desiredPosition.x, desiredPosition.y, localPosition.z);
                transform.localPosition = localPosition;
                
                DragEvent.Invoke(Input.mousePosition.x, _lerpParameter);
            }
        }
    }

    private void OnRectTransformDimensionsChange()
    {
        if (!_rect)
            return;

        var position = transform.localPosition;
        var clampedX = Mathf.Clamp(position.x, LeftBorder, RightBorder);
        position = new Vector3(clampedX, position.y, position.z);
        transform.localPosition = position;

        DragEvent.Invoke(clampedX, 1f);
    }

    public void OnPointerDown(PointerEventData eventData) 
    {
        _dragging = true;
    }

    public void OnPointerUp(PointerEventData eventData) 
    {
        _dragging = false;
    }

    private float LeftBorder => -Screen.width / 2f + _rect.rect.width / 2 - 10;
    private float RightBorder => Screen.width / 2f - _rect.rect.width / 2 + 10;
}

public class DragEvent : UnityEvent<float, float>
{
    
}
