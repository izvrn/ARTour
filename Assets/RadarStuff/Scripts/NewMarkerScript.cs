using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewMarkerScript : MonoBehaviour 
{
    
    [SerializeField] public PointOfInterest _POI;
    [SerializeField] private string _pointName;
    [SerializeField] private Image _locationImage;
    [SerializeField] private Text _distanceText;
    [SerializeField] private GameObject _radarManager;
    [SerializeField] private GameObject _marker;

    private float _distance;
    private float _azimuth;

    public void SetMaterial(Material mat)
    {
        _marker.GetComponent<MeshRenderer>().material = mat;
    }

    private void UpdateData()
    {
        var inLoc = Input.location.lastData;
        var loc = new Location(inLoc.latitude, inLoc.longitude, inLoc.altitude);
        var thisLoc = new Location(_POI.latitude, _POI.longitude, _POI.altitude);
        _azimuth = MathHelper.GetAzimuth(loc, thisLoc) * 180f / Mathf.PI;
        _distance = MathHelper.DistanceTo(loc, thisLoc) / 1000f;
        _distanceText.text = _POI.name[0] + ": " + ((_distance < 1) ? System.Math.Round(_distance*1000f, 0).ToString() + " m" : System.Math.Round(_distance, 1).ToString() + " km");
        _distanceText.color = new Color(Mathf.Clamp(_marker.transform.localPosition.z, 0, 1), Mathf.Clamp(1 - _marker.transform.localPosition.z, 0, 1), 0);
    }

    public void SetImage(Sprite s)
    {
        _locationImage.sprite = s;
    }

    void Start()
    {
        _radarManager = GameObject.FindGameObjectWithTag("RadarController");
        //_locationImage.useSpriteMesh = true;
        //_locationImage.sprite = _POI._sourceImage;
        //Debug.Log(_locationImage.sprite);
        //_marker.GetComponent<MeshRenderer>().material = new Material(_marker.GetComponent<MeshRenderer>().material);
        //_marker.GetComponent<MeshRenderer>().material.color = new Color(Random.Range(0, 1), Random.Range(0, 1), Random.Range(0, 1));
       // Renderer rend = _marker.GetComponent<Renderer>();
        //rend.material = new Material(Shader.Find("Specular"));
        //_marker.GetComponent<MeshRenderer>().material.color = new Color(Random.Range(0, 1), Random.Range(0, 1), Random.Range(0, 1));
    }

    public void DrawMarker()
    {
        UpdateData();
        var _maxDist = _radarManager.GetComponent<ScaleManager>().radarDistance;
//        if (_distance > _maxDist)
//        {
//            gameObject.SetActive(false);
//            return;
//        }
        gameObject.SetActive(true);
        _marker.transform.localPosition = new Vector3(0, 0, Mathf.Clamp01(_distance / _maxDist));
        transform.localRotation = Quaternion.Euler(new Vector3(0, _azimuth + 180, 0));
    }
}
