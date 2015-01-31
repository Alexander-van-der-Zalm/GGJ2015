using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour
{
    #region  Field & Start

    private GameManagement management;

    void Awake()
    {
        management = GetComponent<GameManagement>();
    }

    #endregion

    #region Update & Global Key Presses

    // Update is called once per frame
	void Update ()
    {
        HandleSaveAndLoad();

        HotkeySwapUnits();
        RestartGame();
	}

    private void HotkeySwapUnits()
    {
        for (int i = 0; i < management.UnitMgr.Units.Count; i++)
        {
            int unitNr = i + 1;

            if (Input.GetKeyUp(GetHotKey(unitNr)))
            {
                
                BasicUnit unit = management.UnitMgr.Units[i];

                //Debug.Log(GetHotKey(unitNr) + " unit " + unit.team + " me " + management.UnitMgr.team + " count - " + management.UnitMgr.Units.Count);
                if(management.UnitMgr.team == unit.TeamID)
                {
                    SelectionManager.SelectionChanged(unit);
                }
            }
        }

    }

    private KeyCode GetHotKey(int i)
    {
        switch(i)
        {
            case 0:
                return KeyCode.Alpha0;
            case 1:
                return KeyCode.Alpha1;
            case 2:
                return KeyCode.Alpha2;
            case 3:
                return KeyCode.Alpha3;
            case 4:
                return KeyCode.Alpha4;
            case 5:
                return KeyCode.Alpha5;
            case 6:
                return KeyCode.Alpha6;
            case 7:
                return KeyCode.Alpha7;
            case 8:
                return KeyCode.Alpha8;
            case 9:
                return KeyCode.Alpha9;

        }
        return KeyCode.Alpha0;
    }

    private void RestartGame()
    {

        if (Input.GetKeyUp(KeyCode.F8))
        {
            management.UnitMgr.RespawnAllUnits();
        }
    }

    private void HandleSaveAndLoad()
    {
		//Saving
        if(Input.GetKeyUp(KeyCode.F5))
        {
            BlockManager.SaveLevel();
        }
    }

    #endregion

    #region Clicked on Face

    #region Click Entries (Left & Right)

    public void LefClickOnFace(BlockFace face)
    {
        FacePing(face);
        
        if(GameManagement.Rules.RoomRules.BuildMode)
        {
            HandleBuildLeftClick(face);
        }
        else
        {
            HandleMovementClick(face);
        }
    }

    public void RightClickOnFace(BlockFace face)
    {
        // Remove block
        if (Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftControl))
        {
            BlockManager.Remove(face.Block.ID);
        } // Change color
        else if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            LevelBuildingCommands.ChangeTone(face);
        }
    }

    #endregion

    #region FacePing

    /// <summary>
    /// Spawns an effect on click
    /// </summary>
    private void FacePing(BlockFace face)
    {
        // Reimplement FacePing 
        // (GameObject.FindGameObjectWithTag("manager").GetComponent<Face_Ping>()).ping(this.transform);
    }

    #endregion

    #region HandleMovement

    private void HandleMovementClick(BlockFace face)
    {
        // No need to move if there is no unit selected
        if (SelectionManager.SelectedUnit == null)
            return;

        GameManagement.Rules.MovementRules.MovementClick(face, SelectionManager.SelectedUnit);
    }

    #endregion

    #region Handle Build Clicks (Left & Right

    private void HandleBuildLeftClick(BlockFace face)
    {
        // New block
        if (Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftControl))
        {
            LevelBuildingCommands.AddBlock(face);
        }// Create Unit
        else if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
        {
            UnitManager.Create(face.Block.ID, face.ID, 1);
        }// Set Texture ID and loop over versions
        else if (Input.GetKey(KeyCode.Q))
        {
            LevelBuildingCommands.ChangeBlockToNormal(face);
        } // Set Unit Spawn
        else if (Input.GetKey(KeyCode.W))
        {
            LevelBuildingCommands.ChangeBlockToUnitSpawn(face);
        }// Set Team Spawn 
        else if (Input.GetKey(KeyCode.E))
        {
            LevelBuildingCommands.ChangeBlockToStartSpawn(face);
        } //Set Spawn face
        else if (Input.GetKey(KeyCode.R))
        {
            LevelBuildingCommands.ChangeSpawnFace(face);
        }
    }

    #endregion

    #endregion
}
