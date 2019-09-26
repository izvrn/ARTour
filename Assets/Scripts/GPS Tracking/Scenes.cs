using UnityEngine.SceneManagement;

public static class Scenes
{
    public static LocationProvider CurrentTracker { get; set; }
    
    public static void Load(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    
    /* public static Dictionary<string, dynamic> Parameters { get; private set; }

    public static void Load(string sceneName, Dictionary<string, dynamic> parameters = null)
    {
        Parameters = parameters;
        SceneManager.LoadScene(sceneName);
    }
    
    public static void Load(string sceneName, string paramKey, dynamic paramValue) 
    {
        Parameters = new Dictionary<string, dynamic> {{paramKey, paramValue}};
        SceneManager.LoadScene(sceneName);
    }

    public static dynamic GetParameter(string paramKey)
    {
        return Parameters == null ? "" : Parameters[paramKey];
    }
 
    public static void SetParameter(string paramKey, dynamic paramValue) 
    {
        if (Parameters == null)
            Parameters = new Dictionary<string, dynamic>();
        
        Parameters.Add(paramKey, paramValue); 
    }*/
}
