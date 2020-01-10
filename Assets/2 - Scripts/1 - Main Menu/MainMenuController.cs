using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;
using UnityEngine.UI.Extensions;

public class MainMenuController : MonoBehaviour
{
    [Header("FILE PATHS")] 
    [SerializeField] private string menuJSON = "Menu.json";
    [SerializeField] private string poisJSON = "POIs.json";
    
    [Header("MENU PAGES SETTINGS")]
    public GameObject menuItemSample;
    public HorizontalScrollSnap horizontalScrollSnap;

    [Header("PAGINATION SETTINGS")] 
    public Transform paginationTransform;
    public GameObject paginationSample;

    private List<RectTransform> _pageRectTransforms;
    
    private void Awake() 
    {
        StartCoroutine(RequestPermissions());
    }

    private void Start()
    {
        Initialization();
        
        _pageRectTransforms = new List<RectTransform>();
        foreach (var page in horizontalScrollSnap.ChildObjects)
        {
            var rectTransform = page.GetComponent<RectTransform>();
            _pageRectTransforms.Add(rectTransform);
        }
        
        horizontalScrollSnap.StartingScreen = GlobalParameters.LastMenuPage;
        StartCoroutine(ResizePages());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            OnQuitButtonPressed();
        }
    }

    private IEnumerator ResizePages()
    {
        yield return new WaitForSeconds(0.0001f);
        OnRectTransformDimensionsChange();
    }

    private void OnRectTransformDimensionsChange()
    {
        if (_pageRectTransforms == null) return;
        
        foreach (var page in _pageRectTransforms)
        {
            page.GetComponent<RectTransform>().sizeDelta = Screen.width > Screen.height ? new Vector2(660, 850) : new Vector2(660, 1110);
        }
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
        if (GlobalParameters.POIs.Count < 1)
        {
            foreach (var poi in JsonUtility.FromJson<PoisList>(JsonFileToString(poisJSON)).pois)
            {
                GlobalParameters.POIs.Add(poi.name, new PointOfInterest(poi.name, poi.latitude, poi.longitude, poi.altitude, poi.street));
            }
        }

        foreach (var item in JsonUtility.FromJson<DataList>(JsonFileToString(menuJSON)).data)
        {
            var instance = Instantiate(menuItemSample);
            instance.name = item.title + " (" + item.description + ")";

            Instantiate(paginationSample, paginationTransform);
            
            PointOfInterest poi;
            GlobalParameters.POIs.TryGetValue(item.title, out poi);
            
            var itemController = instance.GetComponent<MenuItemController>();
            itemController.title.text = item.title;
            itemController.description.text = item.description;
            itemController.information.text = item.information;
            itemController.image.sprite = Resources.Load<Sprite>(item.previewImage);
            itemController.icon.sprite = Resources.Load<Sprite>(item.icon);

            var locationProvider = instance.GetComponent<LocationProvider>();
            if (poi != null)
            {
                locationProvider.Location = new Location(poi.latitude, poi.longitude, poi.altitude);
                locationProvider.Name = poi.name;
                locationProvider.Street = poi.street;
                locationProvider.Preview =  itemController.image.sprite;
                locationProvider.SceneName = item.scanningSceneName;
            }
            
            itemController.button.onClick.AddListener(() => LoadScene(item.buttonSceneName, locationProvider));
            
            horizontalScrollSnap.AddChild(instance);
        }
    }

    private string JsonFileToString(string path)
    {
        string loadJsonfile = path.Replace(".json", "");
        TextAsset loadedfile = Resources.Load<TextAsset>(loadJsonfile);
        var jsonstring = loadedfile.text;
        return jsonstring;
    }

    private void LoadScene(string sceneName, LocationProvider locationProvider)
    {
        GlobalParameters.CurrentTracker = locationProvider;
        GlobalParameters.LastMenuPage = horizontalScrollSnap.CurrentPage;
        SceneManager.LoadScene(sceneName);
    }

    public void OnQuitButtonPressed()
    {
        Application.Quit();
    }
}
