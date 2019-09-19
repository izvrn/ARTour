using UnityEngine;
using UnityEngine.UI;
using System;

public class MarkerScript : MonoBehaviour
{
    public LocationProvider locationProvider;

    public Image MarkerImage;

    public Text MarkerText;

    private Vector3 _markerStartPosition;
    
    void Start()
    {
        MarkerImage = GetComponent<Image>();
        _markerStartPosition = MarkerImage.rectTransform.localPosition;
    }

    public void DrawMarker(Location currentLocation, float maxDistance, int length)
    {
        var currentLocatio = new PointOfInterest(tmpPOI.latitude, tmpPOI.longitude, 0, "Me");
        var angle = -((float)MathWorks.GetAzimuth(customPOI, poi) + Mathf.PI / 2);
        var distance = MathWorks.GetDistanceToPOI(customPOI, poi);

        if (distance > maxDistance)
        {
            gameObject.SetActive(false);
            return;
        }
        
        gameObject.SetActive(true);

        MarkerImage.rectTransform.localPosition = new Vector3((float)(_markerStartPosition.x - (distance > maxDistance ? length : length * (distance / (float)maxDistance)) *
            Math.Cos(angle)), (float)(_markerStartPosition.y - (distance > maxDistance ? length : length * (distance / (float)maxDistance)) * Math.Sin(angle)), 0);
        MarkerImage.rectTransform.localScale = MarkerText.rectTransform.localScale = new Vector3((float)(1f - Math.Min(distance, maxDistance) * .5f / maxDistance),
            (float)(1f - Math.Min(distance, maxDistance) * .5f / maxDistance), (float)(1f - Math.Min(distance, maxDistance) * .5f / maxDistance)) ;

        MarkerText.text = poi.name + " : " + (int)distance;
        MarkerText.color = new Color((float)distance / maxDistance, (float)(1 - distance / maxDistance), 0);
    }
}
