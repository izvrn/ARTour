using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void SetTracker(LocationProvider provider)
    {
        Scenes.CurrentTracker = provider;
    }
    
    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}
