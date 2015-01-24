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

        // Redo ID's for all
        for (int i = 0; i < Instance.Blocks.Count; i++)
        {
            Instance.Blocks[i].ID = i; i = i;
        }
            //newBlock.ID = Instance.Blocks.Count;
    }

    public static void Add(int blockID, int clickedFacedID)
    {

    }

    public static void Remove(int blockID)
    {

    }
}
