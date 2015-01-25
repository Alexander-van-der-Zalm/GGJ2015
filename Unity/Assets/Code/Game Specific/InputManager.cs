using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class InputManager : MonoBehaviour 
{
	
	// Update is called once per frame
	void Update ()
    {
        HandleSaveAndLoad();
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
