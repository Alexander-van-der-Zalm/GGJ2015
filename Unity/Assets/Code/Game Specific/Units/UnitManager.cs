using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class UnitManager : Singleton<UnitManager> 
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

    public static void Create(int blockID,int blockFaceID,int version = 0)
    {
        // Translate and rotate
        Block block = BlockManager.Get(blockID);
        BlockFace face = block.GetFace(blockFaceID);
        Vector3 position = face.transform.position;
        Quaternion rotation = Quaternion.LookRotation(face.Normal, new Vector3(0, 1, 0));
        // Create
        Instance.Create(position, rotation, version);
    }

    public void Create(Vector3 position,Quaternion rotation, int version = 0)
    {
        GameObject newUnit = GameObject.Instantiate(UnitPrefabs[version], position, rotation) as GameObject;
        BasicUnit unit = newUnit.GetComponent<BasicUnit>();
        Register(unit);
    }

    public static void Delete(BasicUnit unit)
    {
        UnRegister(unit);
        GameObject.Destroy(unit.gameObject);
    }
}
