using UnityEngine;
using UnityEngine.UI;

public class OccluderController : MonoBehaviour
{
    [SerializeField] private Image occluderPanelImage;
    [SerializeField] private TogglerElementDragger dragger;

    private void Start()
    {
        dragger.DragEvent.AddListener(OnTogglerDrag);
    }

    private void OnTogglerDrag(float xPosition, float t)
    {
        Debug.Log("CLAMPED X FROM EVENT: " + xPosition);
        
        occluderPanelImage.fillAmount = Mathf.Lerp(occluderPanelImage.fillAmount, xPosition / Screen.width, t);
        
        Debug.Log("FILL AMOUNT : " + occluderPanelImage.fillAmount);
    }
}
