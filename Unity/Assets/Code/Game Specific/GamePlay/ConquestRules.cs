using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

#region OwnerShipInfo Class

[System.Serializable]
public class OwnershipInfo
{
    #region Fields

    public int ContestantTeamID;
    public int TeamID;

    //public bool InterruptCapture = false;

    public Coroutine CaptureCoRoutine;

    [Range(0.0f, 1.0f),SerializeField]
    private float progress;

    #endregion

    #region Properties

    public bool CaptureInProgress { get { return CaptureCoRoutine != null; } }

    public float Progress { get { return progress; } set { progress = Mathf.Clamp(value, 0, 1); } }

    #endregion

    #region Reset

    public void Reset()
    {
        ContestantTeamID = 0;
        TeamID = 0;
        Progress = 0;
        CaptureCoRoutine = null;
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

    #endregion
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
        NodeContestedSameTeam,
        NodeConnectedTeamBase,
        NodeContestedTeamBase,
        FaceConnectedSameTeam,
        FaceContestedSameTeam,
        FaceConnectedTeamBase,
        FaceContestedTeamBase
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
    public bool CanQonquer(BlockFace face, int teamID)
    {
        switch (ConnectionRequirement)
        {
            case ConnectedRequirement.None:
                return true;

            case ConnectedRequirement.NodeConnectedSameTeam:
                return CanConquerBlockConnectedSameTeam(face.Block, teamID);

            case ConnectedRequirement.NodeContestedSameTeam:
                return CanConquerBlockContestedSameTeam(face.Block, teamID);

            case ConnectedRequirement.NodeConnectedTeamBase:
                throw new System.NotImplementedException();

            case ConnectedRequirement.FaceConnectedSameTeam:
                return CanConquerFaceConnectedSameTeam(face, teamID);

            case ConnectedRequirement.FaceContestedSameTeam:
                return CanConquerFaceContestedSameTeam(face, teamID);

            case ConnectedRequirement.FaceConnectedTeamBase:
                throw new System.NotImplementedException();
        }
        throw new System.NotImplementedException();
    }

    

    

    /// <summary>
    /// Returns true if any of the units current block neighbor is of the same team
    /// </summary>
    private bool CanConquerBlockConnectedSameTeam(Block block, int teamID)
    {
        return block.Neighbors.Where(b => b.OwnerInfo.TeamID == teamID).Any();
    }

    private bool CanConquerBlockContestedSameTeam(Block block, int teamID)
    {
        return block.Neighbors.Where(b => b.OwnerInfo.TeamID == teamID || b.OwnerInfo.ContestantTeamID == teamID).Any();
    }

    /// <summary>
    /// Returns true if any of the units current face neighbor is of the same team
    /// </summary>
    private bool CanConquerFaceConnectedSameTeam(BlockFace face, int teamID)
    {
        return face.Neighbors.Where(f => f.OwnerInfo.TeamID == teamID).Any();
    }

    private bool CanConquerFaceContestedSameTeam(BlockFace face, int teamID)
    {
        return face.Neighbors.Where(f => f.OwnerInfo.TeamID == teamID || f.OwnerInfo.ContestantTeamID == teamID).Any();
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
        if (EarlyRejectCapture(unit, OnNoMovementCaptureMethod))
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
        if (!PhotonNetwork.offlineMode && unit.TeamID != GameManagement.Unit.team)
            return true;

        // Check if already the owner
        // Check if already contesting
        if (EarlyReject(unit, OnFaceCaptureMethod))
            return true;

        // Check if the unit can capture based on the current rules
        if (!CanQonquer(unit.CurrentFace, unit.TeamID))
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
        // Returns true if already the owner
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
        // Check if already the owner
        if(unit.TeamID == unit.CurrentFace.Block.TeamID)
            return true;

        Debug.Log("Progress: " + unit.CurrentFace.Block.OwnerInfo.CaptureInProgress.ToString() + " ids: " + unit.TeamID + " | " + unit.CurrentFace.Block.OwnerInfo.ContestantTeamID);

        return unit.CurrentFace.Block.OwnerInfo.CaptureInProgress && unit.TeamID == unit.CurrentFace.Block.OwnerInfo.ContestantTeamID;
    }

    private bool EarlyFaceCaptureReject(BasicUnit unit)
    {
        if (unit.TeamID == unit.CurrentFace.TeamID)
            return true;

        return unit.CurrentFace.OwnerInfo.CaptureInProgress && unit.TeamID == unit.CurrentFace.OwnerInfo.ContestantTeamID;
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
    private void StartCaptureRPC(int unitID, int captureMethod)
    {
        BasicUnit unit = UnitManager.Get(unitID);
        CaptureMethod method = (CaptureMethod)captureMethod;
        
        Debug.Log(unit.name + " " + method.ToString());

        // Send RPC's if owner of the unit
        switch(method)
        {
            case CaptureMethod.CaptureNode:// Node
                BlockCapture(unit, unit.CurrentFace.Block);
                break;

            case CaptureMethod.CaptureFace:// Face
                FaceCapture(unit, unit.CurrentFace, FaceCaptureTimers.CaptureTime);
                break;
        }
    }

    #region Face Capture 

    [RPC]
    private void StartFaceCaptureRPC(int unitID, int BlockID, int faceID)
    {
        BasicUnit unit = UnitManager.Get(unitID);
        BlockFace face = BlockManager.GetFace(BlockID, faceID);

        Debug.Log("CaptureRPC");

        FaceCapture(unit, face, FaceCaptureTimers.CaptureTime);
    }

    /// <summary>
    /// Starts FaceCaptureCoRoutine
    /// </summary>
    private void FaceCapture(BasicUnit unit, BlockFace face, float capTime, bool overrideNodeCapture = false)
    {
        if (face.OwnerInfo.CaptureInProgress)
        {
            if(face.Block.OwnerInfo.CaptureInProgress && !overrideNodeCapture)
            {
                Debug.Log("Cancel face capture/cannot override node capture");
                return;
            }
            
            face.StopCoroutine(face.OwnerInfo.CaptureCoRoutine);
            Debug.Log("Stopping previous capture");
        }

        face.OwnerInfo.CaptureCoRoutine = face.StartCoroutine(FaceCaptureCR(unit, face, capTime));
    }
    
    private IEnumerator FaceCaptureCR(BasicUnit unit, BlockFace face, float capTime)
    {
        PhotonPlayer player = PhotonNetwork.player;

        // If it was already in progress of being contested
        if(face.OwnerInfo.Progress > 0 && face.OwnerInfo.ContestantTeamID != unit.TeamID)
        {
            // Can be changed to save unit to info and/or coroutine to info
            Debug.Log("Already contested - do nothing for now");
            //face.OwnerInfo.InterruptCapture = true;
            yield return null;
        }

        //Debug.Log("Start capping");

        // Start capping
        face.OwnerInfo.ContestantTeamID = unit.TeamID;
        face.OwnerInfo.Progress = 0;
        // TODO animation swap

        // Still needed???
        unit.Capping = true;

        //float startTime = Time.realtimeSinceStartup;
        float captureStep = 1 / capTime;

        // Start progress
        while (face.OwnerInfo.Progress < 1)
        {
            // Check captureCondition still true
            if(!CanQonquer(face,unit.TeamID)) // Change to stopCoroutine
            {
                Debug.Log("Capture criteria not valid anymore");
                face.OwnerInfo.CaptureCoRoutine = null;
                //face.OwnerInfo.InterruptCapture = false;
                yield break;
            }

            // continue capture
            face.OwnerInfo.Progress += Time.deltaTime * captureStep;

            //float capTime = Time.realtimeSinceStartup - startTime;
            //Debug.Log(face.OwnerInfo.Progress + " " + capTime + " max: " + FaceCaptureTimers.CaptureTime);

            // Update vertex color
            face.ChangeContestedTeamColor();
            yield return null;
        }

        // Captured
        //Debug.Log("Captured");
        // Reset info
        face.OwnerInfo.Reset();

        // Set new info
        face.OwnerInfo.TeamID = unit.TeamID;
        face.SetTeamColor();
        
    }

    #endregion

    #region Block/Node Capture

    private void BlockCapture(BasicUnit unit, Block block)
    {
        if (block.OwnerInfo.CaptureInProgress && block.OwnerInfo.ContestantTeamID != unit.TeamID)
        {
            block.StopCoroutine(block.OwnerInfo.CaptureCoRoutine);
            Debug.Log("Block: Stopping previous capture");
        }
        
        block.OwnerInfo.CaptureCoRoutine = block.StartCoroutine(BlockCaptureCR(unit, block));
    }

    private IEnumerator BlockCaptureCR(BasicUnit unit, Block block)
    {
        List<BlockFace> faces = block.Faces;
        //// Sort faces
        //faces = faces.OrderBy(f => f.DistanceToPosUnquared(unit.CurrentFace.transform.position)).ToList();

        // Do administration of capping
        if (block.OwnerInfo.Progress > 0 && block.OwnerInfo.ContestantTeamID != unit.TeamID)
        {
            // Can be changed to save unit to info and/or coroutine to info
            Debug.Log("Already contested - do nothing for now");
            //face.OwnerInfo.InterruptCapture = true;
            yield return null;
        }

        // Start capping
        block.OwnerInfo.ContestantTeamID = unit.TeamID;
        block.OwnerInfo.Progress = 0; // ??

        // Capture all faces at once
        for (int i = 0; i < faces.Count; i++ )
        {
            FaceCapture(unit, faces[i], NodeCaptureTimers.CaptureTime, true);
        }

        // Maintain progress
        float captureStep = 1 / NodeCaptureTimers.CaptureTime;
        BlockFace face = unit.CurrentFace;
        while (block.OwnerInfo.Progress < 1)
        {
            // Check captureCondition still true
            if (!CanQonquer(face, unit.TeamID)) // Change to stopCoroutine
            {
                Debug.Log("Capture criteria not valid anymore");
                block.OwnerInfo.CaptureCoRoutine = null;

                // Stop all face CR's

                yield break;
            }

            // continue maintaining progres
            block.OwnerInfo.Progress += Time.deltaTime * captureStep;

            yield return null;
        }


        // Finished capturing
        OnFinishedCaptureBlock(unit, block);

        yield return null;
    }

    #endregion

    #endregion

    private void OnCaptureFace()
    {
        // Check wincondition
    }

    private void OnFinishedCaptureBlock(BasicUnit unit, Block block)
    {
        Debug.Log("Captured Block");
        block.TeamID = unit.TeamID;


        if (block.creature != null)
        {
            block.creature.TeamID = unit.TeamID;
            block.RespawnUnit();
        }
         
        // Check wincondition

    }
}
