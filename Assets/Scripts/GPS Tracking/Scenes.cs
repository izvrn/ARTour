using System.Collections.Generic;
using UnityEngine.SceneManagement;

public static class Scenes
{
    private static Dictionary<string, LocationProvider> _trackers;

    public static LocationProvider GetCurrentTracker(string name)
    {
        if (_trackers == null)
            TrackersInitialization();

        return _trackers[name];
    }
    
    public static LocationProvider CurrentTracker { get; set; }
    
    public static void Load(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    private static void TrackersInitialization()
    {
        _trackers = new Dictionary<string, LocationProvider>();
        
        _trackers["dullenturm"] = new LocationProvider();
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
