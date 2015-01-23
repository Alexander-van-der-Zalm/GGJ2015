using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

[System.Serializable]
public class BT_Selector : BT_Composite
{
    #region Constructor

    protected override void SetDescription()
    {
        Description.Type = NodeDescription.BT_NodeType.Selector;
        Description.Name = "Selector";
        Description.Description = "Keeps going if children are failing, early exits if: succes, invalid ör running";
    }

    #endregion

    #region Update

    protected override Status update()
    {
        List<BT_TreeNode> nodes = Node.Children;

        for (int i = 0; i < nodes.Count; i++)
        {
            Status s = nodes[i].Tick(Agent);

            // Continue on failed
            // Return succes, invalid and running
            if (s != Status.Failed)
                return s;
        }

        // Return failed if all the nodes are hit
        return Status.Failed;
    }

    #endregion
}
