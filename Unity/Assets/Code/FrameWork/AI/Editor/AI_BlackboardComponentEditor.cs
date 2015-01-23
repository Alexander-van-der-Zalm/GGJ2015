using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(AI_BlackboardComponent))]
public class AI_BlackboardComponentEditor : EditorPlus
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        AI_BlackboardComponent bbc = (AI_BlackboardComponent)target;
        AI_Blackboard bb = bbc.Blackboard;

        if (bb == null)
        {
            using (new FixedWidthLabel("No blackboard exists. "))
            {
                if (GUILayout.Button("Create new BB."))
                {
                    bbc.Blackboard = AI_Blackboard.Create();
                    bb = bbc.Blackboard;
                }
                else
                    return;
            }
        }

        //Debug.Log(bb.ObjectPool.Count);

        //bb.Init();        
                    
        // Blackboard name
        GUILayout.BeginHorizontal();
        using (new FixedWidthLabel("Name: "))
            bb.Name = EditorGUILayout.TextField(bb.Name);
        GUILayout.EndHorizontal();

        //Blackboard function
        bbc.Blackboard = BlackBoardGUI(bb);

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Clear BlackBoard"))
            bb.Clear();
        if (GUILayout.Button("Decouple"))
        {
            bbc.Blackboard = AI_Blackboard.CreateShallowCopy(bb);
        }
        if (GUILayout.Button("Set test values"))
        {
            bb["TestInt"] = 1;
            bb["TestFloat"] = 1.0f;
            bb["TestVariable"] = null;
            bb["TestParamater"] = new AI_AgentParameter();
        }

        GUILayout.EndHorizontal();
    }

    public static AI_Blackboard BlackBoardGUI(AI_Blackboard bb)
    {
        EditorGUILayout.LabelField("[" + bb.ObjectPool.Count + "] Parameter items: ");

        GUILayout.BeginVertical();
        EditorGUI.indentLevel++;


        List<string> keys = bb.ObjectPool.Keys.ToList();

        foreach (string str in keys)
        {
            bool variableObject = bb.IsVariableObject[str];
            //object original = bb[str];
            object value = EditorField(bb[str], str, false, variableObject);

            // Set value
            bb.SetObject(str, value);
        }

        EditorGUI.indentLevel--;
        GUILayout.EndVertical();

        return bb;
    }
}