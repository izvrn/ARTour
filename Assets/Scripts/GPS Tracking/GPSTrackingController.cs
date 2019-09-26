using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GPSTrackingController : BaseController
{
    [Header("INFORMATION HEADER SETTINGS")]
    [SerializeField] private Text informationText;
    [SerializeField] private Image informationBackground;

    [Header("CURRENT TRACKER INFORMATION SETTINGS")] 
    [SerializeField] private Text orientationHelperText;

    [Header("GPS TRACKING SETTINGS")] 
    [SerializeField] private float gpsUpdateTime = 1f;
    [SerializeField] private float trackingDistance = 30f;

    private float _currentDistance = 100f;

    protected override void Start()
    {
        base.Start();

        orientationHelperText.text = "HEAD TO: " + Scenes.CurrentTracker.Street;
        orientationHelperText.gameObject.SetActive(true);
        
        GPSTracker.Instance.StatusChanged.AddListener(OnGPSStatusChanged);
        StartCoroutine(DistanceUpdate());
    }

    private IEnumerator DistanceUpdate()
    {
        while (!GPSTracker.Instance.Connected || !Scenes.CurrentTracker)
            yield return null;
        
        while (_currentDistance < trackingDistance)
        {
            _currentDistance = GPSTracker.Instance.CurrentLocation.DistanceTo(Scenes.CurrentTracker.Location);
            informationText.text = "Distance to " + Scenes.CurrentTracker.Name + ": " + Mathf.Round(_currentDistance) + " meters";
            yield return new WaitForSeconds(gpsUpdateTime);
        }
        
        Scenes.Load("Historical Photo Scanning"); 
    }
    
    private void OnGPSStatusChanged()
    {
        informationText.text = GPSTracker.Instance.Status;
    }
}
