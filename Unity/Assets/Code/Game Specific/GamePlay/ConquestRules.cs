using UnityEngine;
using System.Collections;
using System.Linq;

#region OwnerShipInfo Class

[System.Serializable]
public class OwnershipInfo
{
    public int ContestantTeamID;
    public int TeamID;

    public bool InterruptCapture = false;

    [Range(0.0f, 1.0f),SerializeField]
    private float progress;

    public float Progress { get { return progress; } set { progress = Mathf.Clamp(value, 0, 1); } }

    public void Reset()
    {
        ContestantTeamID = -1;
        TeamID = -1;
        Progress = 0;
    }

    public static void ResetAll(Block block)
    {
        // Reset Block
        block.OwnerInfo.Reset();

        // Reset its faces
        for (int i = 0; i < block.Faces.Count; i++)
        {
            block.Faces[i].OwnerInfo.Reset();
        }
    }

    public static void SetAllOwnerID(Block block, int id)
    {
        // Set block
        block.OwnerInfo.TeamID = id;

        for (int i = 0; i < block.Faces.Count; i++)
        {
            block.Faces[i].OwnerInfo.TeamID = id;
        }
    }

    public static void ResetAndSetAll(Block block, int id)
    {
        OwnershipInfo.ResetAll(block);
        OwnershipInfo.SetAllOwnerID(block, id);
    }
}

#endregion

[System.Serializable]
public class ConquestRules
{
    #region Enums & Classes

    public enum ConnectedRequirement
    {
        None,
        NodeConnectedSameTeam,
        NodeConnectedTeamBase,
        FaceConnectedSameTeam,
        FaceConnectedTeamBase
    }

    public enum CaptureMethod
    {
        None = 0,
        CaptureNode = 1,
        CaptureFace = 2,
    }

    [System.Serializable]
    public class CaptureTimes
    {
        public float CaptureTime;
        public float AnimationTime;
    }

    #endregion

    #region Fields

    public ConnectedRequirement ConnectionRequirement;
    public CaptureMethod OnFaceCaptureMethod;
    public CaptureMethod OnNoMovementCaptureMethod;

    public CaptureTimes FaceCaptureTimers;
    public CaptureTimes NodeCaptureTimers;

    #endregion

    #region ConnectionRequirement

    /// <summary>
    /// Check if the unit can conquer the current face/node
    /// </summary>
    public bool CanQonquer(BasicUnit unit)
    {
        switch (ConnectionRequirement)
        {
            case ConnectedRequirement.None:
                return true;

            case ConnectedRequirement.NodeConnectedSameTeam:
                return CanConquerBlockConnectedSameTeam(unit);

            case ConnectedRequirement.NodeConnectedTeamBase:
                throw new System.NotImplementedException();

            case ConnectedRequirement.FaceConnectedSameTeam:
                return CanConquerFaceConnectedSameTeam(unit);

            case ConnectedRequirement.FaceConnectedTeamBase:
                throw new System.NotImplementedException();
        }
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Returns true if any of the units current block neighbor is of the same team
    /// </summary>
    private bool CanConquerBlockConnectedSameTeam(BasicUnit unit)
    {
        return unit.CurrentFace.Block.Neighbors.Where(b => b.OwnerInfo.TeamID == unit.TeamID).Any();
    }

    /// <summary>
    /// Returns true if any of the units current face neighbor is of the same team
    /// </summary>
    private bool CanConquerFaceConnectedSameTeam(BasicUnit unit)
    {
        return unit.CurrentFace.Neighbors.Where(f => f.OwnerInfo.TeamID == unit.TeamID).Any();
    }

    #endregion

    #region Capture Hookups

    /// <summary>
    /// Sends out an RPC network call to all the 
    /// </summary>
    public void OnFace(BasicUnit unit)
    {
        // Check if allowed, possible
        if (EarlyRejectCapture(unit, OnFaceCaptureMethod))
            return;

        // Do the RPC call
        if(PhotonNetwork.offlineMode)
        {
            StartCaptureRPC(unit.ID, (int)OnFaceCaptureMethod);
        }
        else
        {
            GameManagement.Photon.RPC("StartCaptureRPC", PhotonTargets.All, unit.ID, (int)OnFaceCaptureMethod);
        }
    }

    /// <summary>
    /// If the unit is not moving check if it can capture
    /// </summary>
    public void OnNoMovement(BasicUnit unit)
    {
        // Check if allowed, possible
        if (EarlyRejectCapture(unit, OnFaceCaptureMethod))
            return;

        // Do the RPC call
        if (PhotonNetwork.offlineMode)
        {
            StartCaptureRPC(unit.ID, (int)OnNoMovementCaptureMethod);
        }
        else
        {
            GameManagement.Photon.RPC("StartCaptureRPC", PhotonTargets.All, unit.ID, (int)OnNoMovementCaptureMethod);
        }
    }


    #region Early Reject


    private bool EarlyRejectCapture(BasicUnit unit, CaptureMethod OnFaceCaptureMethod)
    {
        // Let only the owner of the units send the RPC's
        if (unit.TeamID != PhotonNetwork.player.ID)
            return true;

        // Check if already the owner
        if (EarlyReject(unit, OnFaceCaptureMethod))
            return true;

        // Check if the unit can capture based on the current rules
        if (!CanQonquer(unit))
        {
            Debug.Log("Cannot Capture");
            return true;
        }

        return false;
    }


    /// <summary>
    /// Returns true if the capture is not necessary (already owned)
    /// </summary>
    private bool EarlyReject(BasicUnit unit, CaptureMethod captureMethod)
    {
        // Returns t
        switch (captureMethod)
        {
            case CaptureMethod.CaptureFace:
                return EarlyFaceCaptureReject(unit);
            case CaptureMethod.CaptureNode:
                return EarlyNodeCaptureReject(unit);
            case CaptureMethod.None:
                return true;
        }

        return false;
    }

    private bool EarlyNodeCaptureReject(BasicUnit unit)
    {
        return unit.TeamID == unit.CurrentFace.Block.TeamID;
    }

    private bool EarlyFaceCaptureReject(BasicUnit unit)
    {
        return unit.TeamID == unit.CurrentFace.TeamID;
    }

    #endregion


    #endregion

    #region CaptureMethods

    /// <summary>
    /// Called over network when a capture starts (Face does currentFace of unit, Node does current block face per face)
    /// </summary>
    /// <param name="unitID">uniqueID of the unit</param>
    /// <param name="captureMethod">0 = none, 1 = node, 2 = face</param>
    [RPC]
    public void StartCaptureRPC(int unitID, int captureMethod)
    {
        BasicUnit unit = UnitManager.Get(unitID);
        CaptureMethod method = (CaptureMethod)captureMethod;
        
        Debug.Log(unit.name + " " + method.ToString());

        // Send RPC's if owner of the unit
        switch(method)
        {
            case CaptureMethod.CaptureNode:// Node

                break;

            case CaptureMethod.CaptureFace:// Face
                FaceCapture(unit, unit.CurrentFace);
                break;
        }
    }

    #region Face Capture 

    [RPC]
    public void StartFaceCaptureRPC(int unitID, int BlockID, int faceID)
    {
        BasicUnit unit = UnitManager.Get(unitID);
        BlockFace face = BlockManager.GetFace(BlockID, faceID);

        FaceCapture(unit, face);
    }

    /// <summary>
    /// Starts FaceCaptureCoRoutine
    /// </summary>
    private void FaceCapture(BasicUnit unit, BlockFace face)
    {
        unit.StartCoroutine(FaceCaptureCR(unit,face));
    }
    
    private IEnumerator FaceCaptureCR(BasicUnit unit, BlockFace face)
    {
        PhotonPlayer player = PhotonNetwork.player;

        // If it was already in progress of being contested
        if(face.OwnerInfo.Progress > 0 && face.OwnerInfo.ContestantTeamID != unit.TeamID)
        {
            // Do nothing for now
            Debug.Log("Already contested - ignoring edge case for now");
        }

        Debug.Log("Start capping");

        // Start capping
        face.OwnerInfo.ContestantTeamID = unit.TeamID;
        face.OwnerInfo.Progress = 0;
        // TODO animation swap

        unit.Capping = true;

        float captureStep = 1 / FaceCaptureTimers.CaptureTime;

        // Start progress
        while (face.OwnerInfo.Progress < 1)
        {
            if(!unit.Capping)
            {
                Debug.Log("Capture interrupted");
                face.OwnerInfo.InterruptCapture = false;
                yield break;
            }

            // continue capture
            face.OwnerInfo.Progress += captureStep;
            face.ChangeTeamColor();
            yield return null;
        }

        // Captured
        Debug.Log("Captured");
        unit.Capping = false;
        face.OwnerInfo.Reset();
        face.OwnerInfo.TeamID = unit.TeamID;
        face.ChangeTeamColor();
    }

    #endregion

    #region Block/Node Capture

    #endregion

    #endregion
}
