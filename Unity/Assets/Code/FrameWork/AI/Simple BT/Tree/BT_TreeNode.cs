using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Status = BT_Behavior.Status;
using System;

[System.Serializable]
public class BT_TreeNode : TreeNodeLogic<BT_TreeNode>
{
    #region Fields

    [SerializeField,HideInInspector]
    private BT_BBParameters behavior;

    [SerializeField,ReadOnly]
    private string behaviorType;

    [SerializeField]
    private AI_Blackboard parametersBB;

    [SerializeField]
    private BT_Tree tree;

    [SerializeField]
    public string Name;

    [SerializeField]
    public string Description;

    #endregion

    #region Properties
    
    public BT_BBParameters Behavior
    {
        get 
        {
            if (behavior == null)
            {
                if (behaviorType == string.Empty)
                {
                    Debug.LogError("Behavior is null and cannot reacreate from type");
                    return null;
                }
                behavior = (BT_BBParameters)Activator.CreateInstance(Type.GetType(behaviorType));
            }
            return behavior; 
        }
        set { behaviorType = value.GetType().ToString(); SetParameters(value); }
    }

    public object this[string name]
    {
        get{return ParametersBB[name];}
        set{ParametersBB[name]=value;}
    }

    public AI_Blackboard ParametersBB { get { return parametersBB; } private set { parametersBB = value; } }

    public BT_Tree Tree { get { return tree; } private set { tree = value; } }

    public bool HasChildren { get { return Children.Count > 0; } }

    public bool IsRoot { get { return Parent == null && Tree.Root == this; } }

    public int ChildIndex { get { return Parent == null ? -1 : Parent.Children.IndexOf(this); } }

    public override int ID
    {
        get
        {
            return base.ID;
        }
        set
        {
            base.ID = value;
            SetNames();
        }
    }

    
    #endregion

    #region Constructor

    public override void Init(HideFlags newHideFlag = HideFlags.None)
    {
        base.Init(newHideFlag);

        // Default values
        if (ParametersBB == null)
            ParametersBB = AI_Blackboard.Create();
        ID = -1337;
        Parent = null;
        Children = new List<BT_TreeNode>();
        behavior = null;
        Name = string.Empty;
        Description = string.Empty;
    }

    public static BT_TreeNode CreateNode(BT_BBParameters behavior,int ID, AI_Blackboard bb = null, BT_Tree treeObj = null )
    {
        // Create node and set values
        BT_TreeNode node = Create();

        if (bb != null)
            node.ParametersBB = bb;

        node.Behavior = behavior;
        node.ID = ID;
        node.Tree = treeObj;

        if(behavior != null)
        {
            //node.Name = node.behavior.Description.Name;
            node.Description = node.Behavior.Description.Description;
        }
            
        
        // Add object to asset
        if(treeObj != null)
        {
            //ScriptableObjectHelper.AddObjectToAsset(treeObj, node);
            node.AddObjectToAsset(treeObj);
        }
            
            
        return node;
    }

    private void SetNames()
    {
        name = ID + " | TREENODE | " + (behavior!=null?behavior.Description.Name:"");
        
        if (ParametersBB != null)
            ParametersBB.name = ID + " | PARAMETERS | " + (behavior != null ? behavior.Description.Name : "");
            
    }


    public static BT_TreeNode CreateNode(BT_BBParameters behavior)//, string filepath = "")
    {
        BT_TreeNode node = Create();
        node.Behavior = behavior;

        //if (filepath != string.Empty)
        //{

        //}
            //node.AddObjectToAsset(filepath);

        return node;
    }
    #endregion

    #region Set Parameters

    private void SetParameters(BT_BBParameters behavior)
    {
        // Set behavior
        this.behavior = behavior;

        //Debug.Log("SetParameters");

        // Reset blackboard
        ParametersBB.Clear();

        SetBehaviorParameters();
    }

    public void SetBehaviorParameters()
    {
        // Call the SetnodeParameters virtual method
        // Sets the blackboard with default parameters
        behavior.SetNodeParameters(this);
    }
    

    /// <summary>
    /// Returns false if the class already exists and true if it had to be created
    /// </summary>
    public bool CheckAndSetClass<T>() where T: BT_BBParameters
    {
        if (Behavior.GetType() == typeof(T))
            return false;

        // Not the same type so reset the behavior
        T newBehavior = (T)Activator.CreateInstance(typeof(T));
        Behavior = newBehavior;
        return true;
    }

    #endregion

    public void SortChildrenByIDS(List<int> id)
    {
        List<BT_TreeNode> newChildren = new List<BT_TreeNode>();
        for(int i = 0; i < Children.Count; i++)
        {
            newChildren.Add(Children.Where(c => c.ID == id[i]).First());

        }
        Children = newChildren;
    }

    public Status Tick(AI_Agent agent)
    {
        return Behavior.Tick(agent, ID);
    }
}