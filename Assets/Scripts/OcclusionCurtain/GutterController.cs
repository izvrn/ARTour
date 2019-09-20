using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GutterController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Image frontPanelImage;
    [SerializeField] private RectTransform gutterRect;
    [SerializeField] private RectTransform bordersRect;

    [SerializeField] private float gutterBordersWidth = 180f;
    [SerializeField] private float gutterWidth = 20f;
    
    private float _lerpParameter = 0.4f;
    private float _timeToHide = 1.5f;
    private float _hideOffset = 40f;
    private float _speed = 40f;

    private float _timer;
    
    private bool _dragging;
    private bool _moving;

    private bool Hidden => gutterRect.anchoredPosition.x < LeftBorder - gutterWidth * .5f;

    protected void Awake()
    {
        bordersRect.sizeDelta = new Vector2(gutterBordersWidth, bordersRect.sizeDelta.y);
        gutterRect.sizeDelta = new Vector2(gutterWidth, gutterRect.sizeDelta.y);
    }

    private void Start()
    {
        _timer = _timeToHide;
        
        var anchoredPosition = gutterRect.anchoredPosition;
        anchoredPosition = new Vector2(LeftBorder - gutterBordersWidth * .5f, anchoredPosition.y);
        gutterRect.anchoredPosition = anchoredPosition;
        
        SwipeManager.Instance.swipeRight.AddListener(OnSwipeRight);
        SwipeManager.Instance.swipeLeft.AddListener(OnSwipeLeft);
    }
    
    private void Update()
    {
        if (Hidden || _moving) return;

        if (_timer < 0)
        {
            StartCoroutine(Hide());
        }
        else if (gutterRect.anchoredPosition.x < gutterBordersWidth + _hideOffset + LeftBorder)
        {
            _timer -= Time.deltaTime;
        }
    }

    private void OnSwipeRight()
    {
        if (Hidden && !_moving)
            StartCoroutine(Show());
    }
    
    private void OnSwipeLeft()
    {
        if (!Hidden && !_moving)
            StartCoroutine(Hide());
    }

    private IEnumerator Hide()
    {
        _moving = true;

        var anchoredPosition = gutterRect.anchoredPosition;
        
        while (anchoredPosition.x > LeftBorder - gutterWidth * .5f && !_dragging)
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
    
    private IEnumerator Show()
    {
        _moving = true;
        var anchoredPosition = gutterRect.anchoredPosition;
        
        while (anchoredPosition.x < LeftBorder + gutterWidth * .5f && !_dragging)
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

    private void OnRectTransformDimensionsChange()
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
