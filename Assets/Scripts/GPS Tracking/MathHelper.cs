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
}
