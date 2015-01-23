using UnityEngine;
using System.Linq;
using System.Collections;

public class BT_Inverter : BT_Decorator 
{
    protected override void SetDescription()
    {
        Description.Name = "Inverter";
        Description.Description = "Fires off child and then returns the inverted status (failed if success etc.)";
        Description.Type = NodeDescription.BT_NodeType.Decorator;
    }
    
    protected override Status update()
    {
        if (!Node.HasChildren)
            return Status.Invalid;

        return invert(Node.Children.First().Tick(Agent));
    }

    private Status invert(Status status)
    {
        switch(status)
        {
            case Status.Failed:
                return Status.Succes;
            case Status.Succes:
                return Status.Failed;
            case Status.Running:
                return Status.Running;
            default:
                return Status.Invalid;
        }
    }


}
