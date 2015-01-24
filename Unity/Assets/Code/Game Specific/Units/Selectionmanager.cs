using UnityEngine;
using System.Collections;

public class Selectionmanager : Singleton<Selectionmanager>  
{
    public BasicUnit SelectedUnit;
    
    // Handle Mouse Clicks Logic
    

    // Hotkeys

    internal static void SelectionChanged(BasicUnit basicUnit)
    {
        if(Instance.SelectedUnit!=null)
            Instance.SelectedUnit.MaterialColor = Instance.SelectedUnit.NormalTint;
        
        //(Camera.main.GetComponent<Camera_Rotation>())
        
        Instance.SelectedUnit = basicUnit;

        Instance.SelectedUnit.MaterialColor = Instance.SelectedUnit.SelectedTint;
    }
}
