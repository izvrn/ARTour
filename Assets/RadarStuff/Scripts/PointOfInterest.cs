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

    [SerializeField] public Sprite _sourceImage;

    public PointOfInterest (float latitude, float longitude, float altitude, string name)
    {
        this.latitude = latitude;
        this.longitude = longitude;
        this.altitude = altitude;
        this.name = name;
    }
}
