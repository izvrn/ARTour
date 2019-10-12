using System;
using System.Collections.Generic;

[Serializable]
public class DataList
{
    public List<Datum> data;
}

[Serializable]
public class PoisList
{
    public List<Poi> pois;
}

[Serializable]
public class Poi
{
    public string name;
    public float latitude;
    public float longitude;
    public float altitude;
    public string street;
    public string previewImage;
}

[Serializable]
public class Datum
{
    public string title;
    public string description;
    public string information;
    public string buttonSceneName;
    public string scanningSceneName;
    public string icon;
}