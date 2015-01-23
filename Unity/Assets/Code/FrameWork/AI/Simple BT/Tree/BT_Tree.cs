using UnityEngine;
using System.Linq;
using Framework.Collections;
using System.Collections;
using System.Collections.Generic;
using Status = BT_Behavior.Status;
using fc = Framework.Collections;


using System;
using System.Reflection;

[System.Serializable]
public class BT_Tree : EasyScriptableObject<BT_Tree>
{
    #region classes

    [Serializable]
    public class TreeInfo
    {
        public string Name = "TreeNoName";
        
        [Range(0.00000000001f, 60)]
        public float UpdateFrequency = 10;

        public int TreeIteration = 0;

        public float NodeSide = 1;
    }

    #endregion

    #region Fields

    // Stores meta information of the tree
    public TreeInfo Info;
    
    // Collection of functional tree elements
    [SerializeField]
    private List<BT_TreeNode> treeNodes;

    // Collection of visual representations of tree elements
    [SerializeField]
    private List<BTNodeWindow> nodeWindows;

    [SerializeField]
    private BT_TreeNode root;

    #endregion

    #region Properties

    public BT_TreeNode Root { get { return root; } private set { root = value; } }

    public List<BT_TreeNode> TreeNodes
    {
        get { return treeNodes != null ? treeNodes : treeNodes = new List<BT_TreeNode>(); }
        private set { treeNodes = value; }
    }

    // If uneven- return rebuild
    public List<BTNodeWindow> NodeWindows
    {
        get { return nodeWindows != null ? nodeWindows : nodeWindows = new List<BTNodeWindow>(); }
        private set { nodeWindows = value; }
    }

    public BT_TreeNode this[int id]
    {
        get { return TreeNodes[id]; }
    }

    public int Version { get { return Info.TreeIteration; } }

    #endregion

    #region Init

    public override void Init(HideFlags newHideFlag = HideFlags.None)
    {
        base.Init(newHideFlag);
        Info = new TreeInfo();
    }

    #endregion

    #region Update Co-Routine

    //// Use this for initialization
    //void Start () 
    //{
    //    //TestTreeFunctionality();
    //    if(Tree != null)
    //        StartCoroutine(updateCR());
    //}

    //// Update loop
    //public IEnumerator updateCR(AI_Agent agent)
    //{
    //    if(Root == null)
    //    {
    //        Debug.Log("BT_BehaviorTree not populated.");
    //         yield break;
    //    }
    //    while (Application.isPlaying)
    //    {
    //        Root.Tick(agent);

    //        yield return new WaitForSeconds(1.0f / Info.UpdateFrequency);
    //    }
    //}

    #endregion

    #region Rebuild Tree (recursive)

    private int treeDepth;

    private void RebuildTreeFromRoot()
    {
        Root = FindRoot(Root);
        RebuildTreeFromRoot(Root);
    }

    private BT_TreeNode FindRoot(BT_TreeNode root)
    {
        while(root.Parent != null)
        {
            root = root.Parent;
        }
        return root;
    }

    private void RebuildTreeFromRoot(BT_TreeNode root)
    {
        Root = FindRoot(root);
        TreeNodes = new List<BT_TreeNode>();
        
        TreeNodes  = RecursiveTreeNodeCrawl(TreeNodes, root);
        
        // Set the new tree iteration
        Info.TreeIteration = Info.TreeIteration + 1 % int.MaxValue;
    }

    private List<BT_TreeNode> RecursiveTreeNodeCrawl(List<BT_TreeNode> list, BT_TreeNode node)
    {
        // All the nodes have already a valid parent and children
        // Ids need to be set and unique for this rebuild 
        // Then they need to be added to the list
        
        // First loop over children recursively
        for (int i = 0; i < node.Children.Count; i++)
        {
            list = RecursiveTreeNodeCrawl(list, node.Children[i]);
        }

        // Set ID
        node.ID = list.Count;
        
        // Add self to list
        list.Add(node);

        return list;
    }

    #endregion

    #region Get children

    public BT_TreeNode GetFirstChild(int index)
    {
        return TreeNodes[index].Children.First();
    }

    public List<BT_TreeNode> GetChildren(int index)
    {
        return TreeNodes[index].Children;
    }

    #endregion

    #region Tree creation

    public static BT_Tree CreateTree(BT_TreeNode newRoot)
    {
        BT_Tree tree = Create();
        tree.Root = newRoot;

        if (newRoot == null)
            return tree;

        // Set right root and set ids
        tree.RebuildTreeFromRoot();

        return tree;
    }

    #endregion

    #region Node management

    // Add node
    public BT_TreeNode CreateNode(BT_BBParameters behavior)
    {
        int id = TreeNodes.Count;
        // Create the asset and connect it to this asset
        // Node Parameters
        AI_Blackboard newBB = AI_Blackboard.Create();
        //newBB.name = id + " | PARAMATERS | " + behavior.Description.Name;
        //ScriptableObjectHelper.AddObjectToAsset(this, newBB);
        newBB.AddObjectToAsset(this);

        // Functional node part
        BT_TreeNode newNode = BT_TreeNode.CreateNode(behavior, id, newBB, this);
        
        // Create UI counterpart
        BTNodeWindow newWindow = BTNodeWindow.CreateWindow(newNode, this, id);

        // Add to lists
        TreeNodes.Add(newNode);
        NodeWindows.Add(newWindow);

        // Make root if there is none
        if (Root == null)
            Root = newNode;

        // Tree has changed
        Info.TreeIteration++;

        return newNode;
    }

    // Remove node
    public void DestroyNode(int index)
    {
        // Exit out if index out of bounds
        if (illegalTreeNodeIndex(index))
            return;

        // Tree has changed
        Info.TreeIteration++;

        // Remove parent children connections & Parent connection from children
        TreeNodes[index].DisconnectAll();
        NodeWindows[index].DisconnectAll();

        // Remove assets
        UnityEngine.Object.DestroyImmediate(TreeNodes[index].ParametersBB, true);
        UnityEngine.Object.DestroyImmediate(TreeNodes[index], true);
        UnityEngine.Object.DestroyImmediate(NodeWindows[index], true);

        // Select the tree
        UnityEditor.Selection.objects = new UnityEngine.Object[] { this };

        // Remove at index
        TreeNodes.RemoveAt(index);
        NodeWindows.RemoveAt(index);

        // Reorder indices
        SimpleReOrderIndices();

        // ISWORK :3
        ScriptableObjectHelper.RefreshAsset(this);
        //RefreshAsset();

    }

    // Connect (child & parent)
    public void Connect(int parentIndex, int childIndex)
    {
        // Cannot connect to itself
        if(parentIndex == childIndex)
        {
            Debug.LogError("BT_Tree.Connect child and parent index are the same");
            return;
        }
        // Check indices
        if (illegalTreeNodeIndex(parentIndex) || illegalTreeNodeIndex(childIndex))
            return;

        // Check if it is trying to child one of its ancestors
        if (TreeNodes[parentIndex].IsDescendantOf(TreeNodes[childIndex]))
        {
            // Then disconnect parent and carry one
            TreeNodes[parentIndex].DisconnectFromParent();
            NodeWindows[parentIndex].DisconnectFromParent();
        }

        // Connect TreeNode and BTNodeWindow
        TreeNodes[parentIndex].AddChildren(TreeNodes[childIndex]);
        NodeWindows[parentIndex].AddChildren(NodeWindows[childIndex]);

        // Tree has changed
        Info.TreeIteration++;

        // Reorder indices
        SimpleReOrderIndices();
        SortChildrenBasedOnPosition();
    }

    // Disconnect (child & parent)

    #region Helpers

    private void SimpleReOrderIndices()
    {
        // Super simple
        for (int i = 0; i < TreeNodes.Count; i++)
        {
            TreeNodes[i].ID = i;
            TreeNodes[i].ParametersBB.name = i + " | PARAMATERS | " + TreeNodes[i].Behavior.Description.Name;
            NodeWindows[i].ID = i;
        }
    }


    private bool illegalTreeNodeIndex(int index)
    {
        bool illegal = index >= TreeNodes.Count || index < 0;
        if (illegal)
            Debug.LogError("BT_Tree.illegalTreeNodeIndex found an illegal index!!");
        return illegal;

    }
    #endregion

    #endregion

    #region ReOrderChildrenBasedOnX

    public void SortChildrenBasedOnPosition()
    {
        for(int i = 0; i < TreeNodes.Count; i++)
        {
            if(TreeNodes[i].HasChildren && TreeNodes[i].Children.Count > 1)
            {
                // Terribly inefficient
                // Needs a redesign of the double children in treenodes and windows to
                // ID, Parent and Children being only present in TreeNode

                // Sort by rect xMin
                NodeWindows[i].SortChildrenByXMin();
                // Create an id list for sorting of treenode childrens
                List<int> ids = NodeWindows[i].Children.Select(c => c.ID).ToList();
                TreeNodes[i].SortChildrenByIDS(ids);

            }
        }
    }

    #endregion

    #region UI nodes (LEGACY)

    internal List<BT_UINodeInfo> GetUINodes()
    {
        //if (UINodes == null)
        //    return DefaultUINodeList();

        // Decide how to handle script initilalization of extra tree nodes
        // Decide what to do when there is already ui nodes...
        return DefaultUINodeList();
    }

    private List<BT_UINodeInfo> DefaultUINodeList()
    {
        // Reset tree info
        resetTreeInfo();

        // Fill the 2d list
        List<List<BT_UINodeInfo>> list = new List<List<BT_UINodeInfo>>();
        
        // Recursive fill
        Fill(list, Root, null);

        // Set grid positions

        // Scale positions?

        List<BT_UINodeInfo> flatList = GetFlatList(list);

        return flatList;
    }

    private List<BT_UINodeInfo> GetFlatList(List<List<BT_UINodeInfo>> masterlist)
    {
        List<BT_UINodeInfo> output = new List<BT_UINodeInfo>();

        foreach (List<BT_UINodeInfo> list in masterlist)
            output.AddRange(list);
        
        return output;
    }

    private void Fill(List<List<BT_UINodeInfo>> list, BT_TreeNode node, BT_UINodeInfo parent)
    {
        // Empty tree exit out
        if (node == null)
            return;
        // Set depth based on the parents depth
        int depth = parent == null ? 0 : parent.Depth + 1;
        
        // Set new treeDepth if now deeper then before
        // Also create a new list for that depth
        if (depth > treeDepth)
        {
            treeDepth = depth;
            list.Add(new List<BT_UINodeInfo>());
        }
            
        // index in the row (0 for slot 0, 1 for slot 1, etc.)
        int index = list[depth].Count; 
        
        // Create a new nodeInfo
        BT_UINodeInfo nodeInfo = new BT_UINodeInfo(depth, index, node, parent, this);

        // Add to datastructure
        list[depth].Add(nodeInfo);

        Debug.Log("GenerateNode[d " + depth + " :i " + index + "] NI: " + nodeInfo + " P: " + list[depth][index].Parent);

        // Add children
        for (int i = 0; i < node.Children.Count; i++)
            Fill(list, node.Children[i], nodeInfo);
        // Could add children to nodeInfo as well if needed
    }

    private void resetTreeInfo()
    {
        treeDepth = -1;
    }

    #endregion

    internal void SetRoot(int id)
    {
        // Loop till you got the most upper parent
        BT_TreeNode node = TreeNodes[id];

        while(node.Parent != null)
        {
            node = node.Parent;
        }

        Root = node;
    }

    #region AgentTreeStatusReset

    internal List<Status> GetNodeStatus()
    {
        List<Status> agentMemory = new List<Status>();

        for (int i = 0; i < TreeNodes.Count; i++)
            agentMemory.Add(Status.Invalid);

        return agentMemory;
    }

    #endregion
}
