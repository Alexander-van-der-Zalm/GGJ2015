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

    public List<Block> Neighbors;
    [SerializeField]
    private int colorTypeID;

    public int ColorTypeID { get { return colorTypeID; } set { colorTypeID = value; } }

	public bool occupied = false;

	public int team = 1;

	public int possesion = 0;

	public float possesionTime = 0.2f;

	public int possesionCap = 10;

    #endregion

    #region Properties

    public int ID { get { return id; } set { id = value; } }

    public bool HasUnit { get { return Faces.Where(f => f.HasUnit).Any(); } }

    #endregion

	void Start(){
		setBaseColor();
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
    }

    public void Remove()
    {
        // UNRegister faces

        // UNSet ID

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

	public void setBaseColor(){
		foreach (BlockFace face in Faces) {
			face.setBaseColor();
		}
	}

    public void OnEnable()
    {
        // Register to blockmanager
        BlockManager.Register(this);
    }

    public void OnDisable()
    {
        Remove();
        BlockManager.UnRegister(this);
    }
}
