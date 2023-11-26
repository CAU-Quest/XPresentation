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
