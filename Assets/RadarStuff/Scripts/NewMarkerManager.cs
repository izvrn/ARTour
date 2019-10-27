using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NewMarkerManager : MonoBehaviour
{
    [SerializeField] GameObject _arrow;
    [SerializeField] GameObject _markerPrefab;
    List<PointOfInterest> _pois;
    List<GameObject> _markerObjs;
    List<Material> _mats;

    void Start()
    {
        _pois = GlobalParameters.POIs.Values.ToList();
        _mats = new List<Material>();
        _markerObjs = new List<GameObject>();
        foreach (var poi in _pois)
        {
            var obj = Instantiate(_markerPrefab, _arrow.transform);
            obj.GetComponent<NewMarkerScript>()._POI = poi;
            obj.GetComponent<NewMarkerScript>().SetImage(GlobalParameters.CurrentTracker.Preview);

            var rend = obj.GetComponentInChildren<Renderer>();
            
            if (poi.name == GlobalParameters.CurrentTracker.Name)
            {
                rend.material.SetColor("_Color", Color.green);
            }
            else
            {
                rend.material.SetColor("_Color", Color.red);
            }
            
            _markerObjs.Add(obj);
        }
    }

    void Update()
    {
        foreach (var item in _markerObjs)
        {
            item.GetComponent<NewMarkerScript>().DrawMarker();
        }
    }
}
