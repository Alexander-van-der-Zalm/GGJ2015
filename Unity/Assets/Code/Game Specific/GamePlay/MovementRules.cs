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

    public bool CanMove(BlockFace face, BasicUnit unit)
    {
        throw new System.NotImplementedException();
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
        FaceBlockID destination = new FaceBlockID() { FaceID = destFaceID, BlockID = destBlockID };
        FaceBlockID origin = new FaceBlockID() { FaceID = originFaceID, BlockID = originBlockID };

        BasicUnit unit = get(unitID);

        // Check if the move is legal
        BlockFace dest = bm.getFace(destination);
        BlockFace orig = bm.getFace(origin);
        //Debug.Log("Move");

        // Check if neighboring
        if (!dest.Neighbors.Where(n => n == orig).Any())
        {
            Debug.Log("Illegal Move");
            return;
        }

        if (dest.Block != orig.Block && dest.Block.Faces.Where(f => f.HasUnit).Any())
        {
            Debug.Log("Blocked");
            return;
        }

        unit.MoveUnit(destination.BlockID, destination.FaceID);

        ColorBlock(destination.BlockID, destination.FaceID, unit);
    }

    


    public static void HighLightPossibleFaces(List<BlockFace> Faces)
    {
        throw new System.NotImplementedException();
    }
}
