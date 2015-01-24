using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(BlockManager))]
public class BlockManagerEditor : EditorPlus
{
    
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        BlockManager mgr = target as BlockManager;
        if (mgr.LevelPaths == null)
            mgr.LevelPaths = mgr.FindLevels();

        EditorGUILayout.BeginHorizontal();
        int index = EditorPrefs.GetInt("Level",0);
        index = EditorGUILayout.Popup(index, mgr.LevelPaths.ToArray());
        EditorPrefs.SetInt("Level", index);

        if (GUILayout.Button("Load Level"))
        {
            BlockManager.LoadLevel("Assets/Levels/" + mgr.LevelPaths[index]);
            Debug.Log("Assets/Levels" + mgr.LevelPaths[index]);
        }
            

        EditorGUILayout.EndHorizontal();

    }   
}
