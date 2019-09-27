using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleRadarController : MonoBehaviour
{
    [Header("RADAR SETTINGS")]
    [SerializeField] private int maxDistance;
    
    [Header("RADAR GAMEOBEJCTS")]
    [SerializeField] private GameObject markerSample;
    [SerializeField] private GameObject compassGameObject;
    [SerializeField] private Image compass;
    
    private MarkerScript _marker;
    private Image _markerImage;

    private int _length = 180;
    private bool _connected;
    private Vector3 _markerStartPosition;
    
    private Queue<float> _compassData;
    private float _actualData;
    
    private void Awake()
    {
        OnTrackerInitialized();
        
        _compassData = new Queue<float>(3);
        _compassData.Enqueue(Input.compass.trueHeading);

        StartCoroutine(AddValueInQueue());
        _actualData = Input.compass.trueHeading + 10f;
    }

    private void Update()
    {
        if (!GPSTracker.Instance.Connected) 
            return;
        
        compass.rectTransform.rotation = Quaternion.Lerp(compass.rectTransform.rotation, Quaternion.Euler(new Vector3(0, 0, _actualData)), Time.deltaTime * 1.3f);
        
        _marker.DrawMarker(maxDistance, _length);
        _markerImage.rectTransform.rotation = Quaternion.identity;
    }

    private IEnumerator AddValueInQueue()
    {
        _actualData = Input.compass.trueHeading;
        
        yield return new WaitForSeconds(.5f);
        
        _actualData = Input.compass.trueHeading;
        StartCoroutine(AddValueInQueue());
    }

    private void OnTrackerInitialized()
    {
        var compassPosition = compassGameObject.transform.position;
        var a = Instantiate(markerSample, new Vector3(compassPosition.x, compassPosition.y, 0), Quaternion.identity, compassGameObject.transform);
        
        var markerScript = a.GetComponent<MarkerScript>();
        markerScript.LocationProvider = Scenes.CurrentTracker;
        _marker = markerScript;
        _markerImage = a.GetComponent<Image>();
    }
}
