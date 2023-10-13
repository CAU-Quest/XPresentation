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
        if (GUILayout.Button("Edit Mode"))
        {
            mainSystem.ChangeMode(0);
        }
        if (GUILayout.Button("Deploy Mode"))
        {
            mainSystem.ChangeMode(1);
        }
        if (GUILayout.Button("Slide Mode"))
        {
            mainSystem.ChangeMode(2);
        }
        if (GUILayout.Button("Animation Mode"))
        {
            mainSystem.ChangeMode(3);
        }
        if (GUILayout.Button("Add Slide"))
        {
            mainSystem.AddSlide();
        }
        if (GUILayout.Button("Remove Slide"))
        {
            mainSystem.RemoveSlide();
        }
    }
}
