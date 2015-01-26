using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class UnitManager : Singleton<UnitManager>
{
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
        Debug.Log(management.gameObject.name);
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
        unit.team = unitTeam;
        unit.CurrentFace = face;
        unit.transform.parent = unitParent.transform;

        block.creature = unit;
        Register(unit);

        //Create(position, rotation, version, block);
    }

    public static void Delete(BasicUnit unit)
    {
        UnRegister(unit);
        GameObject.Destroy(unit.gameObject);
    }

    #endregion

    #region RPC Move section

    public static void LocalMoveOrder(FaceBlockID destination, int unitID, FaceBlockID origin)
    {
        if (PhotonNetwork.offlineMode)
        {
            Instance.MoveUnit(destination.FaceID, destination.BlockID, unitID, origin.FaceID, origin.BlockID);
        }
        else
        {
            // Do the RPC call
            (Instance.GetComponent<PhotonView>()).RPC("MoveUnit", PhotonTargets.All, destination.FaceID, destination.BlockID, unitID, origin.FaceID, origin.BlockID);
        }
    }

    [RPC]
    public void MoveUnit(int destFaceID, int destBlockID, int unitID, int originFaceID, int originBlockID)
    {
        FaceBlockID destination = new FaceBlockID() { FaceID = destFaceID, BlockID = destBlockID };
        FaceBlockID origin = new FaceBlockID() { FaceID = originFaceID, BlockID = originBlockID };

        BasicUnit unit = get(unitID);

        // Check if the move is legal
        BlockFace dest = bm.getFace(destination);
        BlockFace orig = bm.getFace(origin);
        Debug.Log("Move");

        if(!dest.neighbors.Where(n => n == orig).Any())
        {
            Debug.Log("Illegal Move");
            return;
        }

        unit.MoveUnit(destination.BlockID, destination.FaceID);

		ColorBlock (destination.BlockID, destination.FaceID);
    }

	public void ColorBlock(int blockID, int blockFaceID){

		Block block = bm.get(blockID);
		BlockFace face = block.GetFace(blockFaceID);

		if(block.team != SelectionManager.Instance.SelectedUnit.team && !SelectionManager.Instance.SelectedUnit.capping){
			block.StartCapture(SelectionManager.Instance.SelectedUnit);
			SelectionManager.Instance.SelectedUnit.capping = true;
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

    [System.Serializable]
    public struct FaceBlockID
    {
        public int FaceID;
        public int BlockID;
    }
}
