using System;

public class MathHelper
{
    public static double DegreesToRadians(double degrees)
    {
        return degrees * Math.PI / 180;
    }

    public static double RadiansToDegree(double radians)
    {
        return radians * 180 / Math.PI;
    }
    
    public static double GetAzimuth(Location userLocation, Location location)
    {
        var userLong = userLocation.longitude;
        var objLong = location.longitude;
        var userLat = userLocation.latitude;
        var objLat = location.latitude;

        var dLat = Math.Abs(objLat - userLat);
        var dLong = Math.Abs(objLong - userLong);

        if (objLong > userLong)
            return (objLat > userLat) ? 
                Math.PI/2 - Math.Atan(dLat / dLong) :
                Math.PI - Math.Atan(dLong / dLat);
        
        return (objLat > userLat) ?
            Math.PI * 3 / 2 + Math.Atan(dLat / dLong) :
            Math.PI + Math.Atan(dLong / dLat);
    }
}
