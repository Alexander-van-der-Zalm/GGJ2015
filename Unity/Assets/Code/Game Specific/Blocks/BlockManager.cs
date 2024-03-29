﻿using UnityEngine;
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

[System.Serializable]
public class BlockManager : Singleton<BlockManager>
{
    #region Fields

    public ColorPalette Pallet;

    public GameObject BlockPrefab;

    public bool LoadLevelOnReload = false;

    [SerializeField]
    private string selectedLevel;
    
    [HideInInspector]
    public List<Block> Blocks;

    [HideInInspector]
    public List<string> LevelNames;

    [HideInInspector]
    public GameObject levelParent;

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

    //public bool DrawGizmos = false;

    #endregion

    public void Awake()
    {
        Blocks = new List<Block>();

        // Set an empty game object to parent all the blocks too
        levelParent = GameObject.Find("Level");
        if (levelParent == null)
        {
            levelParent = new GameObject("Level");
        }
    }

    #region Enable

    public void OnEnable()
    {
        if (LoadLevelOnReload && !EditorApplication.isPlaying)
        {
            //LevelNames = FindLevelNames();
            Debug.Log("Enable " + SelectedLevel);
            BlockManager.LoadLevel(SelectedLevel);
        }
    }

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

        Debug.Log("Get " + parentBlock.ID);

        // Create new block
        Block newBlock = Instance.CreateBlock();

        // Set palette
        //ColorPallet plt = Instance.GetComponent<ColorPallet>();
        newBlock.Init(0);
        
        // Project to find length
        float blockScale = Vector3.Dot(clickedFace.Normal,clickedFace.transform.localPosition)*2;

        newBlock.transform.position = parentBlock.transform.position + clickedFace.Normal * blockScale;
        Instance.GridSnap(newBlock.transform);

        //// Translate length * normal
        //Vector3 roundMe = parentBlock.transform.position + clickedFace.Normal * blockScale;
        ////newBlock.transform.position = parentBlock.transform.position + clickedFace.Normal * blockScale;
        ////Vector3 roundMe = 
        //roundMe *=2;

        //Vector3 rounded = new Vector3(Mathf.Round(roundMe.x), Mathf.Round(roundMe.y), Mathf.Round(roundMe.z));
        //// Round the numbers
        //newBlock.transform.position = rounded*0.5f;

        // Redo ID's
        Instance.RedoID();

        // Redo Neighbors for faces and blocks
        List<BlockFace> neighborFaces = Block.FindBlockNeighborFaces(Instance.Blocks, newBlock.ID);
        //Debug.Log(neighborFaces.Count);
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

    private Block CreateBlock()
    {
        // Create new block
        GameObject blockGO = GameObject.Instantiate(Instance.BlockPrefab) as GameObject;
        Block block = blockGO.GetComponent<Block>();
        block.transform.parent = levelParent.transform;

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
        // Check if level is in the asset file
        if(!LevelNames.Contains(levelName))
        {
            Debug.Log("Level does not exist: " + levelName);
            return;
        }
        Debug.Log("Load: " + levelName);
        Instance.SelectedLevel = levelName;

        // Delete old units
        UnitManager.Instance.DeleteAll();

        // Get the asset
        BlockListSO blockData = GetLevelData(levelName);

        // Read asset
        LoadBlocks(blockData);

        //// Respawn Units
        //UnitManager.Instance.RespawnAllUnits();
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

        ColorPalette plt = Pallet;

        List<BlockFace> faces = new List<BlockFace>();

        // Create and change blocks
        for (int i = 0; i < blockData.Blocks.Count; i++)
        {
            Block block = CreateBlock();
            BlockData data = blockData.Blocks[i];

            // Load Data
            block.transform.position = data.Position;
            block.transform.localRotation = data.Rot;
            block.transform.localScale = data.Scale;
            block.Type = data.Type;
			block.SpawnFaceID = data.SpawnFaceID;
            block.ColorID = data.ColorTypeID;
            block.TeamID = data.Team;

            // Set up in scene
            block.name = "Block " + i;
            block.ID = i;
            block.transform.parent = levelParent.transform;

            // Set color
            block.Init(data.Team);

            // Grid snap positions
            GridSnap(block.transform);

           // Add ranges for neighbors
            faces.AddRange(block.Faces);
        }

        Block.SetFaceNeighbors(faces);
    }

    private void GridSnap(Transform toSnap)
    {
        Vector3 roundMe = toSnap.position;
        roundMe *= 2;
        Vector3 rounded = new Vector3(Mathf.Round(roundMe.x), Mathf.Round(roundMe.y), Mathf.Round(roundMe.z));
        // Round the numbers
        toSnap.position = rounded * 0.5f;
    }

    public void ClearBlocks()
    {
        // Delete current children
        var children = new List<GameObject>();
        foreach (Transform child in levelParent.transform)
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
                ColorTypeID = Blocks[i].ColorID,
				SpawnFaceID = Blocks[i].SpawnFaceID,
                Team = Blocks[i].TeamID
            });
        }

        list.name = "Level";

        string path = ScriptableObjectHelper.SaveAssetAutoNaming(list, "Assets/Levels");

        Debug.Log("SaveBlocks " + SelectedLevel);

        Instance.SelectedLevel = path.Remove(0,("Assets/Levels").Count()+1);
    }

    #endregion

    public void SetTeamColor()
    {
        foreach (Transform child in levelParent.transform)
        {
            child.GetComponent<Block>().SetCurrentTeamColor();
        }
    }

    #region GetFace

    public BlockFace getFace(UnitManager.FaceBlockID ids)
    {
        return Get(ids.BlockID).GetComponent<Block>().GetFace(ids.FaceID);
    }

    internal static BlockFace GetFace(UnitManager.FaceBlockID ids)
    {
        return Instance.getFace(ids);
    }

    internal static BlockFace GetFace(int blockID, int faceID)
    {
        return Instance.getFace(new UnitManager.FaceBlockID(){ BlockID = blockID, FaceID = faceID});
    }

    #endregion

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
