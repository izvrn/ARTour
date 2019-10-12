using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticArrowRotation : MonoBehaviour
{

    private bool _isInitialized = false;

    public void SwitchInit()
    {
        _isInitialized = true;
        Input.location.Start();
        Input.compass.enabled = true;
        StartCoroutine(GetHeading());
    }

    IEnumerator GetHeading()
    {
        _currentHeading = Input.compass.trueHeading;
        yield return new WaitForSeconds(.5f);
        StartCoroutine(GetHeading());    
    }

    // Start is called before the first frame update
    void Awake()
    {
        InitializingScript.OnInitializationSuccess += SwitchInit;
        SwitchInit();

    }

    private float _currentHeading;

    // Update is called once per frame
    void Update()
    {
        //if (!_isInitialized) return;

        //transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, -_currentHeading, transform.rotation.z));

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler
            (new Vector3(transform.rotation.x, -_currentHeading, transform.rotation.z)),
            2f * Time.deltaTime);
    }
}
