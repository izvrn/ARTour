using UnityEngine;

public class MathHelper
{
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
}
