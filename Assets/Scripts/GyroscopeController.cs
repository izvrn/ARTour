using System.Collections;
using UnityEngine;

public class GyroscopeController : MonoBehaviour
{
    [SerializeField] private Transform trackable;
    [SerializeField] private GameObject cameraPrefab;
    
    private Transform _wikitudeCameraTransform;
    private Transform _newCameraTransform;
    
    private Vector3 _initialPosition;
    private Quaternion _initialQuaternion;
    private Quaternion _gyroQuaternion;

    private bool _targetLost;

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
        _newCameraTransform.localRotation = GyroToUnity(_gyroQuaternion);
    }

    private void ResetOrientation()
    {
        _initialQuaternion = Input.gyro.attitude;
    }
    
    private Quaternion GyroToUnity(Quaternion q)
    {
        return new Quaternion(q.x, q.y, -q.z, -q.w);
    }

    public void OnTargetReco()
    {
        StartCoroutine(TargetReco());
    }

    private IEnumerator TargetReco()
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
    }
}
