using System.Collections.Generic;
using UnityEngine.SceneManagement;

public static class Scenes
{
    public static Dictionary<string, string> Parameters { get; private set; }

    public static void Load(string sceneName, Dictionary<string, string> parameters = null)
    {
        Parameters = parameters;
        SceneManager.LoadScene(sceneName);
    }
    
    public static void Load(string sceneName, string paramKey, string paramValue) 
    {
        Parameters = new Dictionary<string, string> {{paramKey, paramValue}};
        SceneManager.LoadScene(sceneName);
    }
    
    public static void Load(string sceneName, params string[][] parameters) 
    {
        Parameters = new Dictionary<string, string>();

        foreach (var parameter in parameters)
        {
            Parameters.Add(parameter[0], parameter[1]);
        }
        
        SceneManager.LoadScene(sceneName);
    }

    public static string GetParameter(string paramKey)
    {
        return Parameters == null ? "" : Parameters[paramKey];
    }
 
    public static void SetParameter(string paramKey, string paramValue) 
    {
        if (Parameters == null)
            Parameters = new Dictionary<string, string>();
        
        Parameters.Add(paramKey, paramValue);
    }
}
