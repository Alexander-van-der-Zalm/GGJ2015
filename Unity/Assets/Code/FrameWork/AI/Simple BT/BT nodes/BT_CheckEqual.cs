using UnityEngine;
using System.Collections;

public class BT_CheckEqualBBParameter : BT_Condition
{
    private const string P1Str = "Parameter1ToCompare";
    private const string IsObjectStr = "UseObjectToCompare";
    private const string P2Str = "Parameter2ToCompare";
    private const string ObjStr = "ObjectToCompare";

    #region Properties

    private AI_AgentParameter P1 { get { return (AI_AgentParameter)Node[P1Str]; } }

    private AI_AgentParameter P2 { get { return (AI_AgentParameter)Node[P2Str]; } }

    private object Obj1 { get { return Agent[P1]; } }

    private bool IsObject { get { return (bool)Node[IsObjectStr]; } }

    private object Obj2 
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

    #region Constructors

    public override void SetNodeParameters(BT_TreeNode node)
    {
        node[P1Str]        = new AI_AgentParameter();
        node[IsObjectStr]  = false;
        node[P2Str]        = new AI_AgentParameter();
        node[ObjStr]       = null;
        
    }

    protected override void SetDescription()
    {
        Description.Type = NodeDescription.BT_NodeType.Condition;
        Description.Name = "CheckEqual";
        Description.Description = "Succeeds if objects are equal and fails otherwise";
    }

    #endregion

    #region Get Set

    public static BT_TreeNode GetTreeNode(AI_AgentParameter accesparam1, object equalObject)
    {
        BT_TreeNode node = BT_TreeNode.CreateNode(ScriptableObjectHelper.Create<BT_CheckEqualBBParameter>());
        return SetParameters(node, accesparam1, equalObject);
    }

    public static BT_TreeNode SetParameters(BT_TreeNode node, AI_AgentParameter accesparam1, object equalObject)
    {
        node.CheckAndSetClass<BT_CheckEqualBBParameter>();
        node[P1Str]         = accesparam1;
        node[ObjStr]        = equalObject;
        node[IsObjectStr]   = true;
        return node;
    }

    public static BT_TreeNode GetTreeNode(AI_AgentParameter accesparam1, AI_AgentParameter accesparam2)
    {
        BT_TreeNode node = BT_TreeNode.CreateNode(new BT_CheckEqualBBParameter());
        return SetParameters(node, accesparam1, accesparam2);
    }

    public static BT_TreeNode SetParameters(BT_TreeNode node, AI_AgentParameter accesparam1, AI_AgentParameter accesparam2)
    {
        node.CheckAndSetClass<BT_CheckEqualBBParameter>();
        node[P1Str]     = accesparam1;
        node[P2Str]     = accesparam2;
        node[IsObjectStr]  = false;
        return node;
    }

    #endregion

    protected override Status update()
    {      
        return Obj1.Equals(Obj2) ? Status.Succes : Status.Failed;
    }

   
}
