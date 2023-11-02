using UnityEngine;
using UnityEditor;
using System;

namespace DimBoxes
{
    [CustomEditor(typeof(BoundBox))]

    public class BoundBoxEditor : Editor
    {
        override public void OnInspectorGUI()
        {
            var BoundBox = target as BoundBox;
            base.DrawDefaultInspector();
            serializedObject.Update();

            if (GUI.changed)
            {
                serializedObject.ApplyModifiedProperties();
                BoundBox.OnValidate();
            }
            
            if (GUILayout.Button("Calculate Bounds"))
            {
                BoundBox.UpdateBounds();
            }
        }
    }
}
