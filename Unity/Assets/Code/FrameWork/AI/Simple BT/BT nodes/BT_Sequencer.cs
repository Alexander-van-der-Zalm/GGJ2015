using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class BT_Sequencer : BT_Composite
{
    #region Constructor



    public BT_Sequencer(): base()
    {
        Description.Type = NodeDescription.BT_NodeType.Sequencer;
        Description.Name = "Sequencer";
        Description.Description = "Keeps going if children are succesfull, early exits if: failed, invalid ör running";
    }

    #endregion 

    protected override Status update()
    {
        List<BT_TreeNode> nodes = Node.Children;

        for (int i = 0; i < nodes.Count; i++)
        {
            Status s = nodes[i].Tick(Agent);

            // Continue on succes
            // Return failed, invalid and running
            if (s != Status.Succes)
                return s;
        }

        // Return succes if all the nodes are hit
        return Status.Succes;
    }
}
