using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerManager : MonoBehaviour
{
    [SerializeField] GameObject _arrow;
    [SerializeField] GameObject _markerPrefab;
    [SerializeField] public List<PointOfInterest> _pois;
    List<GameObject> _markerObjs;
    List<Material> _mats;

    void Awake()
    {
        _mats = new List<Material>();
        _markerObjs = new List<GameObject>();
        foreach (var poi in _pois)
        {
            var obj = Instantiate(_markerPrefab, _arrow.transform);
            obj.GetComponent<NewMarkerScript>()._POI = poi;
            obj.GetComponent<NewMarkerScript>().SetImage(GlobalParameters.CurrentTracker.Preview);
            var mat = new Material(Shader.Find("Standard"));
            mat.color = new Color(Random.Range(0, 255), Random.Range(0, 255), Random.Range(0, 255));
            obj.GetComponent<NewMarkerScript>().SetMaterial(mat);
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
