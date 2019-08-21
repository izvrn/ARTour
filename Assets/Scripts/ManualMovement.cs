using UnityEngine;
using UnityEngine.UI;
using System;

public class ManualMovement : MonoBehaviour
{
    [Header("UI SETTINGS")]
    [SerializeField] private Text positionText;
    [SerializeField] private Text rotationText;
    [SerializeField] private Text scaleText;

    [Header("CONTROLS SETTINGS")] 
    [SerializeField] private Transform objectTransform;
    [SerializeField] private float movementSpeed;
    
    Vector3 _direction;
    float _scale;
    float _previousScale;
    float _previousAngle;
    
    bool _flag;

    void Start()
    {
        ResetMoving();
        movementSpeed = .1f;
    }
    
    void Update()
    {
        SetTextFields();
        if (Input.touchCount == 0) ResetMoving();
        objectTransform.localPosition += movementSpeed * Time.deltaTime * _direction;

        if ( !_flag)
            WriteLastTouchData(Input.touches);
        
        if (Input.touchCount == 2)
        {
            _flag = true;
            ChangeScale(Input.touches);
            return;
        }
        _flag = false;       
    }
    
    public void MoveObject(string path)
    {
        switch (path)
        {
            case "forward" :
                _direction = -Vector3.forward;
                break;
            case "back":
                _direction = -Vector3.back;
                break;
            case "left":
                _direction = Vector3.left;
                break;
            case "right":
                _direction = Vector3.right;
                break;
            case "up":
                _direction = Vector3.up;
                break;
            case "down":
                _direction = Vector3.down;
                break;
        }
    }
    
    public void RotateObject(Slider s)
    {
        objectTransform.localRotation = Quaternion.Euler(new Vector3(0, s.value, 0));
    }

    private void SetTextFields()
    {
        positionText.text = "Position : (" + Math.Round(objectTransform.localPosition.x, 2)+ " , " + Math.Round(objectTransform.localPosition.y, 2) + " , " + Math.Round(objectTransform.localPosition.z, 2) + ")";
        scaleText.text = "Scale : (" + Math.Round(objectTransform.localScale.x, 2) + ")";
        rotationText.text = "Rotation : (" + (int)objectTransform.localRotation.eulerAngles.y + ")";
    }

    private void ChangeScale (Touch[] touches)
    {
        if (touches.Length != 2) return;

        objectTransform.localScale *= Mathf.Sqrt((touches[0].position.x - touches[1].position.x) * (touches[0].position.x - touches[1].position.x) +
            (touches[0].position.y - touches[1].position.y) * (touches[0].position.y - touches[1].position.y)) / _previousScale;
        WriteLastTouchData(Input.touches);
    }

    private void WriteLastTouchData(Touch[] touches)
    {
        _previousScale = Mathf.Sqrt((touches[0].position.x - touches[1].position.x) * (touches[0].position.x - touches[1].position.x) +
                                    (touches[0].position.y - touches[1].position.y) * (touches[0].position.y - touches[1].position.y));
    }

    public void ResetMoving()
    {
        _direction = Vector3.zero;
    }

    private static AndroidJavaObject _activity; 
    
    public static AndroidJavaObject Activity
    {
        get
        {
            if (_activity == null)
            {
                var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                _activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            }
            return _activity;
        }
    }

    private const string MediaStoreImagesMediaClass = "android.provider.MediaStore$Images$Media";
    public static string SaveImageToGallery(Texture2D texture2D, string title, string description)
    {
        using (var mediaClass = new AndroidJavaClass(MediaStoreImagesMediaClass))
        {
            using (var cr = Activity.Call<AndroidJavaObject>("getContentResolver"))
            {
                var image = Texture2DToAndroidBitmap(texture2D);
                var imageUrl = mediaClass.CallStatic<string>("insertImage", cr, image, title, description);
                return imageUrl;
            }
        }
    }
  
    public static AndroidJavaObject Texture2DToAndroidBitmap(Texture2D texture2D)
    {
        byte[] encoded = texture2D.EncodeToPNG();
        using (var bf = new AndroidJavaClass("android.graphics.BitmapFactory"))
        {
            return bf.CallStatic<AndroidJavaObject>("decodeByteArray", encoded, 0, encoded.Length);
        }
    }
}
