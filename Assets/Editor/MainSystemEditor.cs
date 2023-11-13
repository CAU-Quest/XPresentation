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
        if (GUILayout.Button("Previous Slide"))
        {
            mainSystem.GoToPreviousSlide();
        }
        if (GUILayout.Button("Next Slide"))
        {
            mainSystem.GoToNextSlide();
        }
        if (GUILayout.Button("Start Animation"))
        {
            mainSystem.AnimationToggle();
        }
        if (GUILayout.Button("Main Mode"))
        {
            mainSystem.ChangeMode(MainSystem.Mode.Edit);
        }
        if (GUILayout.Button("Preview Mode"))
        {
            mainSystem.ChangeMode(MainSystem.Mode.Preview);
        }
        if (GUILayout.Button("Animation Mode"))
        {
            mainSystem.ChangeMode(MainSystem.Mode.Animation);
        }
        if (GUILayout.Button("Add Slide"))
        {
            mainSystem.AddSlide();
        }
        if (GUILayout.Button("Remove Slide"))
        {
            mainSystem.RemoveSlide();
        }
        if (GUILayout.Button("Move Slide"))
        {
            mainSystem.MoveSlide();
        }
        
    }
}
