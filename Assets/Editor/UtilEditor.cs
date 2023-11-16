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


[CustomEditor(typeof(SelectObject))]
public class SelectObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SelectObject SelectObject = (SelectObject)target;
        
        
        if (GUILayout.Button("Select"))
        {
            SelectObject.Select();
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
