using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class MovementRules 
{

    public enum MoveDuringCapture
    {
        CanMove,
        NoMovement
    }

    public MoveDuringCapture CaptureFace,CaptureNode;

    public static bool CanMove()
    {
        throw new System.NotImplementedException();
    }


    public static void HighLightPossibleFaces(List<BlockFace> Faces)
    {
        throw new System.NotImplementedException();
    }
}
