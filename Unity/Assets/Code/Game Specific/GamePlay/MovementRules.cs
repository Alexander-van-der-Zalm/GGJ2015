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

    public static void MovementClick(BlockFace face, BasicUnit unit)
    {
        if (!unit.Capping)
        {
            // Pass along the destination, the origin and the unitID
            int originFaceID = (unit.CurrentFace != null) ? unit.CurrentFace.ID : face.ID;
            int originBlockID = (unit.CurrentFace != null) ? unit.CurrentFace.Block.ID : face.Block.ID;

            UnitManager.LocalMoveOrder(face.ID, face.Block.ID, unit.ID, originFaceID, originBlockID);
        }

        //UnitManager.LocalMoveOrder(new UnitManager.FaceBlockID() { FaceID = ID, BlockID = Block.ID }, Selectionmanager.Instance.SelectedUnit.ID, new UnitManager.FaceBlockID());
        //Selectionmanager.Instance.SelectedUnit.MoveUnit(Block.ID, ID);
    }

    public static bool CanMove()
    {
        throw new System.NotImplementedException();
    }


    public static void HighLightPossibleFaces(List<BlockFace> Faces)
    {
        throw new System.NotImplementedException();
    }
}
