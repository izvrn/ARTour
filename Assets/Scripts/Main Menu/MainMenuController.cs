using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;
using UnityEngine.UI.Extensions;

public class MainMenuController : MonoBehaviour
{
    [Header("MENU PAGES SETTINGS")]
    public GameObject menuItemSample;
    public HorizontalScrollSnap horizontalScrollSnap;

    [Header("PAGINATION SETTINGS")] 
    public Transform paginationTransform;
    public GameObject paginationSample;
    
    private void Awake() 
    {
        //StartCoroutine(RequestPermissions());
    }

    private void Start()
    {
        Initialization();
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
        foreach (var poi in JsonUtility.FromJson<PoisList>(JsonFileToString("pois.json")).pois)
        {
            GlobalParameters.POIs.Add(poi.name, new PointOfInterest(poi.name, poi.latitude, poi.longitude, poi.altitude, poi.street, Resources.Load<Sprite>(poi.previewImage)));
        }

        foreach (var item in JsonUtility.FromJson<DataList>(JsonFileToString("menu.json")).data)
        {
            var instance = Instantiate(menuItemSample);
            instance.name = item.title + " (" + item.description + ")";

            Instantiate(paginationSample, paginationTransform);
            var poi = GlobalParameters.POIs[item.title];
            
            var itemController = instance.GetComponent<MenuItemController>();
            itemController.title.text = item.title;
            itemController.description.text = item.description;
            itemController.information.text = item.information;
            itemController.image.sprite = poi.previewImage;
            itemController.icon.sprite = Resources.Load<Sprite>(item.icon);
            itemController.button.onClick.AddListener(() => LoadScene(item.buttonSceneName));

            var locationProvider = instance.GetComponent<LocationProvider>();
            locationProvider.Location = new Location(poi.latitude, poi.longitude, poi.altitude);
            locationProvider.Name = poi.name;
            locationProvider.Street = poi.street;
            locationProvider.Preview =  poi.previewImage;
            locationProvider.SceneName = item.scanningSceneName;
            
            horizontalScrollSnap.AddChild(instance);
        }
    }

    private string JsonFileToString(string path)
    {
        string loadJsonfile = path.Replace(".json", "");
        TextAsset loadedfile = Resources.Load<TextAsset>(loadJsonfile);
        var jsonstring = loadedfile.text;
        Debug.Log("jsonstring:" + jsonstring);
        return jsonstring;
    }

    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
