using UnityEngine;

public class GyroscopeController : MonoBehaviour
{
    [SerializeField] private Transform trackable;
    
    private Transform _wikitudeCameraTransform;
    // private Transform _newCameraTransform;
    
    private Vector3 _initialPosition;
    private Quaternion _initialQuaternion;
    private Quaternion _gyroQuaternion;

    private bool _targetLost;

    private float _currentAccelerationMagnitude, _lastAccelerationMagnitude, _speed = 4;

    private void Start()
    {
        _wikitudeCameraTransform = Camera.main.transform;
        Input.gyro.enabled = true;
    }

    private void Update()
    {
        if (!_targetLost)
            return;
        
        if (_initialPosition == _wikitudeCameraTransform.localRotation.eulerAngles)
            ResetOrientation();
        
        _gyroQuaternion = Quaternion.Inverse(_initialQuaternion) * Input.gyro.attitude;
        _wikitudeCameraTransform.localRotation = GyroToUnity(_gyroQuaternion);
    }

    private void FixedUpdate()
    {
        var movement = Vector3.zero;
        
        _currentAccelerationMagnitude = Input.acceleration.magnitude;
        var accelerationDifference = Mathf.Abs(_currentAccelerationMagnitude - _lastAccelerationMagnitude);

        if (accelerationDifference > .0015 && .004 > accelerationDifference)
            movement = _wikitudeCameraTransform.forward;

        _lastAccelerationMagnitude = Mathf.Abs(_currentAccelerationMagnitude);
    
        _wikitudeCameraTransform.Translate(movement * _speed);
    }

    private void ResetOrientation()
    {
        _initialQuaternion = Input.gyro.attitude;
    }
    
    private Quaternion GyroToUnity(Quaternion q)
    {
        return new Quaternion(q.x, q.y, -q.z, -q.w);
    }
    
    private Vector3 GyroToUnity(Vector3 v)
    {
        return new Vector3(v.x, v.y, -v.z);
    }

    /* public void OnTargetReco()
    {
        StartCoroutine(TargetReco());
    } */

    /* private IEnumerator TargetReco()
    {
        yield return new WaitForSeconds(1);
        
        _wikitudeCameraTransform.gameObject.SetActive(false);
        
        _initialPosition = _wikitudeCameraTransform.localPosition;
        _initialQuaternion = _wikitudeCameraTransform.localRotation;

        trackable.gameObject.SetActive(true);
        
        Debug.Log(_initialQuaternion.eulerAngles);

        _newCameraTransform = Instantiate(cameraPrefab, _initialPosition, _initialQuaternion).transform;
        Debug.Log(_newCameraTransform.eulerAngles);
        _targetLost = true;
    } */
}
