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
    #region Fields

    public GameObject BlockPrefab;
    public List<Block> Blocks;
    

    #endregion

    #region  Get, register, id
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

    #endregion

    #region Add & Remove

    public static void Add(int blockID, int clickedFacedID)
    {
        Block parentBlock = BlockManager.Get(blockID);
        BlockFace clickedFace = parentBlock.GetFace(clickedFacedID);
        
        // Create new block
        GameObject blockGO =  GameObject.Instantiate(Instance.BlockPrefab) as GameObject;
        Block newBlock = blockGO.GetComponent<Block>();

        // Project to find length
        float blockScale = Vector3.Dot(clickedFace.Normal,clickedFace.transform.localPosition)*2;
        // Translate length * normal
        blockGO.transform.position = parentBlock.transform.position + clickedFace.Normal * blockScale;
    }

    public static void Remove(int blockID)
    {
        Debug.Log("Remove");
        Block block = BlockManager.Get(blockID);
        UnRegister(block);
        GameObject.Destroy(block.gameObject);
    }

    #endregion

    #region Load and Save

    public static void LoadLevel(string pathName)
    {
        // Get the asset

        // Create blocks
        // Read asset
    }

    public static void SaveLevel()
    {
        Instance.SaveBlocks();
    }

    public void SaveBlocks()
    {
        BlockListSO list = BlockListSO.Create();
        list.Blocks = new List<BlockData>();
        Debug.Log("Save");
        for(int i = 0; i < Blocks.Count; i++)
        {
            list.Blocks.Add(new BlockData() 
            { 
                Position = Blocks[i].transform.position,
                Scale = Blocks[i].transform.localScale, 
                Rot = Blocks[i].transform.localRotation, 
                Type = Blocks[i].Type 
            });
        }

        ScriptableObjectHelper.SaveAssetAutoNaming(list, "Assets/Levels");
    }

    #endregion
}
