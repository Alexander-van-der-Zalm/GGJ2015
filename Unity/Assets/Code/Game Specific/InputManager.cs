using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour 
{
	
	// Update is called once per frame
	void Update ()
    {
        HandleSaveAndLoad();
	}

    private void HandleSaveAndLoad()
    {
        if(Input.GetKeyUp(KeyCode.F5))
        {
            BlockManager.SaveLevel();
        }
    }
}
