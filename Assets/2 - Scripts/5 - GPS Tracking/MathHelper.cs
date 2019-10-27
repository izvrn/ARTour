using UnityEngine;

public class MathHelper
{
    private const float EARTH_RADIUS = 6371f * 1000;

    public static float DegreesToRadians(float degrees)
    {
        return degrees * Mathf.PI / 180;
    }

    public static float RadiansToDegree(float radians)
    {
        return radians * 180 / Mathf.PI;
    }

    public static float GetAzimuth(Location userLocation, Location location)
    {
        var userLong = userLocation.longitude;
        var objLong = location.longitude;
        var userLat = userLocation.latitude;
        var objLat = location.latitude;

        var dLat = Mathf.Abs(objLat - userLat);
        var dLong = Mathf.Abs(objLong - userLong);

        if (objLong > userLong)
            return (objLat > userLat) ?
                Mathf.PI/2 - Mathf.Atan(dLat / dLong) :
                Mathf.PI - Mathf.Atan(dLong / dLat);

        return (objLat > userLat) ?
            Mathf.PI * 3 / 2 + Mathf.Atan(dLat / dLong) :
            Mathf.PI + Mathf.Atan(dLong / dLat);
    }
    public static float DistanceTo(Location we, Location to)
    {
        var lat1 = DegreesToRadians(we.latitude);
        var lat2 = DegreesToRadians(to.latitude);

        var lon1 = DegreesToRadians(we.longitude);
        var lon2 = DegreesToRadians(to.longitude);

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
}