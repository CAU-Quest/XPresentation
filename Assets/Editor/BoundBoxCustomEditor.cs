using UnityEngine;
using UnityEditor;
using System;

namespace DimBoxesCustom
{
    [CustomEditor(typeof(BoundBoxCustom))]

    public class BoundBoxEditor : Editor
    {
        override public void OnInspectorGUI()
        {
            var BoundBox = target as BoundBoxCustom;
            base.DrawDefaultInspector();
            serializedObject.Update();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Line_renderer");
            Undo.RecordObject(BoundBox, (BoundBox.isDrawingBoundBox? "Enabling" : "Disabling") + " line_renderer");
            BoundBox.isDrawingBoundBox = EditorGUILayout.Toggle(BoundBox.isDrawingBoundBox);
            EditorGUILayout.EndHorizontal();

            using (var lr_group = new EditorGUILayout.FadeGroupScope(Convert.ToSingle(BoundBox.isDrawingBoundBox)))
            {
                if (lr_group.visible == true)
                {
                    EditorGUI.indentLevel++;

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PrefixLabel("linePrefab");
                    BoundBox.linePrefab = EditorGUILayout.ObjectField(BoundBox.linePrefab, typeof(UnityEngine.Object), true);
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PrefixLabel("lineWidth");
                    BoundBox.lineWidth = EditorGUILayout.Slider(BoundBox.lineWidth,0.005f, 0.25f);
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PrefixLabel("lineColor");
                    BoundBox.lineColor = EditorGUILayout.ColorField(BoundBox.lineColor);
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PrefixLabel("numCapVertices");
                    BoundBox.numCapVertices = EditorGUILayout.IntField(BoundBox.numCapVertices);
                    EditorGUILayout.EndHorizontal();

                    EditorGUI.indentLevel--;
                }
            }
            if (GUI.changed)
            {
                serializedObject.ApplyModifiedProperties();
                BoundBox.OnValidate();
            }
        }
    }
}
