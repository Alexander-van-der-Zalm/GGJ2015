using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[ExecuteInEditMode]
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

    public int ColorTypeID { get { return colorTypeID; } set { colorTypeID = value; } }

	public bool occupied = false;

	public int team = 1;

	public int possesion = 0;

	public float possesionTime = 0.2f;

	public int possesionCap = 10;

	public BasicUnit creature;

    #endregion

    #region Properties

    public int ID { get { return id; } set { id = value; } }

    public bool HasUnit { get { return Faces.Where(f => f.HasUnit).Any(); } }

    #endregion

	void Start(){
		if (this.Type == BlockData.BlockType.Unit) {
			this.RespawnUnit();
		}
	}
	
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

	void Update(){
		if (Input.GetKey (KeyCode.P)) {
			this.RespawnUnit ();
		}
	}

	public void ChangeBlock(int index){
		foreach (BlockFace face in Faces) {
			face.ChangeFace(index);
		}
	}

	public void StartCapture(BasicUnit basic){
		StartCoroutine(CaptureTick(basic));
	}

	IEnumerator CaptureTick(BasicUnit basic){
		if (possesion == 0) {
			team = 1;
			if(BlockData.BlockType.Unit == Type){
				this.creature.team = team;
			}
		}
		if (basic.team < team) {
			possesion -= 1;
			if(team == 1){
				ChangeTeam(basic.team, team, (float)Mathf.Abs(possesion)/10);
			}else if(team == 2){
				ChangeTeam(team, 1, (float)Mathf.Abs(possesion)/10);
			}
			if(possesion == -possesionCap){
				team = basic.team;
				if(BlockData.BlockType.Unit == Type){
					this.creature.team = team;
				}
				basic.capping = false;
			}
			yield return new WaitForSeconds(possesionTime);
			StartCoroutine(CaptureTick(basic));
		}
		if (basic.team > team) {
			possesion += 1;
			if(team == 1){
				ChangeTeam(basic.team, team, (float)Mathf.Abs(possesion)/10);
			}else if(team == 0){
				ChangeTeam(team, 1, (float)Mathf.Abs(possesion)/10);
			}
			if (possesion == possesionCap) {
				team = basic.team;
				if(BlockData.BlockType.Unit == Type){
					this.creature.team = team;
				}
				basic.capping = false;
			}
			yield return new WaitForSeconds (possesionTime);
			StartCoroutine (CaptureTick (basic));
		}
	}

	public void ChangeTeam(int from, int to, float slerpCount){
		foreach (BlockFace face in Faces) {
			face.ChangeTeam (from, to, slerpCount);
		}
	}

	public void setBaseCol(){
		foreach (BlockFace face in Faces) {
			face.setBaseColor();
		}
	}

	public void setTeamCol(int i){
		foreach (BlockFace face in Faces) {
			face.setTeamCol(i);
		}
	}

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

	public void RespawnUnit(){
		if (Type == BlockData.BlockType.Unit) {
			UnitManager.Create (ID, SpawnFaceID, team);
			creature.team = team;
		}
	}

    #region Neighbor

    public static void SetFaceNeighbors(List<BlockFace> input)
    {
        for (int i = 0; i < input.Count; i++)
        {
            BlockFace current = input[i];
            float minDistance = float.MaxValue;
            current.neighbors = new List<BlockFace>();

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
                    current.neighbors.Add(other);
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
