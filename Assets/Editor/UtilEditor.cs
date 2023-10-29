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
