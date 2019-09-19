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
    
    public static double GetAzimuth(PointOfInterest userPOI, PointOfInterest poi)
    {
        
        var userLong = userPOI.longitude;
        var objLong = poi.longitude;
        var userLat = userPOI.latitude;
        var objLat = poi.latitude;


        var dLat = Math.Abs(objLat - userLat);
        var dLong = Math.Abs(objLong - userLong);

        if(objLong > userLong)
            return (objLat > userLat) ? 
                Math.PI/2 - Math.Atan(dLat / dLong) :
                Math.PI - Math.Atan(dLong / dLat);
        else
        {
            return (objLat > userLat) ?
                Math.PI * 3 / 2 + Math.Atan(dLat / dLong) :
                Math.PI + Math.Atan(dLong / dLat);
        }

    }
}
