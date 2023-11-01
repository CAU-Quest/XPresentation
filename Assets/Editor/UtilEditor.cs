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

[CustomEditor(typeof(VertexHandler))]
public class VertexHandlerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        
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
        
        EditorGUILayout.LabelField("Vector3 Array:");


        if (transformByVertexHandler != null & transformByVertexHandler.corners != null)
        {
            for (int x = 0; x < transformByVertexHandler.corners.GetLength(0); x++)
            {
                for (int y = 0; y < transformByVertexHandler.corners.GetLength(1); y++)
                {
                    for (int z = 0; z < transformByVertexHandler.corners.GetLength(2); z++)
                    {
                        transformByVertexHandler.corners[x, y, z] =
                            EditorGUILayout.Vector3Field($"Element ({x}, {y}, {z})",
                                transformByVertexHandler.corners[x, y, z]);
                    }
                }
            }
        }
        else
        {
            transformByVertexHandler = (TransformByVertexHandler)target;
        }
        
        
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
