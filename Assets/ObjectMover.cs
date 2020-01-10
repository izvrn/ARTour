using UnityEngine;
using Wikitude;

public class ObjectMover : MonoBehaviour
{
    [SerializeField] private Transform drawable;

    private Vector3 lastFarPosition = new Vector3(0.151f, 0.116839f, -0.3863331f);
    private Vector3 lastFarScale = new Vector3(0.07817825f, 0.07817825f, 0.07817825f);
    
    private Vector3 lastClosePosition = new Vector3(-0.409f, 0.5f, -1.29201f);
    private Vector3 lastCloseScale = new Vector3(0.3134283f, 0.3134283f, 0.3134283f);
        
    public void OnObjectRecognized(ObjectTarget target)
    {
        if (target.Name == "DullenturmFarLeft")
        {
            drawable.localPosition = lastFarPosition;
            drawable.localScale = lastFarScale;
        }
        else
        {
            drawable.localPosition = lastClosePosition;
            drawable.localScale = lastCloseScale;
        }
    }
    
    public void OnObjectLost(ObjectTarget target)
    {
        if (target.Name == "DullenturmFarLeft")
        {
            lastFarPosition = drawable.transform.localPosition;
            lastFarScale = drawable.transform.localScale;
        }
        else
        {
            lastClosePosition = drawable.transform.localPosition;
            lastCloseScale = drawable.transform.localScale;
        }
    }
}
