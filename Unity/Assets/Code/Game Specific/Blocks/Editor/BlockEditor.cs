using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Block))]
public class BlockEditor : EditorPlus
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Block block = target as Block;
        if (GUILayout.Button("Rebuild Face ID's"))
            block.Create();
    }
}
