using UnityEngine;
using Framework.Collections;

public class BT_QueuePop : BT_Action 
{
    const string popStr = "PopedObjParam";
    private const string queueStr = "QueueParameter";

    #region Properties

    private AI_AgentParameter popParam { get { return (AI_AgentParameter)Node[popStr]; } }
    private AI_AgentParameter queueParam { get { return (AI_AgentParameter)Node[queueStr]; } }
    private IQueue Queue 
    {
        // Handle exceptions?
        get { return (IQueue)Agent[queueParam]; } 
    }

    private object PoppedObject { set { Agent[popParam] = value; } }

    #endregion

    #region Constructor

    public override void Init(HideFlags newHideFlag = HideFlags.None)
    {
        description();
    }

    private void description()
    {
        Description.Type = NodeDescription.BT_NodeType.Action;
        Description.Name = "QueuePop";
        Description.Description = "Pop one of the queue, into the parameterdestination of choice";
    }

    public override void SetNodeParameters(BT_TreeNode node)
    {
        node[queueStr] = new AI_AgentParameter();
        node[popStr] = new AI_AgentParameter();
    }

    #endregion

    #region Get Set

    public static BT_TreeNode GetTreeNode(AI_AgentParameter QueueParam, AI_AgentParameter PopParam)
    {
        BT_TreeNode node = BT_TreeNode.CreateNode(ScriptableObjectHelper.Create<BT_QueuePop>());
        return SetParameters(node, QueueParam, PopParam);
    }

    public static BT_TreeNode SetParameters(BT_TreeNode node, AI_AgentParameter QueueParam, AI_AgentParameter PopParam)
    {
        node.CheckAndSetClass<BT_QueuePop>();
        node[popStr] = PopParam;
        node[queueStr] = QueueParam;

        return node;
    }

    #endregion
   
    protected override Status update()
    {
        PoppedObject = Queue.Get();
        
        return Status.Succes;
    }
}
