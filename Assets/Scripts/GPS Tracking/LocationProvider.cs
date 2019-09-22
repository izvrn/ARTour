using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct Location
{
    public double latitude;
    public double longitude;
    public double altitude;

    public Location(double latitude, double longitude, double altitude)
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

    public double DistanceTo(Location other)
    {
        var lat1 = MathHelper.DegreesToRadians(latitude);
        var lat2 = MathHelper.DegreesToRadians(other.latitude);

        var lon1 = MathHelper.DegreesToRadians(longitude);
        var lon2 = MathHelper.DegreesToRadians(other.longitude);

        var deltaLat = lat2 - lat1;
        var deltaLon = lon2 - lon1;

        var a = Math.Sin(deltaLat * 0.5f) * Math.Sin(deltaLat * 0.5f) +
                   Math.Cos(lat1) * Math.Cos(lat2) +
                   Math.Sin(deltaLon * 0.5f) * Math.Sin(deltaLon * 0.5f);
        var c = 2 * Math.Asin(Math.Sqrt(a));

        var r = 6371;

        return c * r;
    }
    
    public double AzimuthTo(Location other)
    {
        var lon1 = longitude;
        var lon2 = other.longitude;
        var lat1 = latitude;
        var lat2 = other.latitude;
        
        var dLat = Math.Abs(lat2 - lat1);
        var dLong = Math.Abs(lon2 - lon1);

        if (lon2 > lon1)
            return (lat2 > lat1) ? 
                Math.PI/2 - Math.Atan(dLat / dLong) :
                Math.PI - Math.Atan(dLong / dLat);
        else
        {
            return (lat2 > lat1) ?
                Math.PI * 3 / 2 + Math.Atan(dLat / dLong) :
                Math.PI + Math.Atan(dLong / dLat);
        }

    }

    public static double DistanceBetween(Location first, Location second)
    {
        return first.DistanceTo(second);
    }

    public static double AzimuthBetween(Location first, Location second)
    {
        return first.AzimuthTo(second);
    }
    
}


public class LocationProvider : MonoBehaviour
{
    [SerializeField] private Location location;
    [SerializeField] private string name;
    [SerializeField] private Sprite preview;
    
    public Location Location => location;
    public string Name => name;
    public Sprite Preview => preview;
}
