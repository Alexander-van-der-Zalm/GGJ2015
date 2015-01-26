using UnityEngine;
using System.Collections;

public class SelectionManager : Singleton<SelectionManager>
{
    public BasicUnit SelectedUnit;

    // Handle Mouse Clicks Logic


    // Hotkeys

    internal static void SelectionChanged(BasicUnit basicUnit)
    {
        if (Instance.SelectedUnit != null)
            Instance.SelectedUnit.MaterialColor = Instance.SelectedUnit.NormalTint;

        Instance.SelectedUnit = basicUnit;

        (Camera.main.GetComponent<Camera_Rotation>()).rotateToUnit(Instance.SelectedUnit.transform);

        Instance.SelectedUnit.MaterialColor = Instance.SelectedUnit.SelectedTint;
    }
}
