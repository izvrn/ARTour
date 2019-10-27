using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GPSTrackingController : BaseController
{
    [Header("INFORMATION HEADER SETTINGS")]
    [SerializeField] private Text informationText;

    [Header("CURRENT TRACKER INFORMATION SETTINGS")] 
    [SerializeField] private Text orientationHelperText;

    [Header("GPS TRACKING SETTINGS")] 
    [SerializeField] private float gpsUpdateTime = 1f;
    [SerializeField] private float trackingDistance = 30f;

    private float _currentDistance = Mathf.Infinity;

    protected override void Start()
    {
        base.Start();

        orientationHelperText.text = "HEAD TO: " + GlobalParameters.CurrentTracker.Street;
        GPSTracker.Instance.StatusChanged.AddListener(OnGPSStatusChanged);
        
        StartCoroutine(DistanceUpdate());
    }
    
    public override void OnBackButtonClicked()
    {
        SceneManager.LoadScene(GlobalParameters.CurrentTracker.SceneName);
    }

    private IEnumerator DistanceUpdate()
    {
        while (!GPSTracker.Instance.Connected)
            yield return null;
        
        while (_currentDistance > trackingDistance)
        {
            _currentDistance = GPSTracker.Instance.CurrentLocation.DistanceTo(GlobalParameters.CurrentTracker.Location);
            informationText.text = "Distance to " + GlobalParameters.CurrentTracker.Name + ": " + Mathf.Round(_currentDistance) + " meters";
            yield return new WaitForSeconds(gpsUpdateTime);
        }
        
        SceneManager.LoadScene("Historical Photo Scanning");
    }
    
    private void OnGPSStatusChanged()
    {
        informationText.text = GPSTracker.Instance.Status;
    }
}
