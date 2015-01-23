using UnityEngine;
using System.Collections;

/// <summary>
/// Copies an object from the blackboard or directly passed on an object on the blackboard
/// </summary>
public class BT_Copy : BT_Action 
{
    private const string Override = "OverrideLocation";
    private const string ObjParam = "CopyParameter";
    private const string Obj = "ObjectsToCopy";
    private const string IsObjectStr = "UseObjectToCopy";

    #region Properties

    private AI_AgentParameter P1 { get { return (AI_AgentParameter)Node[Override]; } }

    private AI_AgentParameter P2 { get { return (AI_AgentParameter)Node[ObjParam]; } }

     private object Obj1 { set { Agent[P1] = value; } }

     private bool IsObject { get { return (bool)Node[IsObjectStr]; } }

    private object Obj2 
    { 
        get 
        {
            if (IsObject)
                return Node[Obj];
            else
                return Agent[P2]; 
        } 
    }

    #endregion

    #region Initialization

    public override void SetNodeParameters(BT_TreeNode node)
    {
        node[Override]  = new AI_AgentParameter();
        node[ObjParam]  = new AI_AgentParameter();
        node[Obj]       = null;
        node[IsObjectStr]  = false;
    }

    protected override void SetDescription()
    {
        Description.Type = NodeDescription.BT_NodeType.Action;
        Description.Name = "Copy";
        Description.Description = "Copies the values from slot2 to slot1 and then succeeds (Uses blackboard)";
    }

    #endregion

    #region Get

    public static BT_TreeNode GetTreeNode(AI_AgentParameter accesparam1, object setObject)
    {
        BT_TreeNode node = BT_TreeNode.CreateNode(ScriptableObjectHelper.Create<BT_Copy>());
        return SetParameters(node, accesparam1, setObject);
    }

    public static BT_TreeNode GetTreeNode(string bbParameter, AI_Agent.BlackBoard accesparam1, object setObject)
    {
        return GetTreeNode(new AI_AgentParameter(bbParameter, accesparam1), setObject);
    }

    public static BT_TreeNode GetTreeNode(AI_AgentParameter accesparam1, AI_AgentParameter accesparam2)
    {
        BT_TreeNode node = BT_TreeNode.CreateNode(ScriptableObjectHelper.Create<BT_Copy>());
        return SetParameters(node, accesparam1, accesparam2);
    }

    public static BT_TreeNode GetTreeNode(string bbParameter1, AI_Agent.BlackBoard param1, string bbParameter2, AI_Agent.BlackBoard param2)
    {
        return GetTreeNode(new AI_AgentParameter(bbParameter1, param1), new AI_AgentParameter(bbParameter2, param2));
    }

    #endregion

    #region Set

    public static BT_TreeNode SetParameters(BT_TreeNode node, AI_AgentParameter accesparam1, object setObject)
    {
        node.CheckAndSetClass<BT_Copy>();
        node[Override] = accesparam1;
        node[Obj] = setObject;
        node[IsObjectStr] = true;
        return node;
    }

    public static BT_TreeNode SetParameters(BT_TreeNode node, string bbParameter, AI_Agent.BlackBoard accesparam1, object setObject)
    {
        return SetParameters(node, new AI_AgentParameter(bbParameter, accesparam1), setObject);
    }
    
    public static BT_TreeNode SetParameters(BT_TreeNode node, AI_AgentParameter accesparam1, AI_AgentParameter accesparam2)
    {
        node.CheckAndSetClass<BT_Copy>();
        node[Override] = accesparam1;
        node[ObjParam] = accesparam2;
        node[IsObjectStr] = false;
        return node;
    }
    public static BT_TreeNode SetParameters(BT_TreeNode node, string bbParameter1, AI_Agent.BlackBoard param1, string bbParameter2, AI_Agent.BlackBoard param2)
    {
        return SetParameters(node, new AI_AgentParameter(bbParameter1, param1), new AI_AgentParameter(bbParameter2, param2));
    }

    #endregion

    protected override Status update()
    {
        Obj1 = Obj2;

        return Status.Succes;
    }
}
