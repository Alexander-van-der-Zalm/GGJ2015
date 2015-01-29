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

    public ColorPallet ColorPallete;

    public int ColorTypeID { get { return colorTypeID; } set { colorTypeID = value; } }

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

    void Start()
    {
        //if (this.Type == BlockData.BlockType.Unit || this.Type == BlockData.BlockType.player) 
        //{
        //    this.RespawnUnit();
        //}
	}

    #endregion

    #region OwnerInfo

    

    #endregion

    public BlockFace GetFace(int blockFaceID)
    {
        return Faces.Where(f => f.ID == blockFaceID).First();
    }

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

    public void Remove()
    {
        // UNRegister faces

        // UNSet ID

    }

    //void Update()
    //{
    //    if (Input.GetKey (KeyCode.P)) 
    //    {
    //        this.RespawnUnit ();
    //    }
    //}

    //public void ChangeBlock(int index){
    //    foreach (BlockFace face in Faces) {
    //        face.ChangeFace(index);
    //    }
    //}

	public void StartCapture(BasicUnit basic)
    {
		StartCoroutine(CaptureTick(basic));
	}

	IEnumerator CaptureTick(BasicUnit basic)
    {
		if (possesion == 0) 
        {
			TeamID = 1;
			if(BlockData.BlockType.UnitSpawn == Type)
            {
				this.creature.TeamID = TeamID;
			}
		}
		if (basic.TeamID < TeamID) 
        {
			possesion -= 1;
			if(TeamID == 1)
            {
				ChangeTeam(basic.TeamID, TeamID, (float)Mathf.Abs(possesion)/10);
			}
            else if(TeamID == 2)
            {
				ChangeTeam(TeamID, 1, (float)Mathf.Abs(possesion)/10);
			}
			if(possesion == -possesionCap)
            {
				TeamID = basic.TeamID;
				if(BlockData.BlockType.UnitSpawn == Type)
                {
					this.creature.TeamID = TeamID;
				}
				basic.Capping = false;
			}
			yield return new WaitForSeconds(possesionTime);
			StartCoroutine(CaptureTick(basic));
		}
		if (basic.TeamID > TeamID) 
        {
			possesion += 1;
			if(TeamID == 1)
            {
				ChangeTeam(basic.TeamID, TeamID, (float)Mathf.Abs(possesion)/10);
			}
            else if(TeamID == 0)
            {
				ChangeTeam(TeamID, 1, (float)Mathf.Abs(possesion)/10);
			}
			if (possesion == possesionCap) 
            {
				TeamID = basic.TeamID;
				if(BlockData.BlockType.UnitSpawn == Type)
                {
					this.creature.TeamID = TeamID;
				}
				basic.Capping = false;
			}
			yield return new WaitForSeconds (possesionTime);
			StartCoroutine (CaptureTick (basic));
		}
	}

	public void ChangeTeam(int from, int to, float slerpCount)
    {
		foreach (BlockFace face in Faces) 
        {
			face.ChangeTeamColor (from, to, slerpCount);
		}
	}

    public void setBaseCol()
    {
		foreach (BlockFace face in Faces) 
        {
			face.setBaseColor();
		}
	}

	public void setTeamCol(int i){
		foreach (BlockFace face in Faces) {
			face.setTeamCol(i);
		}
	}

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

    #endregion

    public void RespawnUnit()
    {
		if (Type == BlockData.BlockType.UnitSpawn || Type == BlockData.BlockType.PlayerSpawn) 
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

    internal void SetColor(ColorPallet clrPallet)
    {
        SetPallette(clrPallet);
        
        if(TeamID == 1)
        {
            setBaseCol();
        }
        else
        {
            setTeamCol(TeamID);
        }
    }

    private void SetPallette(ColorPallet clrPallet)
    {
        foreach (BlockFace face in Faces)
        {
            face.ColorPallet = clrPallet;
        }
    }

    private void SetParent()
    {
        foreach (BlockFace face in Faces)
        {
            face.Block = this;
        }
    }

    internal void Init(ColorPallet plt)
    {
        SetParent();
        SetColor(plt);
    }
}
