using UnityEngine;
using System.Linq;
using System.Collections;

public class BT_AlwayFail : BT_Decorator 
{
    protected override void SetDescription()
    {
        Description.Name = "AlwayFail";
        Description.Description = "Ticks child and then always returns failed";
        Description.Type = NodeDescription.BT_NodeType.Decorator;
    }

    protected override BT_Behavior.Status update()
    {
        // Fire off first child in a cannon (not really)
        if(Node.HasChildren)
            Node.Children.First().Tick(Agent);

        return Status.Failed;
    }
}
