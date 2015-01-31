using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Block : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private int id;
    
    [HideInInspector]
    public List<BlockFace> Faces;

    public BlockData.BlockType Type;

	public int SpawnFaceID;

    public List<Block> Neighbors;
    [SerializeField]
    private int colorTypeID;

    //[HideInInspector]
    public ColorPalette ColorPallete { get { return GameManagement.Block.Pallet; } }

    public int ColorID { get { return colorTypeID; } set { colorTypeID = value; } }

	public bool occupied = false;

    public OwnershipInfo OwnerInfo;

    /// <summary>
    /// Team 0 = player 1, team 1 = neutral, team2 = player 2
    /// </summary>
    public int TeamID { get { return OwnerInfo.TeamID; } set { OwnerInfo.TeamID = value; } }

	public int possesion = 0;

	public float possesionTime = 0.2f;

	public int possesionCap = 10;

	public BasicUnit creature;

    #endregion

    #region Properties

    public int ID { get { return id; } set { id = value; } }

    public bool HasUnit { get { return Faces.Where(f => f.HasUnit).Any(); } }

    #endregion

    #region Start

    internal void Init(int teamID)
    {
        OwnerInfo = new OwnershipInfo();
        TeamID = teamID;
        InitFaces();
        SetCurrentTeamColor();
    }

    #endregion

    #region GetFace

    public BlockFace GetFace(int blockFaceID)
    {
        return Faces.Where(f => f.ID == blockFaceID).First();
    }

    #endregion

    #region Editor Functions

    public void FaceCheck()
    {
        // Register faces
        Faces = GetComponentsInChildren<BlockFace>().ToList();

        // Set ID
        for (int i = 0; i < Faces.Count; i++)
        {
            Faces[i].ID = i;
            Faces[i].gameObject.name = "Face " + i;
            Faces[i].Block = this;
        }

        //// Set Neighbors
        //FindNeighbors();
    }

   

    #endregion

    #region Set Color

    public void SetCurrentTeamColor()
    {
		foreach (BlockFace face in Faces) 
        {
            face.SetTeamColor();
		}
	}

    public void UpdateAllContestedFacesColor()
    {
        foreach (BlockFace face in Faces)
        {
            face.ChangeContestedTeamColor();
        }
    }

    private void InitFaces()
    {
        foreach (BlockFace face in Faces)
        {
            face.Block = this;
            face.OwnerInfo = new OwnershipInfo();
            face.TeamID = TeamID;
        }
    }


    public void SetTeamIDAllFaces(int newTeamID)
    {
        TeamID = newTeamID;
        foreach (BlockFace face in Faces)
        {
            face.OwnerInfo = new OwnershipInfo();
            face.TeamID = newTeamID;
        }
    }
    

    #endregion

    #region Enable & disable

    public void OnEnable()
    {
        //Debug.Log("Enable register");
        // Register to blockmanager
        BlockManager.Register(this);
    }

    public void OnDisable()
    {
        Remove();
        BlockManager.UnRegister(this);
    }

    public void Remove()
    {
        // UNRegister faces

        // UNSet ID

    }

    #endregion

    public void RespawnUnit()
    {
		if (Type == BlockData.BlockType.UnitSpawn || Type == BlockData.BlockType.StartSpawn) 
        {
			// Remove existing spawn
            if(creature != null)
            {
                Debug.Log("Destroy old");
                GameObject.DestroyImmediate(creature.gameObject);
            }
            
            // Create a new one
            UnitManager.Create (ID, SpawnFaceID, TeamID);
			creature.TeamID = TeamID;
		}
	}

    #region Neighbor

    public static void SetFaceNeighbors(List<BlockFace> input)
    {
        for (int i = 0; i < input.Count; i++)
        {
            BlockFace current = input[i];
            float minDistance = float.MaxValue;
            current.Neighbors = new List<BlockFace>();

            // Find mindistance
            for (int j = 0; j < input.Count; j++)
            {
                if (j == i)
                    continue;
                BlockFace other = input[j];
                float dist = Vector3.Magnitude(current.transform.position - other.transform.position);

                if(dist < minDistance)
                {
                    minDistance = dist;
                }
            }

            // Add if mindistace
            for (int j = 0; j < input.Count; j++)
            {
                if (j == i)
                    continue;
                BlockFace other = input[j];
                float dist = Vector3.Magnitude(current.transform.position - other.transform.position);

                if(dist <= minDistance)
                {
                    current.Neighbors.Add(other);
                }
            }
        }
    }

    public static List<BlockFace> FindBlockNeighborFaces(List<Block> input, int centerBlockIndex)
    {
        List<BlockFace> output = new List<BlockFace>();

        // Setting all the neighbors for all of the input
        for (int i = 0; i < input.Count; i++)
        {
            Block current = input[i];
            float minDistance = float.MaxValue;
            current.Neighbors = new List<Block>();

            // Find mindistance
            for (int j = 0; j < input.Count; j++)
            {
                if (j == i)
                    continue;
                Block other = input[j];
                float dist = Vector3.Magnitude(current.transform.position - other.transform.position);

                if (dist < minDistance)
                    minDistance = dist;
            }

            // Add if mindistace
            for (int j = 0; j < input.Count; j++)
            {
                if (j == i)
                    continue;
                Block other = input[j];
                float dist = Vector3.Magnitude(current.transform.position - other.transform.position);

                if (dist <= minDistance)
                    current.Neighbors.Add(other);
            }
        }

        // For the center block add all the neighbor faces to the output
        Block center = input[centerBlockIndex];
        output.AddRange(center.Faces);

        for (int i = 0; i < center.Neighbors.Count; i++)
        {
            output.AddRange(center.Neighbors[i].Faces);
        }

        return output;
    }

    #endregion

   
}
