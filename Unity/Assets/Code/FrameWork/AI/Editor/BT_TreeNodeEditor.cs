using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(BT_TreeNode))]
public class BT_TreeNodeEditor : EditorPlus 
{
    public override void OnInspectorGUI()
    {
        BT_TreeNode node = (BT_TreeNode)target;

        int maxChars = Mathf.Min(7, node.Name.Length);
        // Redo this
        //node.Tree.NodeWindows[node.ID].Header.text = node.Behavior.Description.Type.ToString().Substring(0, 3) + "-" + node.Name.Substring(0, maxChars) + "[" + node.ID + "]";

        //EditorGUILayout.LabelField("NodeName: ", node.name);
        EditorGUILayout.LabelField("BehaviorType: ", node.Behavior.Description.Type.ToString());
        EditorGUILayout.LabelField("Parent: " + (node.Parent != null ? "Y" : "N") + "| ChildIndex: "+ node.ChildIndex + " | Root: " + (node.IsRoot ? "Y" : "N") + " | Children: " + node.Children.Count);
        //EditorGUILayout.LabelField("Description: " + node.Behavior.Description.Description);
        node.Tree.NodeWindows[node.ID].Position = EditorGUILayout.Vector2Field("Rect Position", node.Tree.NodeWindows[node.ID].Position);

        node.Name = EditorGUILayout.TextField("Identifying name:",node.Name);
        EditorGUILayout.LabelField("Description: ");
        node.Description = EditorGUILayout.TextArea(node.Description);
        

        EditorGUILayout.Space();

        AI_BlackboardComponentEditor.BlackBoardGUI(node.ParametersBB);

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Clear BlackBoard"))
            node.ParametersBB.Clear();
        if (GUILayout.Button("Reset Behavior Parameters"))
            node.SetBehaviorParameters();
        //if (GUILayout.Button("Decouple"))
        //{
        //    node.ParametersBB = AI_Blackboard.CreateShallowCopy(node.ParametersBB);
        //}

        if (GUI.changed)
            BTNodeWindowEditor.Instance.Repaint();
    }
}
