using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SimpleFileBrowser;
using UnityEngine;
using UnityEngine.Video;

[Serializable]
public struct VideoData
{
    public int slideNumber;
    public String url;
}

public class VideoManager : MonoBehaviour, ISystemObserver
{
    public static VideoManager Instance = null;
    
    public VideoPlayer videoPlayer;

    public GameObject sphere;

    public GameObject stage;
    public GameObject dome;
    
    public GameObject objects;

    public List<VideoData> videoDataList = new List<VideoData>();

    #region Observer Handling

    public void ObserverUpdateMode(MainSystem.Mode mode)
    {
    }

    public void ObserverUpdateSlide(int slide)
    {
        Debug.Log("Update Slide 시작");
        for (int i = 0; i < videoDataList.Count; i++)
        {
            var videoData = videoDataList[i];
            if (slide == videoData.slideNumber)
            {
                Debug.Log("일치하는 slide 존재함");
                stage.SetActive(false);
                sphere.SetActive(true);
                
                StartVideo(videoData.url);
                return;
            }
        }
        
        stage.SetActive(true);
        sphere.SetActive(false);
        StopVideo();
    }

    public void ObserverRemoveSlide(int index)
    {
        for (int i = 0; i < videoDataList.Count; i++)
        {
            var videoData = videoDataList[i];
            if (videoData.slideNumber > index)
            {
                videoData.slideNumber -= 1;
            } else if (videoData.slideNumber == index)
            {
                videoDataList.RemoveAt(i);
            }
        }
    }

    public void ObserverAddSlide()
    {
        for (int i = 0; i < videoDataList.Count; i++)
        {
            var videoData = videoDataList[i];
            if (videoData.slideNumber > MainSystem.Instance.currentSlideNum)
            {
                videoData.slideNumber += 1;
            }
        }
    }

    public void ObserverAddSlideNextTo(int index)
    {
        for (int i = 0; i < videoDataList.Count; i++)
        {
            var videoData = videoDataList[i];
            if (videoData.slideNumber > index)
            {
                videoData.slideNumber += 1;
            }
        }
    }

    public void ObserverMoveSlides(int moved, int count, int into)
    {
        for (int i = 0; i < videoDataList.Count; i++)
        {
            var videoData = videoDataList[i];
            int index = videoData.slideNumber;
            if (moved <= index && index < moved + count) // video가 이동시키는 slide에 포함되어 있을 경우
            {
                videoData.slideNumber = into + (index - moved);
            } else if (moved < index && moved + count < index)
            {
                if (into < index)
                {
                    // Do nothing
                }
                else
                {
                    videoData.slideNumber -= count;
                }
            } else if (moved > index)
            {
                if (into < index)
                {
                    videoData.slideNumber += count;
                }
                else
                {
                    //Do nothing
                }
            }
        }
    }

    public void ObserverUpdateSave()
    {
        for (int i = 0; i < videoDataList.Count; i++)
        {
            SaveData.Instance.videoDatas.Add(videoDataList[i]);
        }
    }

    public void ObserverDuplicateSlideNextTo(int index)
    {
        for (int i = 0; i < videoDataList.Count; i++)
        {
            var videoData = videoDataList[i];
            if (videoData.slideNumber > MainSystem.Instance.currentSlideNum)
            {
                videoData.slideNumber += 1;
            }
        }
    }

    public void ObserverCreateVideo(int index)
    {
        
    }
    

    #endregion

    #region Test Codes

    public void Add360Video(string path)
    {
        VideoData videoData = new VideoData();
        videoData.url = path;
        videoData.slideNumber = MainSystem.Instance.currentSlideNum + 1;
            
        videoDataList.Add(videoData);
        MainSystem.Instance.AddVideoAsSlideByIndex(MainSystem.Instance.currentSlideNum + 1);
        MainSystem.Instance.GoToNextSlide();
    }

    #endregion
    
    #region Video URL Handler

    public void GetURL(int slideNumber)
    {
        Pose spawnPose = XRUIManager.Instance.GetPlayerSightPose();
        Vector3 position = spawnPose.position;
        Quaternion rotation = spawnPose.rotation;
        
        XRUIManager.Instance.fileBrowser.SetActive(true);
        XRUIManager.Instance.fileBrowser.transform.position = position;
        XRUIManager.Instance.fileBrowser.transform.rotation = rotation;
        LoadVideoFile(slideNumber);
    }
    
    
    public void LoadVideoFile(int slideNumber)
    {
        FileBrowser.SetFilters( true, new FileBrowser.Filter( "Images", ".jpg", ".png" ), new FileBrowser.Filter( "Text Files", ".txt", ".pdf" ) );
        FileBrowser.SetDefaultFilter( ".obj" );
        FileBrowser.SetExcludedExtensions( ".lnk", ".tmp", ".zip", ".rar", ".exe" );
        FileBrowser.AddQuickLink( "Users", "C:\\Users", null );
        StartCoroutine( ShowLoadVideoDialogCoroutine(slideNumber) );
    }
    
    
    IEnumerator ShowLoadVideoDialogCoroutine(int slideNumber)
    {
        yield return FileBrowser.WaitForLoadDialog( FileBrowser.PickMode.FilesAndFolders, true, null, null, "Load Files and Folders", "Load" );
		
        Debug.Log( FileBrowser.Success );

        if( FileBrowser.Success )
        {
            VideoData videoData = new VideoData();
            videoData.url = FileBrowser.Result[0];
            videoData.slideNumber = slideNumber;
            
            videoDataList.Add(videoData);
            MainSystem.Instance.AddVideoAsSlideByIndex(slideNumber);
        }
    }

    #endregion
    
    
    private void Start()
    {
        if (null == Instance)
        {
            Instance = this;
            if (videoPlayer == null) videoPlayer = GetComponent<VideoPlayer>();
            MainSystem.Instance.RegisterObserver(this);
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void StartVideo(string url)
    {
        stage.SetActive(false);
        dome.SetActive(false);
        objects.SetActive(false);
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
        if(MainSystem.Instance.mode == MainSystem.Mode.Preview)
            dome.SetActive(true);
        else
            stage.SetActive(true);
        objects.SetActive(true);
        
    }
}
