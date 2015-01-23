using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

[System.Serializable,ExecuteInEditMode,RequireComponent(typeof(AI_BlackboardComponent))]
public class BT_UINode : MonoBehaviour
{
    public BT_UINodeInfo NodeInfo;

    public AI_Blackboard BB { get { return bb.Blackboard; } set { bb.Blackboard = value; } }

    [HideInInspector]
    public AI_BlackboardComponent bb;

    private RectTransform rtr;
   
    void Start()
    {
        Init();
    }

    public void Init()
    {
        if (bb == null)
            bb = GetComponent<AI_BlackboardComponent>();

        BB.Name = "Public Node parameters";
        NodeInfo.UINode = this;
    }

    void Update()
    {
        if (rtr == null)
            rtr = GetComponent<RectTransform>();

        NodeInfo.Position = rtr.position;
    }

    internal void ChangeNode(BT_UINodeInfo node)
    {
        NodeInfo = node;
        NodeInfo.UINode = this;
        if(NodeInfo.Position != null)
            rtr.position = NodeInfo.Position;
        BB.ChangeValues(node.TreeNode.ParametersBB);
    }

    public void ChangeBehavior(BT_BBParameters behavior)
    {
        NodeInfo.SetBehavior(behavior);
        BB = NodeInfo.TreeNode.ParametersBB;//BB stuff
    }

    public void SetParent()
    {

    }

    public void AddChild()
    {

    }
}
