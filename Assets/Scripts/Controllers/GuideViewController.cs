using UnityEngine;
using UnityEngine.UI;

public class GuideViewController : MonoBehaviour
{
    [SerializeField] private Image guideViewImage;

    public void OnTargetReco()
    {
        guideViewImage.gameObject.SetActive(false);
    }

    public void OnTargetLost()
    {
        guideViewImage.gameObject.SetActive(true);
    }
}
