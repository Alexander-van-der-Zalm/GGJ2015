using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class UnitManger : Singleton<UnitManger> 
{
    public List<GameObject> UnitPrefabs;
    public List<BasicUnit> Units;

    public static BasicUnit Get(int ID)
    {
        return Instance.Units.First(b => b.ID == ID);
    }

    public static void Register(BasicUnit unit)
    {
        Instance.Units.Add(unit);

        Instance.RedoIDs();
    }

    public static void UnRegister(BasicUnit unit)
    {
        Instance.Units.Remove(unit);

        Instance.RedoIDs();
    }

    public void RedoIDs()
    {
        // Redo ID's for all
        for (int i = 0; i < Instance.Units.Count; i++)
        {
            Instance.Units[i].ID = i;
        }
    }
}
