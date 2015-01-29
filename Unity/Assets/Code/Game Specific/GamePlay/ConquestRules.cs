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
    public CaptureMethod OnFace;
    public CaptureMethod OnNoMovement;

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

    /// <summary>
    /// Starts FaceCaptureCoRoutine
    /// </summary>
    /// <param name="unit"></param>
    /// <param name="face"></param>
    private void FaceCapture(BasicUnit unit, BlockFace face)
    {
        unit.StartCoroutine(FaceCaptureCR(unit,face));
    }

    [RPC]
    public void StartFaceCaptureRPC(int unitID, int BlockID, int faceID)
    {
        BasicUnit unit = UnitManager.Get(unitID);
        BlockFace face = BlockManager.GetFace(BlockID, faceID);

        unit.StartCoroutine(FaceCaptureCR(unit, face));
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

        float captureStep = 1 / FaceCaptureTimers.CaptureTime;

        // Start progress
        while (face.OwnerInfo.Progress < 1)
        {
            if(face.OwnerInfo.InterruptCapture)
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
        face.OwnerInfo.Reset();
        face.OwnerInfo.TeamID = unit.TeamID;
        face.ChangeTeamColor();
    }

    #endregion

    #region Block/Node Capture

    #endregion

    #endregion
}
