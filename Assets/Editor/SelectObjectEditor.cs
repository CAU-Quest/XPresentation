using System.Collections;
using System.Collections.Generic;
using DimBoxes;
using UnityEditor;
using UnityEngine;



[CustomEditor(typeof(SelectObject)), CanEditMultipleObjects]
public class SelectObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SelectObject[] selectedObjects = new SelectObject[targets.Length];
        PresentationObject[] presentationObjects = new PresentationObject[targets.Length];
        
        for (int i = 0; i < targets.Length; i++)
        {
            selectedObjects[i] = (SelectObject)targets[i];
            presentationObjects[i] = selectedObjects[i].GetComponentInChildren<PresentationObject>(true);
        }
        
        EditorGUILayout.Separator();
        if (selectedObjects[0].BoundObjectType == BoundObjectType.ThreeDimension)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("TransformByVertexHandler");
            EditorGUILayout.ObjectField(selectedObjects[0].transformByVertexHandler, typeof(TransformByVertexHandler), true);
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("CenterPositionByVertex");
            EditorGUILayout.ObjectField(selectedObjects[0].centerPositionByVertex, typeof(CenterPositionByVertex), true);
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("BoundBox");
            EditorGUILayout.ObjectField(selectedObjects[0].boundBox, typeof(BoundBox), true);
            EditorGUILayout.EndHorizontal();
            
            EditorGUI.indentLevel--;
        }
        else
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("TransformByVertexHandler");
            EditorGUILayout.ObjectField(selectedObjects[0].transformByVertexHandler, typeof(TransformByVertexHandler), true);
            EditorGUILayout.EndHorizontal();
            
            EditorGUI.indentLevel--;
        }
        
        
        if (GUILayout.Button("Select"))
        {
            foreach (SelectObject selectObject in selectedObjects)
            {
                selectObject.Select();
            }
        }
        
        GUILayout.Space(10);
        EditorGUILayout.LabelField("Presentation Object 기능", EditorStyles.boldLabel);
        
        
        if (GUILayout.Button("현재 슬라이드 데이터 저장"))
        {
            foreach (PresentationObject presentationObject in presentationObjects)
            {
                presentationObject.UpdateCurrentObjectDataInSlide();
            }
        }
        if (GUILayout.Button("현재 슬라이드와 동일한 값으로 다음 슬라이드 초기화"))
        {
            foreach (PresentationObject presentationObject in presentationObjects)
            {
                presentationObject.SetNextSlideObjectDataSameAsCurrent();
            }
        }
        if (GUILayout.Button("현재 슬라이드와 동일한 값으로 이전 슬라이드 초기화"))
        {
            foreach (PresentationObject presentationObject in presentationObjects)
            {
                presentationObject.SetPreviousSlideObjectDataSameAsCurrent();
            }
        }
    }
}