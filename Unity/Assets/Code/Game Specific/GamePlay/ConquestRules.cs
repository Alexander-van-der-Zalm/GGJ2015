using UnityEngine;
using System.Collections;
using System.Linq;

[System.Serializable]
public class ConquestRules
{
    #region Enums & Classes

    public enum ConnectedRequirement
    {
        None,
        ConnectedSameTeam,
        ConnectedTeamBase
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

    public class OwnerShipInfo
    {

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
            case ConnectedRequirement.ConnectedSameTeam:
                return CanConquerConnectedSameTeam(unit);
            case ConnectedRequirement.None:
                return true;
            case ConnectedRequirement.ConnectedTeamBase:
                throw new System.NotImplementedException();
        }
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Returns true if any of the units current block neighbor is of the same team
    /// </summary>
    private bool CanConquerConnectedSameTeam(BasicUnit unit)
    {
        return unit.CurrentFace.Block.Neighbors.Where(b => b.TeamID == unit.Team).Any();
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

    /// <summary>
    /// Starts FaceCaptureCoRoutine
    /// </summary>
    /// <param name="unit"></param>
    /// <param name="face"></param>
    private void FaceCapture(BasicUnit unit, BlockFace face)
    {
        unit.StartCoroutine(FaceCaptureCR(unit,face));
    }

    private IEnumerator FaceCaptureCR(BasicUnit unit, BlockFace face)
    {
        PhotonPlayer player = PhotonNetwork.player;
        //player.

        yield return null;
    }

    [RPC]
    public void StartFaceCaptureRPC(int unitID, int BlockID, int faceID)
    {

    }

    #endregion
}
