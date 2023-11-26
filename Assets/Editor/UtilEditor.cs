using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UtilEditor : MonoBehaviour
{
    
    
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

[CustomEditor(typeof(ChangeColorWithColorPicker))]
public class ChangeColorWithColorPickerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ChangeColorWithColorPicker SelectObject = (ChangeColorWithColorPicker)target;
        
        
        if (GUILayout.Button("Select"))
        {
            SelectObject.OpenColorPicker();
        }
    }
}

[CustomEditor(typeof(ObjectCreator))]
public class ObjectCreatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ObjectCreator CreateObject = (ObjectCreator)target;
        
        
        if (GUILayout.Button("Create Cube"))
        {
            CreateObject.CreateObject(DeployType.Cube);
        }
        if (GUILayout.Button("Create Sphere"))
        {
            CreateObject.CreateObject(DeployType.Sphere);
        }
        if (GUILayout.Button("Create Cylinder"))
        {
            CreateObject.CreateObject(DeployType.Cylinder);
        }
        if (GUILayout.Button("Create Plane"))
        {
            CreateObject.CreateObject(DeployType.Plane);
        }
        if (GUILayout.Button("Create Text"))
        {
            CreateObject.CreateObject(DeployType.Text);
        }
        if (GUILayout.Button("Import Image Plane"))
        {
            CreateObject.CreateObject(DeployType.ImportImage);
        }
        if (GUILayout.Button("Import Model"))
        {
            CreateObject.CreateObject(DeployType.ImportModel);
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
