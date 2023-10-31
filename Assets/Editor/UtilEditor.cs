using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UtilEditor : MonoBehaviour
{
    
    
}


[CustomEditor(typeof(CenterChildPosition))]
public class CenterChildPositionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        CenterChildPosition presentationObject = (CenterChildPosition)target;
        if (GUILayout.Button("Center Child Position"))
        {
            presentationObject.EditorCenterChildPositions();
        }
    }
}

[CustomEditor(typeof(VertexHandler))]
public class VertexHandlerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        VertexHandler presentationObject = (VertexHandler)target;
        if (GUILayout.Button("Update Vertex"))
        {
            presentationObject.UpdateVertex();
        }
        
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
        /*
        if (GUILayout.Button("Update Vertex"))
        {
            transformByVertexHandler.UpdateVertex();
        }
        */
        
        EditorGUILayout.LabelField("Vector3 Array:");


        if (transformByVertexHandler != null)
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
        
        

        if (GUI.changed)
        {
            EditorUtility.SetDirty(transformByVertexHandler);
        }
        
    }
}

