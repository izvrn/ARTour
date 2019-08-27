using UnityEngine;
using Wikitude;

public class CameraController : MonoBehaviour
{
     private WikitudeCamera _camera;

     private void Awake()
     {
          _camera = FindObjectOfType<WikitudeCamera>();
     }

     private void Start()
     {
          _camera.Camera2SupportLevel = Camera2SupportLevel.Legacy;
     }
}
