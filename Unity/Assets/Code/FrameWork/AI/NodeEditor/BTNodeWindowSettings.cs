using UnityEngine;
using System.Collections;

public class BTNodeWindowSettings 
{
    public static Color StandardNodeColor { get { return new Color(.8f, 0.9f, 0.9f, 1.0f); } }
    public static Color SelectedNodeColor { get { return new Color(.9f, 0.9f, 0.2f, 1.0f); } }
    public static Color InvalidNodeColor { get { return new Color(.4f, 0.4f, 0.4f, 1.0f); } }
    public static Color RunningNodeColor { get { return new Color(.0f, 0.3f, 0.9f, 1.0f); } }
    public static Color FailedNodeColor { get { return new Color(.9f, 0.2f, 0.2f, 1.0f); } }
    public static Color SuccesNodeColor { get { return new Color(.3f, 0.9f, 0.5f, 1.0f); } }
}
