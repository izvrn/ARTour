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
