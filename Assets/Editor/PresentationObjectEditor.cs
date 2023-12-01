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
        if (GUILayout.Button("Save Transform"))
        {
            presentationObject.UpdateCurrentObjectDataInSlide();
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

