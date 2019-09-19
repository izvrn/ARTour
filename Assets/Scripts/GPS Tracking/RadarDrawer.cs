using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;
using UnityEngine.Networking;

public class RadarDrawer : MonoBehaviour
{

    [Header("Radar Settings")]

    [SerializeField] private int _maxDistance;


    public GameObject Compass;

    public List<PointOfInterest> POIs;

    public GameObject MarkerPrefab;

    private List<GameObject> markers;

    private PointOfInterest _myPOI;

    private int length = 180;

    public int distance = 300;


    [SerializeField] Text _notificationText;

    [SerializeField] private Image _mask;
    [SerializeField] private Image _radar;

    public Text DistanceText;


    [SerializeField] private Image _compass;

    private Vector3 markerStartPosition;


    private bool _connected;

    public void ChangeDistance(Slider s)
    {
        distance = (int)s.value;
        DistanceText.text = "GPS radius : " + (distance < 1000 ? distance : distance / 1000).ToString();
        DistanceText.color = new Color(0, 1 - distance / 10000f, distance / 10000f);
    }


    private void Awake()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
        }
        Input.compass.enabled = true;

        markers = new List<GameObject>();
        foreach (var poi in POIs)
        {
            var a = Instantiate(MarkerPrefab, new Vector3(Compass.transform.position.x, Compass.transform.position.y, 0), Quaternion.identity, Compass.transform);
            
            a.GetComponent<MarkerScript>().poi = poi;
            //a.GetComponent<Image>().rectTransform.anchoredPosition = Anchor
            markers.Add(a);


        }
        foreach (var marker in markers)
        {
            marker.GetComponent<MarkerScript>().OnEnabled += DrawPlaces;
            marker.GetComponent<MarkerScript>().OnDisabled += DrawPlaces;
        }
        compassData = new Queue<float>(3);
        compassData.Enqueue(Input.compass.trueHeading);

        StartCoroutine(AddValueInQueue());
        previousData = Input.compass.trueHeading;
        actualData = Input.compass.trueHeading + 10f;

    }

    public Dropdown listDropDown;

    public void DrawPlaces()
    {
        //listDropDown.ClearOptions();
       // foreach (var marker in markers)
        {
           // if (marker.activeSelf)
            {
                //listDropDown.options.Add(new Dropdown.OptionData(marker.GetComponent<MarkerScript>().poi.name));
            }
        }
    }

    // Start is called before the first frame update
    IEnumerator Start()
    {
        if (!Input.location.isEnabledByUser)
            yield break;

        // Start service before querying location
        Input.location.Start(.1f, 0.1f);

        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            _notificationText.text = ("Timed out");
            yield break;
        }
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            _notificationText.text = ("Unable to determine device location");
            yield break;
        }
        else
        {
            // Access granted and location value could be retrieved
            _connected = true;
            StartCoroutine(TextFade());
        }

        
    }

    IEnumerator TextFade()
    {
        _notificationText.color = Color.green;
        _notificationText.text = "GPS tracking enabled !";
        yield return new WaitForSeconds(2f);
        _notificationText.text = "";
    }

    //public float angle = 30;
    public void DrawPOIs (List<PointOfInterest> pois, float trueHeading)
    {
        
        foreach (var marker in markers)
        {
            marker.GetComponent<MarkerScript>().DrawMarker(distance, length);
        }

    }



    float midValue(Queue<float> data)
    {
        float res = 0;
        foreach (var t in data)
        {
            res += t;
        }
        return res / data.Count;
    }

    Queue<float> compassData;
    private float previousData;
    private float actualData;


    IEnumerator AddValueInQueue()
    {
        actualData = Input.compass.trueHeading;
        
        yield return new WaitForSeconds(.5f);
        
        actualData = Input.compass.trueHeading;
        StartCoroutine(AddValueInQueue());
    }

    // Update is called once per frame
    void Update()
    {
        if (_connected)
        {
            {
                _compass.rectTransform.rotation = Quaternion.Lerp(_compass.rectTransform.rotation,
                    Quaternion.Euler(new Vector3(0, 0, actualData)), Time.deltaTime * 1.3f);
            }

            DrawPOIs(POIs, actualData * Mathf.PI / 180f);


            foreach (var marker in markers)
            {
                marker.GetComponent<Image>().rectTransform.rotation = Quaternion.identity;
            }
        }
    }
}
