//using UnityEngine;
//using System.Collections;
//using System.Reflection;
//using System;

//public class BT_GetTreeNode
//{
//    public static BT_TreeNode GetTreeNode<T>() where T : BT_BBParameters
//    {
//        // Find current type
//        Type type = typeof(T);
//        Debug.Log("GetTreeNode " + type.ToString());
//        // Create the behavior through reflection
//        object obj = (BT_Behavior)Activator.CreateInstance(type);

//        return new BT_TreeNode((T)obj);
//    }
//}
