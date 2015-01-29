using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour 
{
    private GameManagement management;

    void Awake()
    {
        management = GetComponent<GameManagement>();
    }


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
                if(management.UnitMgr.team == unit.Team)
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
}
