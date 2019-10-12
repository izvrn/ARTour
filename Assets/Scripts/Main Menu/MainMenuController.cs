using System.Collections;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI.Extensions;

public class MainMenuController : MonoBehaviour
{
    [Header("MENU PAGES SETTINGS")]
    public GameObject menuItemSample;
    public HorizontalScrollSnap horizontalScrollSnap;

    [Header("PAGINATION SETTINGS")] 
    public GameObject paginationGameObject;
    
    private void Awake() 
    {
        StartCoroutine(RequestPermissions());
    }
    
    private IEnumerator RequestPermissions()
    {
        while (!Permission.HasUserAuthorizedPermission(Permission.Camera)) 
        {
            Permission.RequestUserPermission(Permission.Camera);
            yield return null;
        }
        
        while (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
            yield return null;
        }
    }

    private void Initialization()
    {
        var menuItems = JsonUtility.FromJson<MenuItems>(null);
        
    }
}
