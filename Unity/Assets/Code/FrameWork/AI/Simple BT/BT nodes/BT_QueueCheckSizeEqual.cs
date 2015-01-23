using UnityEngine;
using System.Linq;
using Framework.Collections;

public class BT_QueueCheckSize : BT_Condition
{
    private const string SizeObjPar = "SizeBBParameter";
    private const string ObjStr = "SizeCount";
    private const string IsObjectStr = "UseObjectToCompare";
    protected const string QueueStr = "QueueParameter";

    #region Properties

    // Parameters

    private AI_AgentParameter queueParam { get { return (AI_AgentParameter)Node[QueueStr]; } }
    private AI_AgentParameter P2 { get { return (AI_AgentParameter)Node[ObjStr]; } }
    private IQueue Queue
    {
        // Handle exceptions?
        get { return (IQueue)Agent[queueParam]; }
    }

    private bool IsObject { get { return (bool)Node[IsObjectStr]; } }

    private object CountToCompare
    {
        get
        {
            if (IsObject)
                return Node[ObjStr];
            else
                return Agent[P2];
        }
    }

    #endregion

    #region Constructor

    protected override void SetDescription()
    {
        Description.Type = NodeDescription.BT_NodeType.Action;
        Description.Name = "QueueCheckSize";
        Description.Description = "Check if the queue size is equal to the count (use int with parameter)";
    }

    public override void SetNodeParameters(BT_TreeNode node)
    {
        node[QueueStr] = new AI_AgentParameter();
        node[SizeObjPar] = new AI_AgentParameter();
        node[ObjStr] = (int)0;
        node.ParametersBB[ObjStr] = null;
        node[IsObjectStr] = false;
    }

    #endregion

    #region Get and Set

    public static BT_TreeNode GetTreeNode(AI_AgentParameter QueueParam, object sizeObject)
    {
        BT_TreeNode node = BT_TreeNode.CreateNode(ScriptableObjectHelper.Create<BT_QueueCheckSize>());
        return SetParameters(node, QueueParam, sizeObject);
    }

    public static BT_TreeNode SetParameters(BT_TreeNode node, AI_AgentParameter QueueParam, object sizeObject)
    {
        node.CheckAndSetClass<BT_QueueCheckSize>();
        node[QueueStr] = QueueParam;
        node[ObjStr] = sizeObject;
        node[IsObjectStr] = true;
        return node;
    }

    public static BT_TreeNode GetTreeNode(AI_AgentParameter QueueParam, AI_AgentParameter sizeObjParam)
    {
        BT_TreeNode node = BT_TreeNode.CreateNode(ScriptableObjectHelper.Create<BT_QueueCheckSize>());
        return SetParameters(node, QueueParam, sizeObjParam);
    }

    public static BT_TreeNode SetParameters(BT_TreeNode node, AI_AgentParameter QueueParam, AI_AgentParameter sizeObj)
    {
        node.CheckAndSetClass<BT_QueueCheckSize>();
        node[QueueStr] = QueueParam;
        node[SizeObjPar] = sizeObj;
        node[IsObjectStr] = false;
        return node;
    }

    #endregion

    protected override Status update()
    {
        int size = (int)CountToCompare; 
        
        return Queue.Count == size ? Status.Succes : Status.Failed;
    }
}
