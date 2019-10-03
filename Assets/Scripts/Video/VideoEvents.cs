using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class VideoEvents : MonoBehaviour
{
    private VideoPlayer _vPlayer;

    private void Awake()
    {
        _vPlayer = GetComponent<VideoPlayer>();
        StartCoroutine(ResetVideo());
    }

    private IEnumerator ResetVideo()
    {        
        yield return new WaitForSeconds(2f);
        _vPlayer.Pause();
        _vPlayer.time = 0;
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
