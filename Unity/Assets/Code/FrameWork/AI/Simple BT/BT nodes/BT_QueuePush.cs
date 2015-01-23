using UnityEngine;
using Framework.Collections;

public class BT_QueuePush : BT_Action  
{
    private const string PushObjPar = "ObjectToPushParameter";
    private const string ObjStr = "ObjectToPush";
    private const string IsObjectStr = "UseObjectToPush";
    protected const string QueueStr = "QueueParameter";

    #region Properties

    private AI_AgentParameter queueParam { get { return (AI_AgentParameter)Node[QueueStr]; } }
    private AI_AgentParameter P2 { get { return (AI_AgentParameter)Node[PushObjPar]; } }
    private IQueue Queue
    {
        // Handle exceptions?
        get { return (IQueue)Agent[queueParam]; }
    }

    private bool IsObject { get { return (bool)Node[IsObjectStr]; } }

    private object PushObject
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

    public BT_QueuePush()
    {
        Description.Type = NodeDescription.BT_NodeType.Action;
        Description.Name = "QueuePush";
        Description.Description = "Pushes a value to the queue from the agents blackboard";
    }

    public override void SetNodeParameters(BT_TreeNode node)
    {
        node[QueueStr] = new AI_AgentParameter();
        node[PushObjPar] = new AI_AgentParameter();
        node[ObjStr] = null;
        node[IsObjectStr] = false;
    }

    #endregion

    #region Get Set

    public static BT_TreeNode GetTreeNode(AI_AgentParameter QueueParam, object pushObj)
    {
        BT_TreeNode node = BT_TreeNode.CreateNode(ScriptableObjectHelper.Create<BT_QueuePush>());
        return SetParameters(node, QueueParam, pushObj);
    }

    public static BT_TreeNode SetParameters(BT_TreeNode node, AI_AgentParameter QueueParam, object toPushObject)
    {
        node.CheckAndSetClass<BT_QueuePush>();
        node[QueueStr] = QueueParam;
        node[ObjStr] = toPushObject;
        node[IsObjectStr] = true;
        return node;
    }

    public static BT_TreeNode GetTreeNode(AI_AgentParameter QueueParam, AI_AgentParameter PushParam)
    {
        BT_TreeNode node = BT_TreeNode.CreateNode(ScriptableObjectHelper.Create<BT_QueuePush>());
        return SetParameters(node, QueueParam, PushParam);
    }

    public static BT_TreeNode SetParameters(BT_TreeNode node, AI_AgentParameter QueueParam, AI_AgentParameter PushParam)
    {
        node.CheckAndSetClass<BT_QueuePush>();
        node[QueueStr] = QueueParam;
        node[PushObjPar] = PushParam;
        node[IsObjectStr] = false;
        return node;
    }

    #endregion
    
    protected override Status update()
    {
        Queue.Add(PushObject); 

        return Status.Succes;
    }
}
