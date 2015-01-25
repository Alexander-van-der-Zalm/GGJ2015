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

[ExecuteInEditMode,System.Serializable]
public class BlockManager : Singleton<BlockManager>
{
    #region Fields

    public GameObject BlockPrefab;
    //[HideInInspector]
    public List<Block> Blocks;

    public bool LoadLevelOnReload = false;

    [SerializeField]
    private string selectedLevel;

    private string sla = "CurrentLevel";

    public string SelectedLevel 
    { 
        get 
        {
            selectedLevel = PlayerPrefs.GetString(sla); 
            return selectedLevel; 
        } 
        set 
        { 
            selectedLevel = value;
            PlayerPrefs.SetString(sla, value); 
            
            // Set index
            LevelNames = FindLevelNames();
            int index = LevelNames.FindIndex(v => v == value);
            EditorPrefs.SetInt("Level", index);
        } 
    }

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

    public Block get(int ID)
    {
        //Debug.Log(ID);
        return Blocks[ID];
        //for (int i = 0; i < Blocks.Count; i++)
        //    if (Blocks[i].ID == ID)
        //        return Blocks[i];
        //return null;
        
        //return Blocks.First(b => b.ID == ID);
    }

    public static Block Get(int ID)
    {
        return Instance.get(ID);
    }

    public static void Register(Block newBlock)
    {
        if (!Instance.Blocks.Contains(newBlock))
        {
            Instance.Blocks.Add(newBlock);
            Instance.RedoID();
        }
    }

    public static void UnRegister(Block newBlock)
    {
        if(instance == null)
        {
            //Debug.Log("Unregister encountered null");
            return;
        }
        Instance.Blocks.Remove(newBlock);

        Instance.RedoID();
    }

    //public void RedoIDs()
    //{
    //    // Redo ID's for all
    //    for (int i = 0; i < Instance.Blocks.Count; i++)
    //    {
    //        Instance.Blocks[i].ID = i;
    //    }
    //}

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
        Vector3 roundMe = parentBlock.transform.position + clickedFace.Normal * blockScale;
        //newBlock.transform.position = parentBlock.transform.position + clickedFace.Normal * blockScale;
        //Vector3 roundMe = 
        roundMe *=2;

        Vector3 rounded = new Vector3(Mathf.Round(roundMe.x), Mathf.Round(roundMe.y), Mathf.Round(roundMe.z));
        // Round the numbers
        newBlock.transform.position = rounded*0.5f;

        // Redo ID's
        Instance.RedoID();

        // Redo Neighbors for faces and blocks
        List<BlockFace> neighborFaces = Block.FindBlockNeighborFaces(Instance.Blocks, newBlock.ID);
        Debug.Log(neighborFaces.Count);
        Block.SetFaceNeighbors(neighborFaces);
    }

    private void RedoID()
    {
        for (int i = 0; i < Blocks.Count; i++)
        {
            Blocks[i].name = "Block " + i;
            Blocks[i].ID = i;
        }
    }

    private static Block CreateBlock()
    {
        // Create new block
        GameObject blockGO = GameObject.Instantiate(Instance.BlockPrefab) as GameObject;
        Block block = blockGO.GetComponent<Block>();
        block.transform.parent = Instance.transform;

        // Find all neighbor faces


        // Recalculate neighbors
        //block.FindNeighbors()

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

    public void loadLevel(string levelName)
    {
        
        if(!LevelNames.Contains(levelName))
        {
            Debug.Log("Level does not exist: " + levelName);
            return;
        }
        Debug.Log("Load: " + levelName);
        Instance.SelectedLevel = levelName;

        // Get the asset
        BlockListSO blockData = GetLevelData(levelName);

        // Read asset
        LoadBlocks(blockData);
    }

    public static void LoadLevel(string levelName)
    {
        Debug.Log("Load: " + levelName);
        Instance.SelectedLevel = levelName;
        
        // Get the asset
        BlockListSO blockData = Instance.GetLevelData(levelName);

        // Read asset
        Instance.LoadBlocks(blockData);
    }

    public BlockListSO GetLevelData(string levelName)
    {
        return (BlockListSO)AssetDatabase.LoadAssetAtPath("Assets/Levels/" + levelName, typeof(BlockListSO));
    }

    public void LoadBlocks(BlockListSO blockData)
    {
        ClearBlocks();
        Debug.Log("Clear Blocks");
        // Create and change blocks
        for (int i = 0; i < blockData.Blocks.Count; i++)
        {
            Block block = CreateBlock();
            BlockData data = blockData.Blocks[i];
            block.transform.position = data.Position;
            block.transform.localRotation = data.Rot;
            block.transform.localScale = data.Scale;
            block.Type = data.Type;
			block.SpawnFaceID = data.SpawnFaceID;
            block.name = "Block "+i;
            block.ID = i;
            block.ColorTypeID = data.ColorTypeID;
            block.transform.parent = transform;
        }
    }

    public void ClearBlocks()
    {
        // Delete current children
        var children = new List<GameObject>();
        foreach (Transform child in transform)
            children.Add(child.gameObject);
        children.ForEach(child => DestroyImmediate(child));

        // Create new blocks list
        Blocks = new List<Block>();
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
                Type = Blocks[i].Type,
                ColorTypeID = Blocks[i].ColorTypeID,
				SpawnFaceID = Blocks[i].SpawnFaceID
            });
        }

        list.name = "Level";

        string path = ScriptableObjectHelper.SaveAssetAutoNaming(list, "Assets/Levels");

        Debug.Log("SaveBlocks " + SelectedLevel);

        Instance.SelectedLevel = path.Remove(0,("Assets/Levels").Count()+1);
    }

    #endregion

    public BlockFace getFace(UnitManager.FaceBlockID ids)
    {
        return Get(ids.BlockID).GetComponent<Block>().GetFace(ids.FaceID);
    }

    internal static BlockFace GetFace(UnitManager.FaceBlockID ids)
    {
        return Instance.getFace(ids);
    }

    public void OnEnable()
    {
        if (LoadLevelOnReload && !EditorApplication.isPlaying)
        {
            //LevelNames = FindLevelNames();
            Debug.Log("Enable " + SelectedLevel);
            BlockManager.LoadLevel(SelectedLevel);
        }

    }


    //void OnEnable()
    //{
    //#if UNITY_EDITOR
    //        EditorApplication.playmodeStateChanged += StateChange;
    //#endif
    //}

    //#if UNITY_EDITOR
    //void StateChange()
    //{
    //    if (!EditorApplication.isPlayingOrWillChangePlaymode && EditorApplication.isPlaying)
    //    {
    //        BlockManager.LoadLevel(SelectedLevel);
    //    }
    //}
    //#endif

}
