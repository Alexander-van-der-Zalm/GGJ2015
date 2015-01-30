using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class UnitManager : Singleton<UnitManager>
{
    #region Custom Struct

    [System.Serializable]
    public struct FaceBlockID
    {
        public int FaceID;
        public int BlockID;
    }

    #endregion

    #region Fields

    public int team = 1;
    
    public List<GameObject> UnitPrefabs;

    [HideInInspector]
    public List<BasicUnit> Units;

    private GameManagement management;
    private GameObject unitParent;
    private BlockManager bm 
    { 
        get 
        { 
            if(management == null)
                management = FindObjectOfType<GameManagement>();
            return management.BlockMgr;
        } 
    }

    #endregion

    #region Start

    void Awake()
    {
        management = FindObjectOfType<GameManagement>();

        // Set an empty game object to parent all the blocks too
        unitParent = GameObject.Find("Units");
        if (unitParent == null)
        {
            unitParent = new GameObject("Units");
        }
    }

    #endregion

    #region Get, reg & unreg & id's

    public BasicUnit get(int ID)
    {
        //return Units.Where(b => b.ID == ID).First();
        for (int i = 0; i < Units.Count; i++)
            if (Units[i].ID == ID)
                return Units[i];
        return null;
            //return Units.First(b => b.ID == ID);
    }

    public static BasicUnit Get(int ID)
    {
        return Instance.get(ID);
    }

    public static void Register(BasicUnit unit)
    {
        Instance.Units.Add(unit);

        Instance.RedoIDs();
    }

    public static void UnRegister(BasicUnit unit)
    {
        if(instance == null)
        {
            Debug.Log("Unit Manager already inactive");
            return;
        }
        
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

    #endregion

    #region Create n delete

    public static void Create(int blockID,int blockFaceID, int unitTeam,int version = 0)
    {
        if (PhotonNetwork.offlineMode)
        {
            Instance.CreateUnit(blockID, blockFaceID, unitTeam, version);
        }
        else
        {// Do the RPC call
            (Instance.GetComponent<PhotonView>()).RPC("CreateUnit", PhotonTargets.All, blockID, blockFaceID, unitTeam, version);
        }
    }

    [RPC]
	public void CreateUnit(int blockID,int blockFaceID,int unitTeam, int version = 0)
    {
        // Translate and rotate
        Block block = bm.get(blockID);

        BlockFace face = block.GetFace(blockFaceID);
        Vector3 position = face.transform.position;
        Quaternion rotation = Quaternion.LookRotation(face.Normal, new Vector3(0, 1, 0));
        
        // Create
        GameObject newUnit = GameObject.Instantiate(UnitPrefabs[version], position, rotation) as GameObject;
        BasicUnit unit = newUnit.GetComponent<BasicUnit>();
        unit.TeamID = unitTeam;
        unit.CurrentFace = face;
        unit.transform.parent = unitParent.transform;

        
        
        block.creature = unit;
        //Register(unit);

        //Debug.Log("Block: " + block.name + " Face: " + face.name + " Unit on block: " + block.creature.name);

        //Create(position, rotation, version, block);
    }

    public static void Delete(BasicUnit unit)
    {
        UnRegister(unit);
        GameObject.Destroy(unit.gameObject);
    }

    public void DeleteAll()
    {
        // Delete current children
        var children = new List<GameObject>();
        foreach (Transform child in unitParent.transform)
            children.Add(child.gameObject);
        children.ForEach(child => DestroyImmediate(child));
    }

    #endregion

    #region RPC Move section

    

	public void ColorBlock(int blockID, int blockFaceID, BasicUnit unit)
    {
		Block block = bm.get(blockID);
		BlockFace face = block.GetFace(blockFaceID);

        if (block.TeamID != unit.TeamID && !unit.Capping)
        {
            block.StartCapture(unit);
            unit.Capping = true;
		}
	}

    //public void MoveUnit(FaceBlockID destination, int unitID, FaceBlockID origin)
    //{
    //    Debug.Log("WORKING! :D:D:D:D");
    //    BasicUnit unit = GetBlock(unitID);
    //    BlockFace dest = bm.Get(destination);

    //    //Check if legal move
    //    // if(origin!=null)
    //    // BlockFace orig = BlockManager.GetFace(origin);

    //    unit.MoveUnit(destination.BlockID, destination.FaceID);
    //}

    #endregion

    #region Respawn

    public void RespawnAllUnits()
    {
        DeleteAll();

        Debug.Log("Respawn block count: " + bm.Blocks.Count);

        foreach(Block block in bm.Blocks)
        {
            block.RespawnUnit();
        }
    }

    #endregion
}
