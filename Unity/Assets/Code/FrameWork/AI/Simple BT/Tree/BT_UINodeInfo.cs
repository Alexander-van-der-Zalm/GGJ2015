using UnityEngine;
using System.Collections;

[System.Serializable]
public class BT_UINodeInfo  
{
    public Vector3 Position;
    public BT_UINode UINode;

    public BT_Tree Tree;
    public BT_UINodeInfo Parent;
    public BT_TreeNode TreeNode;
    public int Depth;
    public int Index;

    public int ParentIndex { get { return Parent != null ? Parent.Index : -1; } }
    public int ChildrenCount { get { return TreeNode == null ? 0 : TreeNode.Children.Count; } }

    public BT_UINodeInfo(int depth, int rank, BT_TreeNode node, BT_UINodeInfo parent, BT_Tree tree)
    {
        Depth = depth;
        Index = rank;
        TreeNode = node;
        Parent = parent;
        Tree = tree;
    }

    public void SetBehavior(BT_BBParameters behavior)
    {
        if (TreeNode == null)
            TreeNode = BT_TreeNode.CreateNode(behavior);
        else
            TreeNode.Behavior = behavior;
    }
}
