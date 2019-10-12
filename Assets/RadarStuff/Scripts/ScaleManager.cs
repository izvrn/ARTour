using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScaleManager : MonoBehaviour
{
    [SerializeField] private GameObject _gear;

    public float radarDistance
    {
        get;
        private set;
    }
    [SerializeField] private Text _distanceText;
    
    private float _scale = 1;
    private Touch _startTouch;

    [SerializeField] private GameObject _info;


    IEnumerator InfoGlance()
    {
        _info.SetActive(false);

        yield return new WaitForSeconds(3f);
        while (!_wasFirstTouch)
        {
            _info.SetActive(!_info.active);
            yield return new WaitForSeconds(.5f);
        }
        _info.SetActive(false);
    }

    private void SetDistanceText()
    {
        _distanceText.text = "RADIUS : " + Math.Round((double)radarDistance, 1) + " KM";
    }

    // Start is called before the first frame update
    void Awake()
    {
        StartCoroutine(SetDistance());
        StartCoroutine(InfoGlance());
    }

    bool _collides;

    bool _isTouched;

    bool _wasFirstTouch;


    bool ColliderCheck(Touch t)
    {
            // The ray to the touched object in the world
            var ray = Camera.main.ScreenPointToRay(t.position);

            // Your raycast handling
            if (Physics.Raycast(ray.origin, ray.direction, out var hit))
            {
                if (hit.transform.tag == "Gear")
                {
                    return true;
                }
            }
            return false;        
    }

    private float _permanentScaleCoef;

    private float _lastDistance;

    public void SetDistance(float distance)
    {
        radarDistance = distance;
    }

    private IEnumerator SetDistance()
    {
        while (Input.location.status != LocationServiceStatus.Running)
        {
            yield return null;
        }

        radarDistance = MathHelper.DistanceTo(new Location(Input.location.lastData), Scenes.CurrentTracker.Location) / 1000f * 1.2f;
        _lastDistance = radarDistance;
        SetDistanceText();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.touchCount == 0)
        {
            if (_isTouched)
            {
                _scale += _permanentScaleCoef;
                _lastDistance = radarDistance;
            }
            _isTouched = false;
            return;
        }

        if (Input.touchCount == 1)
        {
            _wasFirstTouch = true;
            if (!_isTouched)
            {
                _startTouch = Input.touches[0];
                if (ColliderCheck(_startTouch))
                    _isTouched = true;
                return;
            }
            
          
            _permanentScaleCoef = _scale = (Input.touches[0].position.y - _startTouch.position.y) / 2000f;
            radarDistance = Mathf.Clamp(_lastDistance + _scale * 10f, 0, Mathf.Infinity);
            SetDistanceText();
            _gear.transform.rotation = Quaternion.Euler(new Vector3(_gear.transform.rotation.x +(Input.touches[0].position.y - _startTouch.position.y) / 5, 0, 0) * -1);
        }        
    }
}
