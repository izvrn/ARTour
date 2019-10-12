using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GPSMultiTrackingController : BaseController
{
    [Header("INFORMATION HEADER SETTINGS")]
    [SerializeField] private Text informationText;

    [Header("CURRENT TRACKER INFORMATION SETTINGS")] 
    [SerializeField] private Text orientationHelperText;

    [Header("GPS TRACKING SETTINGS")] 
    [SerializeField] private float gpsUpdateTime = 1f;
    [SerializeField] private float trackingDistance = 30f;

    [SerializeField] private bool useFakeGPS;
    [SerializeField] private Location fakeLocation;

    private float _currentDistance = Mathf.Infinity;

    protected override void Start()
    {
        base.Start();
        orientationHelperText.text = "HEAD TO: " + GlobalParameters.CurrentTracker.Street;
        StartCoroutine(DistanceUpdate());
    }
    
    public override void OnBackButtonClicked()
    {
        SceneManager.LoadScene("Main Menu");
    }

    private IEnumerator DistanceUpdate()
    {
        if (!useFakeGPS)
            while (Input.location.status != LocationServiceStatus.Running)
                yield return null;
            
        while (_currentDistance > trackingDistance)
        {
            if (useFakeGPS)
            {
                _currentDistance = MathHelper.DistanceTo(fakeLocation, GlobalParameters.CurrentTracker.Location);
            }
            else
            {
                _currentDistance = MathHelper.DistanceTo(new Location(Input.location.lastData), GlobalParameters.CurrentTracker.Location);
            }
            
            informationText.text = "Distance to " + GlobalParameters.CurrentTracker.Name + ": " + Mathf.Round(_currentDistance) + " meters";
            yield return new WaitForSeconds(gpsUpdateTime);
        }
        
        SceneManager.LoadScene(GlobalParameters.CurrentTracker.SceneName);
    }
}
