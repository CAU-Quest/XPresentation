using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PresentationObject))]
public class PresentationObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        PresentationObject presentationObject = (PresentationObject)target;
        
        
        GUILayout.Space(10);
        EditorGUILayout.LabelField("기능", EditorStyles.boldLabel);
        
        if (GUILayout.Button("현재 슬라이드 데이터 저장"))
        {
            presentationObject.UpdateCurrentObjectDataInSlide();
        }
        if (GUILayout.Button("현재 슬라이드와 동일한 값으로 다음 슬라이드 초기화"))
        {
            presentationObject.SetNextSlideObjectDataSameAsCurrent();
        }
        if (GUILayout.Button("현재 슬라이드와 동일한 값으로 이전 슬라이드 초기화"))
        {
            presentationObject.SetPreviousSlideObjectDataSameAsCurrent();
        }
    }
}

[CustomEditor(typeof(PresentationGhostObject))]
public class PresentationGhostObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        PresentationGhostObject presentationObject = (PresentationGhostObject)target;
        if (GUILayout.Button("Save Transform"))
        {
            presentationObject.UpdateCurrentObjectDataInSlide();
        }
    }
}

