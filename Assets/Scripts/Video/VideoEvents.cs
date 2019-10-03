using UnityEngine;
using UnityEngine.Video;

public class VideoEvents : MonoBehaviour
{
    private VideoPlayer _vPlayer;

    void Start()
    {        
        _vPlayer = GetComponent<VideoPlayer>();
        _vPlayer.waitForFirstFrame = true;
    }
   
    public void OnTargetRecognized()
    {
        _vPlayer.Play();
    }

    public void OnTargetLost()
    {
        _vPlayer.Pause();
    }
}
