using UnityEngine;
using System.Collections;

[System.Serializable]
public class BT_HasChild : BT_BBParameters
{
     protected override void onEnter()
    {
        if (Agent != null)
            Agent.TreeInfo.Depth++;
    }

     protected override void onExit(BT_Behavior.Status status)
     {
         if (Agent != null)
             Agent.TreeInfo.Depth--;
     }
}
