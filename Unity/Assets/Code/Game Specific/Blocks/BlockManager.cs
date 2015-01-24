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
        // Written while durped
        Instance.Blocks.Add(newBlock);
    }

    public static void Add(int blockID, int clickedFacedID)
    {

    }

    public static void Remove(int blockID)
    {

    }
}
