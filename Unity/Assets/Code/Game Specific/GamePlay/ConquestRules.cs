using UnityEngine;
using System.Collections;
using System.Linq;

[System.Serializable]
public class ConquestRules
{
    public enum ConnectedRequirement
    {
        None,
        ConnectedSameTeam,
        ConnectedTeamBase
    }

    public enum CaptureMethod
    {
        None,
        CaptureNode,
        CaptureFace,
    }

    public ConnectedRequirement ConnectionRequirement;
    public CaptureMethod OnFace;
    public CaptureMethod OnNoMovement;

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


}
