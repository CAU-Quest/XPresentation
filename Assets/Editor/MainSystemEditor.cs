using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MainSystem))]
public class MainSystemEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MainSystem mainSystem = (MainSystem)target;
        
        if (GUILayout.Button("슬라이드 이동하기"))
        {
            mainSystem.MoveSlide();
        }
        
        GUILayout.Space(10); // 각 섹션 사이에 여백 추가
        EditorGUILayout.LabelField("기능", EditorStyles.boldLabel);
        
        if (GUILayout.Button("애니메이션 실행"))
        {
            mainSystem.AnimationToggle();
        }
        
        GUILayout.Space(10);
        EditorGUILayout.LabelField("Slide 관리", EditorStyles.boldLabel);

        if (GUILayout.Button("이전 슬라이드로"))
        {
            mainSystem.GoToPreviousSlide();
        }
        if (GUILayout.Button("다음 슬라이드로"))
        {
            mainSystem.GoToNextSlide();
        }
        if (GUILayout.Button("다음 슬라이드에 복제된 슬라이드 추가하기"))
        {
            mainSystem.AddSlideNextToCurrent();
        }
        if (GUILayout.Button("현재 슬라이드 삭제하기"))
        {
            mainSystem.RemoveSlide();
        }
        
        GUILayout.Space(10); // 각 섹션 사이에 여백 추가
        EditorGUILayout.LabelField("Mode 변경", EditorStyles.boldLabel);

        if (GUILayout.Button("메인 모드로 변경"))
        {
            mainSystem.ChangeMode(MainSystem.Mode.Edit);
        }
        if (GUILayout.Button("미리보기 모드로 변경"))
        {
            mainSystem.ChangeMode(MainSystem.Mode.Preview);
        }
        if (GUILayout.Button("애니메이션 모드로 변경"))
        {
            mainSystem.ChangeMode(MainSystem.Mode.Animation);
        }
        
    }
}
