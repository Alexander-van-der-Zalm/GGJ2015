using UnityEngine;
using System.Linq;
using System.Collections;
using Framework.Collections;

public class BT_QueueHelper
{
    public static bool HasIQueue(object obj)
    {
        if (obj == null)
            return false;

        bool has = obj.GetType().GetInterfaces().Contains(typeof(IQueue));
        if(!has)
            Debug.Log("BT_QueuePush has invalid object behind paramters, needs: " + typeof(Queue<>));
        return has;
    }
}
