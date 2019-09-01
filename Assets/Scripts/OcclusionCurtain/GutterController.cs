using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GutterController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Image frontPanelImage;
    [SerializeField] private RectTransform gutterRect;
    [SerializeField] private RectTransform bordersRect;

    protected float _gutterBordersWidth = 180f;
    protected float _gutterWidth = 20f;
    
    protected float _lerpParameter = 0.4f;
    protected float _timeToHide = 1.5f;
    protected float _hideOffset = 40f;
    protected float _speed = 40f;
    
    protected float _timer;
    
    protected bool _dragging;
    protected bool _moving;

    protected bool Hidden => gutterRect.anchoredPosition.x < LeftBorder - _gutterWidth * .5f;

    protected void Awake()
    {
        bordersRect.sizeDelta = new Vector2(_gutterBordersWidth, bordersRect.sizeDelta.y);
        gutterRect.sizeDelta = new Vector2(_gutterWidth, gutterRect.sizeDelta.y);
    }

    protected void Start()
    {
        _timer = _timeToHide;
        
        var anchoredPosition = gutterRect.anchoredPosition;
        anchoredPosition = new Vector2(LeftBorder - _gutterBordersWidth * .5f, anchoredPosition.y);
        gutterRect.anchoredPosition = anchoredPosition;
        
        SwipeManager.Instance.swipeRight.AddListener(OnSwipeRight);
        SwipeManager.Instance.swipeLeft.AddListener(OnSwipeLeft);
    }
    
    protected void Update()
    {
        if (Hidden || _moving) return;

        if (_timer < 0)
        {
            StartCoroutine(Hide());
        }
        else if (gutterRect.anchoredPosition.x < _gutterBordersWidth + _hideOffset + LeftBorder)
        {
            _timer -= Time.deltaTime;
        }
    }

    protected void OnSwipeRight()
    {
        if (Hidden && !_moving)
            StartCoroutine(Show());
    }
    
    protected void OnSwipeLeft()
    {
        if (!Hidden && !_moving)
            StartCoroutine(Hide());
    }

    protected IEnumerator Hide()
    {
        _moving = true;

        var anchoredPosition = gutterRect.anchoredPosition;
        
        while (anchoredPosition.x > LeftBorder - _gutterWidth * .5f && !_dragging)
        {
            anchoredPosition = gutterRect.anchoredPosition;
            var desiredPosition = new Vector2(anchoredPosition.x - _speed, anchoredPosition.y);
            gutterRect.anchoredPosition = Vector2.Lerp(anchoredPosition, desiredPosition, _lerpParameter);
            
            frontPanelImage.fillAmount = Mathf.Lerp(frontPanelImage.fillAmount, desiredPosition.x / Screen.width + .5f, _lerpParameter);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        
        _moving = false;
        _timer = _timeToHide;
    }
    
    protected IEnumerator Show()
    {
        _moving = true;
        var anchoredPosition = gutterRect.anchoredPosition;
        
        while (anchoredPosition.x < LeftBorder + _gutterWidth * .5f && !_dragging)
        {
            anchoredPosition = gutterRect.anchoredPosition;
            var desiredPosition = new Vector2(anchoredPosition.x + _speed, anchoredPosition.y);
            gutterRect.anchoredPosition = Vector2.Lerp(anchoredPosition, desiredPosition, _lerpParameter);
            
            frontPanelImage.fillAmount = Mathf.Lerp(frontPanelImage.fillAmount, desiredPosition.x / Screen.width + .5f, _lerpParameter);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        
        _moving = false;
        _timer = _timeToHide;
    }

    protected void OnRectTransformDimensionsChange()
    {
        if (!gutterRect)
            return;

        var desiredX = Mathf.Clamp(Screen.width * frontPanelImage.fillAmount  - Screen.width * 0.5f, LeftBorder, RightBorder);
        
        gutterRect.anchoredPosition = new Vector2(desiredX, gutterRect.anchoredPosition.y);
        frontPanelImage.fillAmount = Mathf.Lerp(frontPanelImage.fillAmount, desiredX / Screen.width + .5f, 1);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _timer = _timeToHide;
        _moving = false;
        _dragging = true;
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        var anchoredPosition = gutterRect.anchoredPosition;
        var desiredX = Mathf.Clamp(eventData.position.x - RightBorder, LeftBorder, RightBorder);
        var desiredPosition = new Vector2(desiredX, anchoredPosition.y);
        
        gutterRect.anchoredPosition = Vector2.Lerp(anchoredPosition, desiredPosition, _lerpParameter);
        frontPanelImage.fillAmount = Mathf.Lerp(frontPanelImage.fillAmount, desiredPosition.x / Screen.width + .5f, _lerpParameter);
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        _dragging = false;
    }

    private float LeftBorder => -Screen.width * .5f;
    private float RightBorder => Screen.width * .5f;
}
