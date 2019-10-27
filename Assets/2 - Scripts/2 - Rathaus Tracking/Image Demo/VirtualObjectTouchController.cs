using UnityEngine;
using UnityEngine.Events;

public class VirtualObjectTouchController : MonoBehaviour
{
    public UnityEvent objectTouched;
    private Camera _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        for (int i = 0; i < Input.touchCount; ++i) 
        {
            if (Input.GetTouch(i).phase.Equals(TouchPhase.Began)) 
            {
                Ray ray = _mainCamera.ScreenPointToRay(Input.GetTouch(i).position);
                
                if (Physics.Raycast(ray, out _)) 
                {
                    objectTouched.Invoke();
                }
            }
        }
    }
}
