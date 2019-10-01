using System;
using UnityEngine;

[Serializable]
public struct Location
{
    private const float EARTH_RADIUS = 6371f * 1000;
    
    public float latitude;
    public float longitude;
    public float altitude;

    public Location(float latitude, float longitude, float altitude)
    {
        this.latitude = latitude;
        this.longitude = longitude;
        this.altitude = altitude;
    }

    public Location(LocationInfo locationInfo)
    {
        latitude = locationInfo.latitude;
        longitude = locationInfo.longitude;
        altitude = locationInfo.altitude;
    }

    public float DistanceTo(Location other)
    {
        var lat1 = MathHelper.DegreesToRadians(latitude);
        var lat2 = MathHelper.DegreesToRadians(other.latitude);

        var lon1 = MathHelper.DegreesToRadians(longitude);
        var lon2 = MathHelper.DegreesToRadians(other.longitude);

        var cosL1 = Mathf.Cos(lat1);
        var cosL2 = Mathf.Cos(lat2);
        
        var sinL1 = Mathf.Cos(lon1);
        var sinL2 = Mathf.Cos(lon2);

        var delta = lon2 - lon1;

        var cosDelta = Mathf.Cos(delta);
        var sinDelta = Mathf.Sin(delta);

        var y = Mathf.Sqrt((cosL2 * sinDelta) * (cosL2 * sinDelta) +
                           (cosL1 * sinL2 - sinL1 * cosL2 * cosDelta) * (cosL1 * sinL2 - sinL1 * cosL2 * cosDelta));
        var x = sinL1 * sinL2 + cosL1 * cosL2 * cosDelta;

        return Mathf.Atan2(y, x) * EARTH_RADIUS;
    }
    
    public float DistanceToOld(Location other)
    {
        var lat1 = MathHelper.DegreesToRadians(latitude);
        var lat2 = MathHelper.DegreesToRadians(other.latitude);

        var lon1 = MathHelper.DegreesToRadians(longitude);
        var lon2 = MathHelper.DegreesToRadians(other.longitude);

        var deltaLat = lat2 - lat1;
        var deltaLon = lon2 - lon1;

        var a = Mathf.Sin(deltaLat * 0.5f) * Mathf.Sin(deltaLat * 0.5f) +
                Mathf.Cos(lat1) * Mathf.Cos(lat2) +
                Mathf.Sin(deltaLon * 0.5f) * Mathf.Sin(deltaLon * 0.5f);
        var c = 2 * Mathf.Asin(Mathf.Sqrt(a));

        return c * EARTH_RADIUS;
    }
    
    public float AzimuthTo(Location other)
    {
        var lon1 = longitude;
        var lon2 = other.longitude;
        var lat1 = latitude;
        var lat2 = other.latitude;
        
        var dLat = Mathf.Abs(lat2 - lat1);
        var dLong = Mathf.Abs(lon2 - lon1);

        if (lon2 > lon1)
            return (lat2 > lat1) ? 
                Mathf.PI / 2 - Mathf.Atan(dLat / dLong) :
                Mathf.PI - Mathf.Atan(dLong / dLat);
        
        return (lat2 > lat1) ?
            Mathf.PI * 3 / 2 + Mathf.Atan(dLat / dLong) :
            Mathf.PI + Mathf.Atan(dLong / dLat);
    }

    public static float DistanceBetween(Location first, Location second)
    {
        return first.DistanceTo(second);
    }

    public static float AzimuthBetween(Location first, Location second)
    {
        return first.AzimuthTo(second);
    }

    public override string ToString()
    {
        return latitude + ";" + longitude + ";" + altitude;
    }
}


public class LocationProvider : MonoBehaviour
{
    [SerializeField] private Location location;
    [SerializeField] private string name;
    [SerializeField] private string street;
    [SerializeField] private Sprite preview;

    public Location Location => location;
    public string Name => name;
    public string Street => street;
    public Sprite Preview => preview;

    public override string ToString()
    {
        return location.ToString() + Environment.NewLine + name + Environment.NewLine + Street;
    }
}
