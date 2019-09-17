using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GutterSwipeController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Image frontPanelImage;
    [SerializeField] private RectTransform gutterRect;
    [SerializeField] private RectTransform bordersRect;
    
    private float _gutterBordersWidth = 180f;
    private float _gutterWidth = 20f;

    private float _lerpParameter = 1f;
    private float _hideOffset = 40f;

    private float _lastX;
    
    private void Awake()
    {
        bordersRect.sizeDelta = new Vector2(_gutterBordersWidth, bordersRect.sizeDelta.y);
        gutterRect.sizeDelta = new Vector2(_gutterWidth, gutterRect.sizeDelta.y);
    }

    private void Start()
    {
        var anchoredPosition = gutterRect.anchoredPosition;
        anchoredPosition = new Vector2(LeftBorder - _gutterBordersWidth * .5f, anchoredPosition.y);
        gutterRect.anchoredPosition = anchoredPosition;
    }

    private void OnRectTransformDimensionsChange()
    {
        if (!gutterRect)
            return;

        var desiredX = Mathf.Clamp(Screen.width * frontPanelImage.fillAmount - Screen.width * 0.5f, LeftBorder, RightBorder);
        
        gutterRect.anchoredPosition = new Vector2(desiredX, gutterRect.anchoredPosition.y);
        frontPanelImage.fillAmount = Mathf.Lerp(frontPanelImage.fillAmount, desiredX / Screen.width + .5f, 1);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _lastX = eventData.position.x;

        Debug.Log("TEST");
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        var anchoredPosition = gutterRect.anchoredPosition;
        var diff = eventData.position.x - _lastX;
        var desiredX = Mathf.Clamp(anchoredPosition.x + diff, LeftBorder, RightBorder);
        var desiredPosition = new Vector2(desiredX, anchoredPosition.y);

        _lastX = eventData.position.x;

        gutterRect.anchoredPosition = Vector3.Lerp(anchoredPosition, desiredPosition, _lerpParameter);
        frontPanelImage.fillAmount = Mathf.Lerp(frontPanelImage.fillAmount, desiredX / Screen.width + .5f, _lerpParameter);
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {

    }
    
    private float LeftBorder => -Screen.width * .5f - _hideOffset;
    private float RightBorder => Screen.width * .5f + _hideOffset;
}
