using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class MovementRules
{
    #region Enums

    public enum MoveDuringCapture
    {
        CanMove,
        NoMovement
    }

    public enum MovementMethodEnum
    {
        OneFace
    }

    #endregion

    #region Fields

    public MovementMethodEnum MovementMethod;
    public float SpeedInFacePerSecond = 1.0f;

    public MoveDuringCapture CaptureFace,CaptureNode;
    

    #endregion

    #region MovementClick

    public void MovementClick(BlockFace face, BasicUnit unit)
    {
        if (!CanMove(face, unit))
            return;

        // Pass along the destination, the origin and the unitID
        int originFaceID = (unit.CurrentFace != null) ? unit.CurrentFace.ID : face.ID;
        int originBlockID = (unit.CurrentFace != null) ? unit.CurrentFace.Block.ID : face.Block.ID;

        SendMoveOrder(face.ID, face.Block.ID, unit.ID, originFaceID, originBlockID);

        //UnitManager.LocalMoveOrder(new UnitManager.FaceBlockID() { FaceID = ID, BlockID = Block.ID }, Selectionmanager.Instance.SelectedUnit.ID, new UnitManager.FaceBlockID());
        //Selectionmanager.Instance.SelectedUnit.MoveUnit(Block.ID, ID);
    }

    #endregion

    public bool CanMove(BlockFace dest, BasicUnit unit)
    {
        // Check if neighboring
        if (!dest.Neighbors.Where(n => n == unit.CurrentFace).Any())
        {
            Debug.Log("Illegal Move");
            return false;
        }

        if (dest.Block != unit.CurrentFace.Block && dest.Block.Faces.Where(f => f.HasUnit).Any())
        {
            Debug.Log("Blocked");
            return false;
        }

        return true;
    }

    public void SendMoveOrder(int destFaceID, int destBlockID, int unitID, int originFaceID, int originBlockID)
    {
        if (PhotonNetwork.offlineMode)
        {
            MoveUnit(destFaceID, destBlockID, unitID, originFaceID, originBlockID);
        }
        else
        {
            // Do the RPC call
            GameManagement.Photon.RPC("MoveUnit", PhotonTargets.All, destFaceID, destBlockID, unitID, originFaceID, originBlockID);
        }
    }

    [RPC]
    public void MoveUnit(int destFaceID, int destBlockID, int unitID, int originFaceID, int originBlockID)
    {
        UnitManager.FaceBlockID destination = new UnitManager.FaceBlockID() { FaceID = destFaceID, BlockID = destBlockID };
        UnitManager.FaceBlockID origin = new UnitManager.FaceBlockID() { FaceID = originFaceID, BlockID = originBlockID };

        BasicUnit unit = GameManagement.Unit.GetUnit(unitID);
        BlockFace dest = GameManagement.Block.getFace(destination);

        // Check if the move is legal
        if (!CanMove(dest, unit))
            return;

        Debug.Log("Move 2.0");

        unit.MoveUnit(destination.BlockID, destination.FaceID);

        // Capture Block
        GameManagement.Rules.ConquestRules.OnFace(unit);
        //ColorBlock(destination.BlockID, destination.FaceID, unit);
    }

    


    public static void HighLightPossibleFaces(List<BlockFace> Faces)
    {
        throw new System.NotImplementedException();
    }
}
