using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UtilEditor : MonoBehaviour
{
    
    
}


[CustomEditor(typeof(VideoManager))]
public class VideoManagerEditor : Editor
{
    public string videoPath;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        VideoManager videoManager = (VideoManager)target;
        
        GUILayout.Space(10);
        EditorGUILayout.LabelField("360 Video 추가하기", EditorStyles.boldLabel);
        
        videoPath = EditorGUILayout.TextField("영상 주소", videoPath);
        if (GUILayout.Button("다음 슬라이드에 해당 360 영상 넣기"))
        {
            videoManager.Add360Video(videoPath);
        }
    }
}


[CustomEditor(typeof(CenterPositionByVertex))]
public class CenterPositionByVertexEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        CenterPositionByVertex presentationObject = (CenterPositionByVertex)target;
        if (GUILayout.Button("Center Position"))
        {
            presentationObject.CenterPosition();
        }
    }
}


[CustomEditor(typeof(ObjectCreator))]
public class ObjectCreatorEditor : Editor
{
    private string _imagePath = "";
    private string objectPath = "";
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ObjectCreator CreateObject = (ObjectCreator)target;
        
        
        GUILayout.Space(10);
        EditorGUILayout.LabelField("Primitive 생성", EditorStyles.boldLabel);
        if (GUILayout.Button("Cube 생성"))
        {
            CreateObject.CreateObject(DeployType.Cube);
        }
        if (GUILayout.Button("Sphere 생성"))
        {
            CreateObject.CreateObject(DeployType.Sphere);
        }
        if (GUILayout.Button("Cylinder 생성"))
        {
            CreateObject.CreateObject(DeployType.Cylinder);
        }
        if (GUILayout.Button("Plane 생성"))
        {
            CreateObject.CreateObject(DeployType.Plane);
        }
        if (GUILayout.Button("Text 생성"))
        {
            CreateObject.CreateObject(DeployType.Text);
        }
        
        
        GUILayout.Space(10);
        EditorGUILayout.LabelField("파일 불러오기", EditorStyles.boldLabel);
        
        _imagePath = EditorGUILayout.TextField("이미지 주소", _imagePath);
       
        objectPath = EditorGUILayout.TextField("obj 파일 주소", objectPath);

        if (GUILayout.Button("이미지 불러오기"))
        {
            CreateObject.ImportImage(_imagePath);
        }
        if (GUILayout.Button("모델(obj 파일) 불러오기"))
        {
            CreateObject.ImportObject(objectPath, _imagePath);
        }
    }
}


[CustomEditor(typeof(SnapListController))]
public class SnapListControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SnapListController SelectObject = (SnapListController)target;
        
        
        if (GUILayout.Button("Set Number"))
        {
            SelectObject.SetInitialNumber();
        }
        if (GUILayout.Button("Render Camera"))
        {
            SelectObject.RenderAllTexture();
        }
        if (GUILayout.Button("Swipe To Left"))
        {
            SelectObject.SwipeToLeft();
        }
        if (GUILayout.Button("Swipe To Right"))
        {
            SelectObject.SwipeToRight();
        }
        if (GUILayout.Button("Go To slide 4"))
        {
            SelectObject.GoToSlideByIndex(4);
        }
    }
}


[CustomEditor(typeof(StageSetter))]
public class StageSetterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        StageSetter SelectObject = (StageSetter)target;
        
        
        if (GUILayout.Button("Set Stage"))
        {
            SelectObject.GoToStage();
        }
        if (GUILayout.Button("Set Audience"))
        {
            SelectObject.GoToAudience();
        }

    }
}



[CustomEditor(typeof(SaveData))]
public class SaveDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SaveData saveData = (SaveData)target;
        
        
        if (GUILayout.Button("Save Data"))
        {
            saveData.SaveGameData();
        }
        if (GUILayout.Button("Load Data"))
        {
            saveData.LoadGameData();
        }

    }
}


[CustomEditor(typeof(EdgeHandler))]
public class EdgeHandlerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EdgeHandler edgeHandler = (EdgeHandler)target;
        
        

    }
}

[CustomEditor(typeof(TransformByVertexHandler))]
public class TransformByVertexHandlerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();
        
        TransformByVertexHandler transformByVertexHandler = (TransformByVertexHandler)target;

        if (GUILayout.Button("Update Line"))
        {
            for (int i = 0; i < 12; i++)
            {
                transformByVertexHandler.lineList[i].UpdateLine();
            }
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(transformByVertexHandler);
        }
        
    }
}




[CustomEditor(typeof(PreviewRenderTexture))]
public class PreviewRenderTextureEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        PreviewRenderTexture renderTexture = (PreviewRenderTexture)target;
        
        
        if (GUILayout.Button("Render"))
        {
            renderTexture.RenderRequiredTexture();
        }

    }
}
