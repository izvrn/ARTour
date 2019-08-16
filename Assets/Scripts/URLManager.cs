using UnityEngine;

public class URLManager : MonoBehaviour
{
    public void OpenURL (string url)
    {
        Application.OpenURL(url);
    }
}
