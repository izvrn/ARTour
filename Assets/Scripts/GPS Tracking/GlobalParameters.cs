using System.Collections.Generic;

public static class GlobalParameters
{
    public static Dictionary<string, PointOfInterest> POIs { get; set; } = new Dictionary<string, PointOfInterest>();
    public static LocationProvider CurrentTracker { get; set; }
}
