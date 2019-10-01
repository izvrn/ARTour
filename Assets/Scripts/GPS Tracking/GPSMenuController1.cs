using System.Collections;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using Wikitude;

public class GPSMenuController1 : MonoBehaviour
{
    public HorizontalScrollSnap HorizontalScrollSnap;
    
    public GameObject InfoPanel;

    public Text VersionNumberText;
    public Text BuildDateText;
    public Text BuildNumberText;
    public Text BuildConfigurationText;
    public Text UnityVersionText;
    
    private void Awake()
    {
        StartCoroutine(RequestPermissions());
    }

    public void OnSampleButtonClicked(LocationProvider locationProvider) 
    {
        /* Start the appropriate scene based on the button name that was pressed. */
        Scenes.CurrentTracker = locationProvider;
        Scenes.Load("Historical Photo GPS Tracking");
    }

    public void OnInfoButtonPressed() 
    {
        /* Display the info panel, which contains additional information about the Wikitude SDK. */
        InfoPanel.SetActive(true);

        var buildInfo = WikitudeSDK.BuildInformation;
        VersionNumberText.text = buildInfo.SDKVersion;
        BuildDateText.text = buildInfo.BuildDate;
        BuildNumberText.text = buildInfo.BuildNumber;
        BuildConfigurationText.text = buildInfo.BuildConfiguration;
        UnityVersionText.text = Application.unityVersion;
    }

    public void OnInfoDoneButtonPressed() 
    {
        InfoPanel.SetActive(false);
    }

    public void OnToggleValueChanged()
    {
        
    }

    void Update() 
    {
        /* Also handles the back button on Android */
        if (Input.GetKeyDown(KeyCode.Escape)) {
            /* There is nowhere else to go back, so quit the app. */
            Application.Quit();
        }
    }

    IEnumerator RequestPermissions()
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
    
}
