using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(AI_AgentComponent))]
public class AI_AgentEditor : EditorPlus 
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        AI_Agent agent = ((AI_AgentComponent)target).Agent;
        BT_Tree tree = agent.Tree;

        if (GUI.changed)
        { // Force a selection change
            if (BTNodeWindowEditor.IsOpen)
                BTNodeWindowEditor.Instance.SelectionChange();
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Tree Controls:");

        #region Show Hide Tree

        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("Show Tree"))
            {
                BTNodeWindowEditor.ShowWindow();
                BTNodeWindowEditor.Instance.SelectionChange();
            }

            if (GUILayout.Button("Hide Tree"))
            {
                BTNodeWindowEditor.HideWindow();
            }
        }
        EditorGUILayout.EndHorizontal();

        #endregion

        #region Stop Start Tree

        EditorGUILayout.BeginHorizontal();
        {
            // Stop start tree
            if (agent.TreeRunning)
            {
                if (GUILayout.Button("Stop Tree"))
                {
                    //agent.TreeTick();
                    //BTNodeWindowEditor.Instance.Repaint();
                }
            }
            else
            {
                if (GUILayout.Button("Start Tree"))
                {
                    //agent.TreeTick();
                    //BTNodeWindowEditor.Instance.Repaint();
                }
            }

            // Manual tick
            if (GUILayout.Button("Tick Tree"))
            {
                // Pause tree?
                agent.TreeTick();
                BTNodeWindowEditor.Instance.Repaint();
            }
        }
        EditorGUILayout.EndHorizontal();

        #endregion
    }
}
