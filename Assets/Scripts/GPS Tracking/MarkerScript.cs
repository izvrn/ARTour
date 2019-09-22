using UnityEngine;
using UnityEngine.UI;
using System;

public class MarkerScript : MonoBehaviour
{
    public LocationProvider LocationProvider { get; set; }
    
    public Image markerImage;
    public Text markerText;
    private Vector3 _markerStartPosition;
    
    void Start()
    {
        markerImage = GetComponent<Image>();
        _markerStartPosition = markerImage.rectTransform.localPosition;
    }

    public void DrawMarker(float maxDistance, int length)
    {
        var angle = -((float)MathHelper.GetAzimuth(GPSTracker.Instance.CurrentLocation, LocationProvider.Location) + Mathf.PI / 2);
        var distance = Location.DistanceBetween(GPSTracker.Instance.CurrentLocation, LocationProvider.Location);

        if (distance > maxDistance)
        {
            gameObject.SetActive(false);
            return;
        }
        
        gameObject.SetActive(true);

        markerImage.rectTransform.localPosition = new Vector3((float)(_markerStartPosition.x - (distance > maxDistance ? length : length * (distance / (float)maxDistance)) *
            Math.Cos(angle)), (float)(_markerStartPosition.y - (distance > maxDistance ? length : length * (distance / (float)maxDistance)) * Math.Sin(angle)), 0);
        markerImage.rectTransform.localScale = markerText.rectTransform.localScale = new Vector3((float)(1f - Math.Min(distance, maxDistance) * .5f / maxDistance),
            (float)(1f - Math.Min(distance, maxDistance) * .5f / maxDistance), (float)(1f - Math.Min(distance, maxDistance) * .5f / maxDistance)) ;

        markerText.text = LocationProvider.name + " : " + (int)distance;
        markerText.color = new Color((float)distance / maxDistance, (float)(1 - distance / maxDistance), 0);
    }
}
