using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
        Block newBlock = CreateBlock();

        // Project to find length
        float blockScale = Vector3.Dot(clickedFace.Normal,clickedFace.transform.localPosition)*2;
        // Translate length * normal
        newBlock.transform.position = parentBlock.transform.position + clickedFace.Normal * blockScale;
    }

    private static Block CreateBlock()
    {
        // Create new block
        GameObject blockGO = GameObject.Instantiate(Instance.BlockPrefab) as GameObject;
        Block block = blockGO.GetComponent<Block>();
        Register(block);

        return block;
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
        BlockListSO blockData = (BlockListSO)AssetDatabase.LoadAssetAtPath(pathName, typeof(BlockListSO));

        // Read asset
        Instance.LoadBlocks(blockData);
    }

    public void LoadBlocks(BlockListSO blockData)
    {
        int count=  Blocks.Count;
        // Delete oldblocks
        for (int i = 0; i < count; i++)
        {
            GameObject.Destroy(Blocks[count - i - 1].gameObject);
        }

        // Create new blocks list
        Blocks = new List<Block>();

        // Create and change blocks
        for (int i = 0; i < blockData.Blocks.Count; i++)
        {
            Block block = CreateBlock();
            BlockData data = blockData.Blocks[i];
            block.transform.position = data.Position;
            block.transform.localRotation = data.Rot;
            block.transform.localScale = data.Scale;
            block.Type = data.Type;
        }
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
