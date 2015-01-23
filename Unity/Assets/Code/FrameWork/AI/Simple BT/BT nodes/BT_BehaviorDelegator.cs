using UnityEngine;
using System.Collections;

public class BT_BehaviorDelegator : BT_BBParameters 
{
    public delegate Status UpdateDelegate(AI_Agent agent, BT_TreeNode node);
    public delegate void InitDelegate(AI_Agent agent, BT_TreeNode node);
    public delegate void EnterDelegate(AI_Agent agent, BT_TreeNode node);
    public delegate void ExitDelegate(AI_Agent agent, BT_TreeNode node, Status status);
    public delegate void TerminateDelegate(AI_Agent agent, BT_TreeNode node, Status status);

    
    private InitDelegate initDel;
    private EnterDelegate enterDel;
    private UpdateDelegate updateDel;
    private ExitDelegate exitDel;
    private TerminateDelegate terminateDel;
    

    public BT_BehaviorDelegator(NodeDescription.BT_NodeType type, UpdateDelegate onUpdate, InitDelegate onInit = null, EnterDelegate onEnter = null, ExitDelegate onExit = null, TerminateDelegate onTerm = null)
    {
        Description.Type = type;
        
        initDel = onInit;
        enterDel = onEnter;
        updateDel = onUpdate;
        exitDel = onExit;
        terminateDel = onTerm;
    }

    protected override Status update()
    {
        return updateDel(Agent, Node);
    }

    protected override void onInitialize()
    {
        if(initDel!=null)
            initDel(Agent, Node);
    }

    protected override void onEnter()
    {
        if (enterDel != null)
            enterDel(Agent, Node);
    }

    protected override void onExit(BT_Behavior.Status status)
    {
        if (exitDel != null)
            exitDel(Agent, Node, status);
    }
    protected override void onTerminate(Status status)
    {
        if(terminateDel!=null)
            terminateDel(Agent, Node, status);
    }
}
