using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void SetTracker(LocationProvider provider)
    {
        GlobalParameters.CurrentTracker = provider;
    }
    
    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}
