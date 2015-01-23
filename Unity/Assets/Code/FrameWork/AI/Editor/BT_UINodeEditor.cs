using UnityEditor;
using UnityEngine;

using System;
using System.Reflection;
using System.Linq;
using System.Collections;

using nodeType = BTNodeWindowEditor.nodeType;
using NodeDescription = BT_Behavior.NodeDescription;
using System.Collections.Generic;

[CustomEditor(typeof(BT_UINode))]
public class BT_UINodeEditor : EditorPlus
{
    private int selectedClass = 0;
    private int selectedField = 0;
    private nodeType lastType = nodeType.Action;
    private nodeType curType = nodeType.Composite;
    

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        // Get refs
        BT_UINode uiNode = (BT_UINode)target;
        BT_UINodeInfo nodeInfo = uiNode.NodeInfo;

        // BB & UINode hookup check
        uiNode.Init();

        // Check if there is a behavior
       // Debug.Log(nodeInfo + " " + nodeInfo.TreeNode);
        bool hasNode = nodeInfo.TreeNode == null ? false : nodeInfo.TreeNode.Behavior != null;

        // Todo:
        // Node Info properly
        bool hasParent = nodeInfo.Parent != null;
        bool hasTree = nodeInfo.Tree != null;
        //using
        string line1 = "Connected to Tree[" + hasTree + "] - TreeNode set [" + hasNode + "]";
        string line2 = "Node pos[" + nodeInfo.Depth + "," + nodeInfo.Index + "] with [" + nodeInfo.ChildrenCount + "] children";
        string line3 = "Parent[" + hasParent + "]";
        line3 += hasParent ? " @ [" + nodeInfo.Depth + 1 + "," + nodeInfo.Parent.Index + "]" : "";

        EditorGUILayout.LabelField("Node Info: ");
        EditorGUI.indentLevel++;
            EditorGUILayout.LabelField(line1);
            EditorGUILayout.LabelField(line2);
            EditorGUILayout.LabelField(line3);
        EditorGUI.indentLevel--;

        EditorGUILayout.Separator();

        EditorGUILayout.LabelField("Behavior Info: ");
        EditorGUI.indentLevel++; 
        if (nodeInfo.TreeNode != null && nodeInfo.TreeNode.Behavior != null)
        {
            EditorGUILayout.LabelField("Name:         "+nodeInfo.TreeNode.Behavior.Description.Name);
            EditorGUILayout.LabelField("Desciription: "+nodeInfo.TreeNode.Behavior.Description.Description);
        }
        else
        {
            EditorGUILayout.LabelField("No Behavior Set");
            //EditorGUILayout.LabelField(nodeInfo.TreeNode.Behavior.ToString() + " " + (nodeInfo.TreeNode.Behavior==null).ToString());
        }
        EditorGUI.indentLevel--;
        

        EditorGUILayout.Separator();
        EditorGUILayout.LabelField("Temp replace node:");

        EditorGUILayout.BeginHorizontal();

        curType = (nodeType)EditorGUILayout.Popup((int)curType, Enum.GetNames(typeof(nodeType)));

        nodeType ntype = curType;//hasNode ? nodeInfo.TreeNode.Behavior.Description.Type : 
        Type type = GetType(ntype);

        if (lastType != ntype)
        {
            selectedClass = 0;
            selectedField = 0;
        }
        lastType = ntype;

        // Get all the classes from the assembly that inherent from the selected BT node type
        var q1 = from t in Assembly.GetAssembly(typeof(BT_Behavior)).GetTypes()
                 where t.IsClass && (t.IsSubclassOf(type))// && !t.GetInterfaces().Contains(typeof(IReflectionIgnore)))//t == type) // No more equal types
                 select t;

        var q2 = from t in q1
                 select t.Name.ToString();


        List<string> classList = q2.ToList<string>();

        selectedClass = EditorGUILayout.Popup(selectedClass, classList.ToArray());

        var l1 = q1.ToList();
        if (GUILayout.Button("Replace node"))
        {
            ResetParameters(uiNode,(BT_BBParameters)Activator.CreateInstance(l1[selectedClass]));
        }
        
        EditorGUILayout.EndHorizontal();

        //if (nodeInfo.TreeNode != null && nodeInfo.TreeNode.ParametersBB.ObjectPool.Count > 0)
        //    if(GUILayout.Button("Reset parameters"))
        //        ResetParameters(uiNode, nodeInfo.TreeNode.Behavior);
        //// Show all the fields from the selected class
        //var fields = l1[selectedClass].GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy);

        //var q3 = from f in fields
        //         select f.Name.ToString();

        //List<string> fieldList = q3.ToList();

        //var q4 = fields.Where(f => f.Name == "Description").FirstOrDefault();

        //selectedField = EditorGUILayout.Popup(selectedField, fieldList.ToArray());
    }

    private void ResetParameters(BT_UINode uiNode, BT_BBParameters behavior)
    {
        uiNode.ChangeBehavior(behavior);
    }

    private Type GetType(nodeType bT_NodeType)
    {
        switch (bT_NodeType)
        {
            case nodeType.Action:
                return typeof(BT_Action);
            case nodeType.Condition:
                return typeof(BT_Condition);
            case nodeType.Decorator:
                return typeof(BT_Decorator);
            case nodeType.Composite:
                return typeof(BT_Composite);
        }
        return typeof(BT_Behavior);
    }
}
