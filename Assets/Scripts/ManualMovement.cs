using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class ManualMovement : MonoBehaviour
{
    [Header("UI SETTINGS")]
    [SerializeField] private Text positionText;
    [SerializeField] private Text rotationText;
    [SerializeField] private Text scaleText;

    [Header("CONTROLS SETTINGS")] 
    [SerializeField] private Transform objectTransform;
    [SerializeField] private Slider movementSlider;
    
    Vector3 _direction;
    float _scale;
    float _previousScale;
    float _previousAngle;
    
    bool _flag;

    void Start()
    {
        ResetMoving();
        movementSlider.value = .1f;
        
        Debug.LogError("This message will make the console appear in Development Builds");
    }
    
    void Update()
    {
        SetTextFields();
        objectTransform.localPosition += movementSlider.value * Time.deltaTime * _direction;
        
        if (Input.touchCount == 0)
        {
            ResetMoving();
            return;
        }
        
        if (!_flag)
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
                _direction = Vector3.forward;
                break;
            case "back":
                _direction = Vector3.back;
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

    public void CaptureScreenshot()
    {
        StartCoroutine(CaptureScreenshotCoroutine(Screen.width, Screen.height));
    }

    private IEnumerator CaptureScreenshotCoroutine(int width, int height)
    {
        yield return new WaitForEndOfFrame();
        Texture2D tex = new Texture2D(width, height);
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();

        yield return tex;
        string path = SaveImageToGallery(tex, "Name", "Description");
        Debug.Log("Picture has been saved at:\n" + path);
    }
    
    private const string MEDIA_STORE_IMAGE_MEDIA = "android.provider.MediaStore$Images$Media";
    private static AndroidJavaObject m_Activity;

    private static string SaveImageToGallery(Texture2D aTexture, string aTitle, string aDescription)
    {
        using (AndroidJavaClass mediaClass = new AndroidJavaClass(MEDIA_STORE_IMAGE_MEDIA))
        {
            using (AndroidJavaObject contentResolver = Activity.Call<AndroidJavaObject>("getContentResolver"))
            {
                AndroidJavaObject image = Texture2DToAndroidBitmap(aTexture);
                return mediaClass.CallStatic<string>("insertImage", contentResolver, image, aTitle, aDescription);
            }
        }
    }

    private static AndroidJavaObject Texture2DToAndroidBitmap(Texture2D aTexture)
    {
        byte[] encodedTexture = aTexture.EncodeToPNG();
        using (AndroidJavaClass bitmapFactory = new AndroidJavaClass("android.graphics.BitmapFactory"))
        {
            return bitmapFactory.CallStatic<AndroidJavaObject>("decodeByteArray", encodedTexture, 0, encodedTexture.Length);
        }
    }

    private static AndroidJavaObject Activity
    {
        get
        {
            if (m_Activity == null)
            {
                AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                m_Activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            }
            return m_Activity;
        }
    }

    private void SendObjectDataToServer()
    {
        
    }
}
