using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
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

        RestartGame();
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
