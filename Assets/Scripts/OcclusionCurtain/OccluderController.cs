using UnityEngine;
using UnityEngine.UI;

public class OccluderController : MonoBehaviour
{
    [SerializeField] private Image occluderPanelImage;
    [SerializeField] private TogglerElementDragger dragger;

    private void Start()
    {
        dragger.dragEvent.AddListener(OnTogglerDrag);
    }

    private void OnTogglerDrag(float xPosition, float t)
    {
        if (xPosition < 0)
            return;
        
        occluderPanelImage.fillAmount = Mathf.Lerp(occluderPanelImage.fillAmount, xPosition / Screen.width, t);
    }
}
