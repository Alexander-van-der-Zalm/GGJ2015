using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(BlockManager))]
public class BlockManagerEditor : EditorPlus
{
    public override void OnInspectorGUI()
    {
        BlockManager mgr = target as BlockManager;
        
        mgr.LevelNames = mgr.FindLevelNames();

        EditorGUILayout.BeginHorizontal();

        int index = EditorPrefs.GetInt("Level", 0);
        index = EditorGUILayout.Popup(index, mgr.LevelNames.ToArray());
        EditorPrefs.SetInt("Level", index);

        if (GUILayout.Button("Load Level"))
        {
            BlockManager.LoadLevel(mgr.LevelNames[index]);
        }
        if (GUILayout.Button("Save Level"))
        {
            BlockManager.SaveLevel();
        }
        if (GUILayout.Button("Clear"))
        {
            BlockManager.Instance.ClearBlocks();
        }
            
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Create New Pallet"))
        {
            ColorPalette pal = ColorPalette.Create();
            pal.Init();
            ScriptableObjectHelper.SaveAssetAutoNaming(pal, "Assets/ColorPalletes");
            Selection.activeObject = pal;

        }
        if (GUILayout.Button("ChangeColor"))
        {
            GameManagement.Block.SetTeamColor();
        }

        EditorGUILayout.EndHorizontal();
        
        base.OnInspectorGUI();
        

    }   
}
