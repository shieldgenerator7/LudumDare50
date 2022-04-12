using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AIGridManager))]
public class AIGridManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUI.enabled = !EditorApplication.isPlaying;
        if (GUILayout.Button("Generate Grid (Editor Only"))
        {
            (target as AIGridManager).GenerateGridObjects();
            EditorUtility.SetDirty(target);
        }
    }
}
