using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
// Block Prefab
// 

// Todo
// (X) ADD
// (X) Spawn and move
// (X) Register to lists (blocks & neighbours)
// (X) Art sauce it up
// (X) Remove
// Connected blocks?

// Block
// Spawn
// Delete

// BlockManager
// Create n register
// Delete n unregister

public class BlockManager : Singleton<BlockManager> 
{
    public GameObject BlockPrefab;
    public List<Block> Blocks;



    public static Block Get(int ID)
    {
        return Instance.Blocks.First(b => b.ID == ID);
    }

    public static void Register(Block newBlock)
    {
        Instance.Blocks.Add(newBlock);

        Instance.RedoIDs();
    }

    public static void UnRegister(Block newBlock)
    {
        Instance.Blocks.Remove(newBlock);

        Instance.RedoIDs();
    }

    public void RedoIDs()
    {
        // Redo ID's for all
        for (int i = 0; i < Instance.Blocks.Count; i++)
        {
            Instance.Blocks[i].ID = i;
        }
    }

    public static void Add(int blockID, int clickedFacedID)
    {
        Block parentBlock = BlockManager.Get(blockID);
        BlockFace clickedFace = parentBlock.GetFace(clickedFacedID);
        
        // Create new block
        GameObject blockGO =  GameObject.Instantiate(Instance.BlockPrefab) as GameObject;
        Block newBlock = blockGO.GetComponent<Block>();

        float blockScale = Vector3.Dot(clickedFace.Normal,clickedFace.transform.localPosition)*2;
        // Translate half width * normal?
        blockGO.transform.position = parentBlock.transform.position + clickedFace.Normal * blockScale;
        Debug.Log(clickedFace.Normal + "  pos "+ parentBlock.transform.position);
    }

    public static void Remove(int blockID)
    {
        Block block = BlockManager.Get(blockID);
        UnRegister(block);
        GameObject.Destroy(block.gameObject);
    }
}
