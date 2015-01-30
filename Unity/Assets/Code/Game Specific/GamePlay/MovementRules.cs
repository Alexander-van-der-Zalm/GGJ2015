using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class MovementRules
{
    #region Enums

    public enum MoveDuringCapture
    {
        CanMove,
        NoMovement
    }

    #endregion

    #region Fields

    public MoveDuringCapture CaptureFace,CaptureNode;
    public float SpeedInFacePerSecond = 1.0f;

    #endregion

    public static bool CanMove()
    {
        throw new System.NotImplementedException();
    }


    public static void HighLightPossibleFaces(List<BlockFace> Faces)
    {
        throw new System.NotImplementedException();
    }
}
