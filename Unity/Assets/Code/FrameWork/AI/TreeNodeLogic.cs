using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Can only have one parent,
/// Childrens parent all need to be the parent
/// </summary>
/// <typeparam name="T"></typeparam>
[System.Serializable]
public class TreeNodeLogic<T>: EasyScriptableObject<T> where T : TreeNodeLogic<T>
{
    [SerializeField]
    protected int id;
    
    [SerializeField]
    protected T parent;

    [SerializeField]
    protected List<T> children;

    [SerializeField]
    protected int MaxChildren;

    public T Parent { get { return parent; } protected set { parent = value; } }
    public List<T> Children { get { return children; } protected set { children = value; } }

    public virtual int ID { get { return id; } set { id = value; } }

    public TreeNodeLogic<T> This { get { return this; } }

    public virtual void Init()
    {
        Children = new List<T>();
        id = -1337;
    }


    public void AddChildren(params T[] children)
    {
        foreach (T newChild in children)
        {
            // Check if the child already has a parent
            if (newChild.Parent != null)
            {
                // Remove new child from old parent
                newChild.Parent.RemoveChildren(newChild);
            }

            // Check if the parent is this node's parent
            if (Parent == newChild)
            {
                // Remove this child from the ex parent
                Parent.RemoveChildren((T)this);
            }

            // Set new parent
            newChild.Parent = (T)this;
        }

        Children.AddRange(children.ToList());
    }

    public void RemoveChildren(params T[] children)
    {
        // Remove children
        for (int i = 0; i < children.Length; i++)
        {
            children[i].Parent = null;
            Children.Remove(children[i]);
        }
    }

    public void DisconnectAll()
    {
        // Remove self from parent as child
        if(Parent != null)
            Parent.RemoveChildren((T)this);

        // Remove self as parent from children
        for(int i = 0; i < Children.Count; i++)
        {
            Children[i].Parent = null;
        }

        // Remove children
        RemoveChildren(Children.ToArray());
    }

    public void DisconnectFromParent()
    {
        if (Parent != null)
            Parent.RemoveChildren((T)this);
        Parent = null;
    }
    public bool IsDescendantOf(T Ancestor)
    {
        T curAncestor = Parent;
        while (curAncestor != null)
        {
            if (curAncestor == Ancestor)
                return true;
            curAncestor = curAncestor.Parent;
        }
        return false;
    }

}
