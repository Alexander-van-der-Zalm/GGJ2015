using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(BlockManager))]
public class BlockManagerEditor : EditorPlus
{
    
    
    public override void OnInspectorGUI()
    {
        BlockManager mgr = target as BlockManager;
        if (mgr.LevelNames == null)
            mgr.LevelNames = mgr.FindLevelNames();

        EditorGUILayout.BeginHorizontal();
        int index = EditorPrefs.GetInt("Level", 0);
        index = EditorGUILayout.Popup(index, mgr.LevelNames.ToArray());
        EditorPrefs.SetInt("Level", index);

        if (GUILayout.Button("Load Level"))
        {
            BlockManager.LoadLevel(mgr.LevelNames[index]);
            Debug.Log("Assets/Levels" + mgr.LevelNames[index]);
        }


        EditorGUILayout.EndHorizontal();
        
        
        base.OnInspectorGUI();
        

    }   
}
