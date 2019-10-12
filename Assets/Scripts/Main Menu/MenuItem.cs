using System.Collections.Generic;

public class MenuItems
{
    public List<ActData> data;
}

public class ActData
{
    public string title;
    public string description;
    public string information;

    public string buttonSceneName;

    public string imagePath;
    public string iconPath;

    public float latitide;
    public float longitude;
    public string name;
    public string street;
    public string scanningSceneName;

    public override string ToString()
    {
        return "Title";
    }
}
