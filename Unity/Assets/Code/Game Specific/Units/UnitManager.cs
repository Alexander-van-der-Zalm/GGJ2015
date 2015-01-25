using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class UnitManager : Singleton<UnitManager> 
{
    public List<GameObject> UnitPrefabs;
    public List<BasicUnit> Units;

	public int team = 1;

    public BlockManager bm;

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
        //Instance.CreateUnit(blockID, blockFaceID, version);

        // Do the RPC call
        (Instance.GetComponent<PhotonView>()).RPC("CreateUnit", PhotonTargets.All, blockID, blockFaceID, unitTeam, version);
    }

    [RPC]
	public void CreateUnit(int blockID,int blockFaceID,int unitTeam, int version = 0)
    {
        Debug.Log("Creating unit 1");
        // Translate and rotate
        Block block = bm.get(blockID);

        BlockFace face = block.GetFace(blockFaceID);
        Vector3 position = face.transform.position;
        Quaternion rotation = Quaternion.LookRotation(face.Normal, new Vector3(0, 1, 0));
        
        // Create
        Create(position, rotation, version, block);
    }

	public void Create(Vector3 position,Quaternion rotation, int unitTeam, Block block, int version = 0)
    {
        Debug.Log("Creating unit 2");
        GameObject newUnit = GameObject.Instantiate(UnitPrefabs[version], position, rotation) as GameObject;
        BasicUnit unit = newUnit.GetComponent<BasicUnit>();
		unit.team = unitTeam;
	
		block.creature = unit;
        Register(unit);
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
        // Do the RPC call
        (Instance.GetComponent<PhotonView>()).RPC("MoveUnit", PhotonTargets.All, destination.FaceID, destination.BlockID, unitID, origin.FaceID, origin.BlockID);
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
        if(!dest.neighbors.Where(n => n == orig).Any())
        {
            Debug.Log("Illegal Move");
        }

        unit.MoveUnit(destination.BlockID, destination.FaceID);

		ColorBlock (destination.BlockID, destination.FaceID);
    }

	public void ColorBlock(int blockID, int blockFaceID){

		Block block = bm.get(blockID);
		BlockFace face = block.GetFace(blockFaceID);

		if(block.team != Selectionmanager.Instance.SelectedUnit.team && !Selectionmanager.Instance.SelectedUnit.capping){
			block.StartCapture(Selectionmanager.Instance.SelectedUnit);
			Selectionmanager.Instance.SelectedUnit.capping = true;
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
