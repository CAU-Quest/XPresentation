using System.Collections;
using System.Collections.Generic;
using Dummiesman;
using Oculus.Interaction;
using UnityEngine;
using UnityEngine.UI;

public struct SaveObjectData
{
    public uint id;
    public DeployType deployType;
    public List<SlideObjectData> slideObjectDatas;
    public List<XRAnimation> animations;
    public string objectPath;
    public string imagePath;
}

public class SaveData : MonoBehaviour
{
    public static SaveData Instance = null;

    public List<SaveObjectData> objects = new List<SaveObjectData>();

    void Awake()
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
        MainSystem.Instance.NotifyObserverSaveData();
        ES3.Save("ObjectData", objects);
    }

    public void LoadGameData()
    {
        objects = ES3.Load<List<SaveObjectData>>("ObjectData");
        for (int i = 0; i < objects.Count; i++)
        {
            SaveObjectData data = objects[i];
            GameObject go = PresentationObjectPool.Instance.Get((int)data.deployType - 1, Vector3.zero);
            if (data.deployType == DeployType.ImportImage)
            {
                go.GetComponentInChildren<RawImage>().texture = Resources.Load<Texture>(data.imagePath);
            } else if (data.deployType == DeployType.ImportModel)
            {
                GameObject element = go.GetComponentInChildren<Grabbable>().gameObject;

                new OBJLoader().Load(data.objectPath);
                element.AddComponent<PresentationObject>();
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
                presentationObject.animationList.Add(data.animations[j]);
            }

        }
    }
}
