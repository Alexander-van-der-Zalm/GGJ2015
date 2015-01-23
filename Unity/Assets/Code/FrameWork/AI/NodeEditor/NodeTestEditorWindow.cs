using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;



[System.Serializable]
public class NodeTestEditorWindow : NodeEditorWindow
{
    [SerializeField]
    private string path = "Assets/TestNode.asset";
    
    // Constructor
    [MenuItem("CustomTools/Node Test Editor")]
    public static void ShowWindow()
    {
        Instance = EditorWindow.GetWindow<NodeTestEditorWindow>();
        Instance.Init();
    }

    public override void Init()
    {
        base.Init();

        generateTestNodes();
    }

    void Update()
    {
        
        //// Check if tree is selected
        //if (Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<AI_Agent>() != null)
        //{
        //    //Todo tree processing if not done yet
        //    AI_Agent agent = Selection.activeGameObject.GetComponent<AI_Agent>();
        //    if(agent.Tree != null)
        //        Selection.objects = new UnityEngine.Object[1]{ (UnityEngine.Object)agent.Tree};
        //}
    }

    protected override void DrawButtons()
    {
        // Temp test buttons for functionality
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Create new node"))
            addTestNode();
        if (GUILayout.Button("Connect 1 to 0"))
            ConnectChild(1, 0);
        if (GUILayout.Button("Connect 0 to 1"))
            ConnectChild(0, 1);
        if (GUILayout.Button("Select Test Object"))
            Selection.objects = selection;
        //if (GUILayout.Button("Create Test Object"))
        //    test.CreateAsset();
        if (GUILayout.Button("Print 0 childCount"))
            Debug.Log(windows[0].Children.Count);

        EditorGUILayout.LabelField("Focus on window: " + FocusID);
        EditorGUILayout.EndHorizontal();

        // Temp move buttons
        // Move to base
        if (GUI.RepeatButton(new Rect(20, 40, 20, 20), "<"))
        {
            panX++;
            Repaint();
        }

        if (GUI.RepeatButton(new Rect(40, 40, 20, 20), ">"))
        {
            panX--;
            Repaint();
        }

        if (GUI.RepeatButton(new Rect(30, 20, 20, 20), "^"))
        {
            panY++;
            Repaint();
        }

        if (GUI.RepeatButton(new Rect(30, 60, 20, 20), "v"))
        {
            panY--;
            Repaint();
        }
        EditorGUILayout.BeginHorizontal();
        path = EditorGUILayout.TextField("Path", path);
        //if (GUILayout.Button("AddNode"))
        //    ScriptableObjectHelper.AddObjectToAsset()
        //    BT_TreeNode.CreateObjAddToAsset(path);
        //if (GUILayout.Button("CreateTree"))
        //    BT_Tree.CreateObjAndAsset(path);
        EditorGUILayout.EndHorizontal();
    }


    #region Test

    private void generateTestNodes()
    {
        float width = 100;
        float height = 100;

        List<Vector2> topLeftPos = new List<Vector2>();
        topLeftPos.Add(new Vector2(110, 10));
        topLeftPos.Add(new Vector2(10, 210));
        topLeftPos.Add(new Vector2(210, 210));

        for (int i = 0; i < 3; i++)
        {
            windows.Add(NodeWindow.CreateInstance<NodeWindow>());
            windows[i].Init(i, topLeftPos[i], width, height, "TestNode " + i);
        }

        windows[0].AddChildren(windows[1], windows[2]);
    }

    private void addTestNode()
    {
        windows.Add(NodeWindow.CreateInstance<NodeWindow>());
        int id = windows.Count - 1;
        windows[id].Init(id, new Vector2(10, 10), 100, 100, "TestNode " + id);
    }

    #endregion
}
