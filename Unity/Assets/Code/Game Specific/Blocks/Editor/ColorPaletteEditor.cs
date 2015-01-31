using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(ColorPalette))]

public class ColorPaletteEditor : EditorPlus
{
    public override void OnInspectorGUI()
    {
        ColorPalette pal = (ColorPalette)target;

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add"))
            pal.AddTeamPalette(Color.yellow);

        if (GUILayout.Button("Resize Tones"))
            pal.ResizeMaxTones();

        EditorGUILayout.EndHorizontal();

        base.OnInspectorGUI();
    }
}
