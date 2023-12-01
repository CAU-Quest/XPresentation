using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour, ISystemObserver
{
    public VideoPlayer videoPlayer;

    public GameObject sphere;

    public GameObject stage;

    public String url;

    #region Observer Handling

    public void ObserverUpdateMode(MainSystem.Mode mode)
    {
    }

    public void ObserverUpdateSlide(int slide)
    {
    }

    public void ObserverRemoveSlide(int index){}
    public void ObserverAddSlide(){}
    public void ObserverAddSlideNextTo(int index){}
    public void ObserverMoveSlides(int moved, int count, int into){}
    public void ObserverUpdateSave(){}

    public void ObserverCreateVideo(int index)
    {
        
    }
    

    #endregion
    private void Start()
    {
        if (videoPlayer == null) videoPlayer = GetComponent<VideoPlayer>();
    }

    public void StartVideo(string url)
    {
        stage.SetActive(false);
        sphere.SetActive(true);
        videoPlayer.url = url;
        videoPlayer.Play();
    }

    public void StopVideo()
    {
        videoPlayer.Stop();
    }

    public void FinishVideo()
    {
        videoPlayer.Stop();
        sphere.SetActive(false);
        stage.SetActive(true);
    }
}
