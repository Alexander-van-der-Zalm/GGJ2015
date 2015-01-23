using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections;
using Status = BT_Behavior.Status;

public class BTNodeWindow : NodeWindow
{
    #region Classes
    //TODO tree info & Pos info

    #endregion

    #region Fields

    // TreeInfo
    //int currentTreeRevision;
    //BT_Tree Tree;

    // Pos Info

    // Functional info (used for drawing)
    [SerializeField]
    private BT_TreeNode treeNode;

    [SerializeField]
    private Status status;

    [SerializeField]
    private bool agentRunning;

    #endregion

    #region Properties

    public BT_TreeNode TreeNode { get { return treeNode; } private set { treeNode = value; } }

    //public Status Status { get { return status; } private set { status = value; } }

    public override int ID
    {
        get { return base.ID; }
        set
        {
            base.ID = value;
            SetName();
        }
    }

    private void SetName()
    {
        name = id + " | WINDOW | " + (TreeNode != null ? TreeNode.Behavior.Description.Name : "");
        Header = new GUIContent((TreeNode != null ? TreeNode.Behavior.Description.Type.ToString() : "")+" [" + id + "]");
        //RefreshAsset();
    }

    #endregion

    #region Ctor

    internal static BTNodeWindow CreateWindow(BT_TreeNode node, UnityEngine.Object asset, int id)
    {
        BTNodeWindow window = ScriptableObject.CreateInstance<BTNodeWindow>();
        window.Init();
        window.TreeNode = node;
        window.ID = id;
        window.SetName();
        window.BGColor = GUI.color;

        window.Rect = new Rect(100, 100, 100, 100);

        window.status = Status.Invalid;

        // Add object to asset
        //ScriptableObjectHelper.AddObjectToAsset(asset, window);
        window.AddObjectToAsset(asset);

        return window;
    }

    #endregion

    #region Draw Window

    protected override void DrawWindowContent(int id)
    {
        // Makes it draggable
        base.DrawWindowContent(id);
        //TODO render this window

        int childIndex = TreeNode.ChildIndex;
        EditorGUILayout.LabelField("("+TreeNode.Behavior.Description.Name + ")" + (childIndex == -1 ? "" : "[" + childIndex + "]"));
        if (!TreeNode.Name.Equals(string.Empty))
        {
            EditorGUILayout.LabelField(TreeNode.Name);
        }
        if (TreeNode.IsRoot)
            EditorGUILayout.LabelField("Root");

        // If active agent draw a color and text


        // Handle being pressed
        if (((BTNodeWindowEditor)BTNodeWindowEditor.Instance).IsPressedParent(ID))
            BGColor = BTNodeWindowSettings.SelectedNodeColor;
        else if(agentRunning)
        {
            switch(status)
            {
                case Status.Failed:
                    BGColor = BTNodeWindowSettings.FailedNodeColor;
                    break;
                case Status.Invalid:
                    BGColor = BTNodeWindowSettings.InvalidNodeColor;
                    break;
                case Status.Running:
                    BGColor = BTNodeWindowSettings.RunningNodeColor;
                    break;
                case Status.Succes:
                    BGColor = BTNodeWindowSettings.SuccesNodeColor;
                    break;
            }
            EditorGUILayout.LabelField(status.ToString());
        }
        else
            BGColor = BTNodeWindowSettings.StandardNodeColor;
    }

    protected override void OnMoved()
    {
        base.OnMoved();
        //BT_TreeNode
        EditorUtility.SetDirty(TreeNode);
    }

    #endregion

    #region SetAgentStatus

    public void SetAgentStatus(bool activeAgent, Status newStatus = Status.Invalid)
    {
        agentRunning = activeAgent;
        status = newStatus;
    }

    #endregion

    #region Sort

    public void SortChildrenByXMin()
    {
        Children = Children.OrderBy(c => c.Rect.xMin).ToList();
    }

    #endregion
}
