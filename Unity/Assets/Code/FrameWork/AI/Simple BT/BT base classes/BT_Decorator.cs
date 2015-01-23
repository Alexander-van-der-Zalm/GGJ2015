using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BT_Decorator : BT_HasChild 
{
    #region Constructor

    public BT_Decorator()
    {
        Description.Type = NodeDescription.BT_NodeType.Decorator;
    }

    #endregion
}
