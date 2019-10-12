using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PointOfInterest
{
    [Header("POI Data")]
    [SerializeField] public float latitude;

    [SerializeField] public float longitude;

    [SerializeField] public float altitude;

    [SerializeField] public string name;
    
    [SerializeField] public string street;

    [SerializeField] public Sprite previewImage;

    public PointOfInterest (string name, float latitude, float longitude, float altitude, string street, Sprite previewImage)
    {
        this.latitude = latitude;
        this.longitude = longitude;
        this.altitude = altitude;
        this.name = name;
        this.previewImage = previewImage;
        this.street = street;
    }
}
