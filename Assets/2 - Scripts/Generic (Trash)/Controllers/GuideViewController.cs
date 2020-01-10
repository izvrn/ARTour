using UnityEngine;
using UnityEngine.UI;

public class GuideViewController : MonoBehaviour
{
    [SerializeField] private bool guideViewAlwaysOff;
    [SerializeField] private Image guideViewImage;

    public void OnTargetReco()
    {
        if (!guideViewAlwaysOff)
            guideViewImage.gameObject.SetActive(false);
    }

    public void OnTargetLost()
    {
        if (!guideViewAlwaysOff)
            guideViewImage.gameObject.SetActive(true);
    }
}
