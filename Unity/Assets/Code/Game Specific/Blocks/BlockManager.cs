using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
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

[ExecuteInEditMode]
public class BlockManager : Singleton<BlockManager>, ISerializationCallbackReceiver
{
    #region Fields

    public GameObject BlockPrefab;
    //[HideInInspector]
    public List<Block> Blocks;

    //private string selectedLevel

    public string SelectedLevel { get { return PlayerPrefs.GetString("CurrentLevel"); } set { PlayerPrefs.SetString("CurrentLevel", value); } }

    [HideInInspector]
    public List<string> LevelNames;
    

    #endregion

    public List<string> FindLevelNames()
    {
        string basePath = Application.dataPath + "/Levels";
        string[] files = Directory.GetFiles(basePath).Where(s => s.EndsWith(".asset")).ToArray();
        for (int i = 0; i < files.Length; i++ )
        {
            files[i] = files[i].Remove(0, basePath.Count()+1);
        }
        return files.ToList();

        // Filter results
    }

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

    public static void LoadLevel(string levelName)
    {
        Debug.Log("Load: " + levelName);

        //Instance.SelectedLevel = levelName;

        // Get the asset
        BlockListSO blockData = (BlockListSO)AssetDatabase.LoadAssetAtPath("Assets/Levels/"+levelName, typeof(BlockListSO));

        // Read asset
        Instance.LoadBlocks(blockData);
    }

    public void LoadBlocks(BlockListSO blockData)
    {
        int count=  Blocks.Count;

        GameObject parent;
        if (Blocks.Count > 0)
            parent = Blocks[0].transform.parent.gameObject;
        else
            parent = null;

        // Delete oldblocks
        for (int i = 0; i < count; i++)
        {
            GameObject.DestroyImmediate(Blocks[count - i - 1].gameObject);
        }

        // Create a new parent
        if(parent != null)
        {
            GameObject.DestroyImmediate(parent);
        }

        parent = new GameObject();
        parent.name = "LevelBlocks";
            

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
            block.name = "Block "+i;
            block.ID = i;
            block.transform.parent = parent.transform;
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

        list.name = "Level";

        string path = ScriptableObjectHelper.SaveAssetAutoNaming(list, "Assets/Levels");

        Instance.SelectedLevel = path.Remove(0,("Assets/Levels").Count()+1);
    }

    #endregion

    public void OnAfterDeserialize()
    {
        Debug.Log("Serialize");
        LevelNames = FindLevelNames();
        BlockManager.LoadLevel(SelectedLevel);
    }

    public void OnBeforeSerialize()
    {
        //BlockManager.SaveLevel();
    }
}
