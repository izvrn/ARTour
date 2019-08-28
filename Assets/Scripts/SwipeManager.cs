using UnityEngine;
using UnityEngine.Events;

public class SwipeManager : MonoBehaviour
{
    [SerializeField] private bool debugWithArrowKeys = true;
    
    public static SwipeManager Instance { get; set; }

    public UnityEvent swipeUp;
    public UnityEvent swipeDown;
    public UnityEvent swipeLeft;
    public UnityEvent swipeRight;
    
    // If the touch is longer than MAX_SWIPE_TIME, we dont consider it a swipe
    private const float MAX_SWIPE_TIME = 0.5f; 
	
    // Factor of the screen width that we consider a swipe
    // 0.17 works well for portrait mode 16:9 phone
    private const float MIN_SWIPE_DISTANCE = 0.17f;
  
    private Vector2 _startPos;
    private float _startTime;

    private void Awake()
    {
        if (!Instance)
            Instance = this;
        
        swipeUp = new UnityEvent();
        swipeDown = new UnityEvent();
        swipeLeft = new UnityEvent();
        swipeRight = new UnityEvent();
    }

    private void Update()
    {
        if (Input.touches.Length > 0)
        {
            var t = Input.GetTouch(0);
            switch (t.phase)
            {
                case TouchPhase.Began:
                    _startPos = new Vector2(t.position.x/Screen.width, t.position.y/Screen.width);
                    _startTime = Time.time;
                    break;
                // press too long
                case TouchPhase.Ended when Time.time - _startTime > MAX_SWIPE_TIME:
                    return;
                case TouchPhase.Ended:
                {
                    Vector2 endPos = new Vector2(t.position.x/Screen.width, t.position.y/Screen.width);

                    Vector2 swipe = new Vector2(endPos.x - _startPos.x, endPos.y - _startPos.y);

                    if (swipe.magnitude < MIN_SWIPE_DISTANCE) // Too short swipe
                        return;

                    if (Mathf.Abs (swipe.x) > Mathf.Abs (swipe.y)) { // Horizontal swipe
                        if (swipe.x > 0) {
                            swipeRight.Invoke();
                        }
                        else {
                            swipeLeft.Invoke();
                        }
                    }
                    else { // Vertical swipe
                        if (swipe.y > 0) {
                            swipeUp.Invoke();
                        }
                        else {
                            swipeDown.Invoke();
                        }
                    }

                    break;
                }
            }
        }

        if (debugWithArrowKeys) {
            if (Input.GetKeyDown (KeyCode.DownArrow)) swipeDown.Invoke();
            if (Input.GetKeyDown (KeyCode.UpArrow)) swipeUp.Invoke();
            if (Input.GetKeyDown (KeyCode.RightArrow)) swipeRight.Invoke();
            if (Input.GetKeyDown (KeyCode.LeftArrow)) swipeLeft.Invoke();
        }
    }
    
    
}

public class SwipeEvent : UnityEvent
{
    
}