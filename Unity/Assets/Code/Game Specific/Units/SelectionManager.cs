using UnityEngine;
using System.Collections;

public class SelectionManager : Singleton<SelectionManager>
{
    public static BasicUnit SelectedUnit { get { return Instance.selectedUnit; } set { Instance.selectedUnit = value; } }

    private BasicUnit selectedUnit;

    // Handle Mouse Clicks Logic


    // Hotkeys

    internal static void SelectionChanged(BasicUnit basicUnit)
    {
        BasicUnit oldSelection = SelectedUnit;
        
        // Change previous selected UI

        // Change previous selected Unit visuals
        //if (Instance.SelectedUnit != null)
        //    Instance.SelectedUnit.MaterialColor = Instance.SelectedUnit.NormalTint;
        if (oldSelection != null)
        {
            oldSelection.Selectable(true);
        }
       
        

        // Set new unit
        SelectedUnit = basicUnit;
        basicUnit.Selectable(false);

        // Change camera perspective
        (Camera.main.GetComponent<Camera_Rotation>()).rotateToUnit(SelectedUnit.transform);

        // Change UI
        
        // Change Unit visuals
        //Instance.SelectedUnit.MaterialColor = Instance.SelectedUnit.SelectedTint;
    }
}
