using System.Collections;
using System.Collections.Generic;
using System.IO;
using Dummiesman;
using Oculus.Interaction;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public struct SaveObjectData
{
    public uint id;
    public DeployType deployType;
    public List<SlideObjectData> slideObjectDatas;
    public List<XRAnimation> animations;
    public string objectPath;
    public string imagePath;
    public string text;
}

public class SaveData : MonoBehaviour
{
    public static SaveData Instance = null;

    public List<SaveObjectData> objects = new List<SaveObjectData>();
    public List<VideoData> videoDatas = new List<VideoData>();

    public Transform parent;

    void Start()
    {
        if (null == Instance)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    
    public void SaveGameData()
    {
        videoDatas = new List<VideoData>();
        objects = new List<SaveObjectData>();
        MainSystem.Instance.NotifyObserverSaveData();
        ES3.Save("ObjectData", objects);
        ES3.Save("SlideCount", MainSystem.Instance.GetSlideCount());
        ES3.Save("VideoData", videoDatas);
    }

    public void LoadGameData()
    {
        videoDatas = ES3.Load<List<VideoData>>("VideoData");
        MainSystem.Instance.SetSlideCount(ES3.Load<int>("SlideCount"));
        VideoManager.Instance.videoDataList = videoDatas;
        objects = ES3.Load<List<SaveObjectData>>("ObjectData");
        for (int i = 0; i < objects.Count; i++)
        {
            SaveObjectData data = objects[i];
            GameObject go = PresentationObjectPool.Instance.Get((int)data.deployType - 1, Vector3.zero, parent);
            if (data.deployType == DeployType.ImportImage)
            {
                string imagePath = data.imagePath.Replace("#", "\\");
                go.GetComponent<SelectObject>().imagePath = imagePath;
                Texture2D texture = LoadTexture(imagePath);
                if(texture != null)
                    go.GetComponentInChildren<RawImage>().texture = texture;
            } else if (data.deployType == DeployType.ImportModel)
            {
                GameObject element = go.GetComponentInChildren<Grabbable>().gameObject;

                string objectPath = data.objectPath.Replace("#", "\\");
                GameObject model = new OBJLoader().Load(objectPath);
                model.transform.SetParent(element.transform);
	            model.transform.position = element.transform.position;
	            model.transform.rotation = element.transform.rotation;
                go.GetComponent<SelectObject>().objectPath = objectPath;
                
                string imagePath = data.imagePath.Replace("#", "\\");
                Texture2D texture = LoadTexture(imagePath);
                if(texture != null)
                    model.GetComponentInChildren<MeshRenderer>().material.mainTexture = texture;
                
                go.GetComponent<SelectObject>().imagePath = imagePath;
                element.AddComponent<PresentationObject>();
            } else if (data.deployType == DeployType.Text)
            {
                go.GetComponentInChildren<TMP_InputField>().text = data.text;
            }
            PresentationObject presentationObject = go.GetComponentInChildren<PresentationObject>();
            presentationObject.animationList = new List<XRAnimation>();

            presentationObject.slideData = new List<SlideObjectData>();
            for (int j = 0; j < data.slideObjectDatas.Count; j++)
            {
                presentationObject.slideData.Add(data.slideObjectDatas[j]);
            }
            for (int j = 0; j < data.animations.Count; j++)
            {
                data.animations[j].presentationObject = presentationObject;
                presentationObject.animationList.Add(data.animations[j]);
            }
        }
        MainSystem.Instance.NotifySlideChangeToObservers();
    }
    
    Texture2D LoadTexture(string path)
    {
        if (File.Exists(path))
        {
            byte[] fileData = System.IO.File.ReadAllBytes(path);

            Texture2D texture = new Texture2D(2, 2); // 텍스쳐의 가로 세로 크기를 지정합니다. 실제 크기에 맞게 수정하세요.
            bool success = texture.LoadImage(fileData); // 바이트 배열을 텍스쳐에 로드합니다.

            if (success)
            {
                // 텍스쳐 로드 성공 시 반환
                return texture;
            }
            else
            {
                // 텍스쳐 로드 실패 시 null 반환
                return null;
            }
        }
        else
        {
            return null;
        }
    }
}
