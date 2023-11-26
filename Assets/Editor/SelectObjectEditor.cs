using System.Collections;
using System.Collections.Generic;
using DimBoxes;
using UnityEditor;
using UnityEngine;



[CustomEditor(typeof(SelectObject))]
public class SelectObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SelectObject selectObject = (SelectObject)target;
        
        EditorGUILayout.Separator();
        if (selectObject.BoundObjectType == BoundObjectType.ThreeDimension)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("TransformByVertexHandler");
            EditorGUILayout.ObjectField(selectObject.transformByVertexHandler, typeof(TransformByVertexHandler), true);
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("CenterPositionByVertex");
            EditorGUILayout.ObjectField(selectObject.centerPositionByVertex, typeof(CenterPositionByVertex), true);
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("BoundBox");
            EditorGUILayout.ObjectField(selectObject.boundBox, typeof(BoundBox), true);
            EditorGUILayout.EndHorizontal();
            
            EditorGUI.indentLevel--;
        }
        else
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("TransformByVertexHandler");
            EditorGUILayout.ObjectField(selectObject.transformByVertexHandler, typeof(TransformByVertexHandler), true);
            EditorGUILayout.EndHorizontal();
            
            EditorGUI.indentLevel--;
        }
        
        if (GUILayout.Button("Select"))
        {
            selectObject.Select();
        }

    }
}