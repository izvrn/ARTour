﻿using System.Collections;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;
using Wikitude;

public class GPSMenuController : MonoBehaviour
{
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
    
    void Update() 
    {
        /* Also handles the back button on Android */
        if (Input.GetKeyDown(KeyCode.Escape)) {
            /* There is nowhere else to go back, so quit the app. */
            Scenes.Load("Main Menu");
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
