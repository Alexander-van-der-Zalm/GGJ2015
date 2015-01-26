using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Face Info?
// Has Unit
// Captured
// CapturedState?

[System.Serializable]
public class BlockFace : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private int id;

    [SerializeField,HideInInspector]
    private Block parentBlock;

    [HideInInspector]
    public List<BlockFace> neighbors;

	public int TextureId = 0;

    public bool HasUnit;

	public Colorpallet colPal;

    private Transform tr;

    #endregion

    #region Properties

    public int ID { get { return id; } set { id = value; } }

    public Block Block { get { return parentBlock; } set { parentBlock = value; } }

	void Start()
    {
		colPal = GetComponent<Colorpallet> ();
		setBaseColor ();
	}

    // Change this to mesh rotation
    public Quaternion Rotation 
    { 
        get 
        {
            return Quaternion.LookRotation(Normal, new Vector3(0,1,0));
        } 
    }

    public Vector3 Normal
    {
       get
       {
            Vector3 center = transform.parent.transform.position;
            Vector3 normalDirection = transform.position - center;
            return normalDirection.normalized;
       }
        
    }

    #endregion

    #region ClickEvent

    public void OnMouseOver()
    {
        // Left click
        if(Input.GetMouseButtonDown(0))
        {
            if (Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftControl))
            {
                // New block
                Debug.Log("Create new block");
                BlockManager.Add(Block.ID, ID);
            }
            else if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
            {
                // Create Unit
                UnitManager.Create(Block.ID, ID, 1);
            }
			else if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.RightAlt))
			{
				// Create Unit
				this.parentBlock.Type = BlockData.BlockType.Normal;
				this.parentBlock.ColorTypeID = 0;
				this.parentBlock.setBaseCol();
			}
			else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.RightAlt))
			{
				// Create Unit

				this.parentBlock.Type = BlockData.BlockType.Unit;
				this.parentBlock.ColorTypeID = 3;
				this.parentBlock.setBaseCol();
			}
			else if (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.RightAlt))
			{
				// Create Unit
				this.parentBlock.Type = BlockData.BlockType.player;
				if(this.parentBlock.team == 0){
					this.parentBlock.team = 2;
					this.parentBlock.setTeamCol(2);
				}else{
					this.parentBlock.team = 0;
					this.parentBlock.setTeamCol(0);
				}
			}
			else if (Input.GetKey(KeyCode.R) || Input.GetKey(KeyCode.RightAlt))
			{
				// Create Unit
				this.parentBlock.SpawnFaceID = this.ID;
			}
			else
			{
				//SelectionManager Stuff
                if (SelectionManager.Instance.SelectedUnit != null)
                {
                    // Reimplement FacePing 
                   // (GameObject.FindGameObjectWithTag("manager").GetComponent<Face_Ping>()).ping(this.transform);

                    BasicUnit unit = SelectionManager.Instance.SelectedUnit;
                    if (!unit.capping)
                    {
						// Pass along the destination, the origin and the unitID
                        UnitManager.LocalMoveOrder(new UnitManager.FaceBlockID() { FaceID = ID, BlockID = Block.ID }, 
                        SelectionManager.Instance.SelectedUnit.ID,
                        new UnitManager.FaceBlockID() 
                        { 
                            FaceID = (unit.CurrentFace != null) ? unit.CurrentFace.ID : ID,
                            BlockID = (unit.CurrentFace != null) ? unit.CurrentFace.Block.ID : Block.ID
                        });
					}

                    //UnitManager.LocalMoveOrder(new UnitManager.FaceBlockID() { FaceID = ID, BlockID = Block.ID }, Selectionmanager.Instance.SelectedUnit.ID, new UnitManager.FaceBlockID());
                    //Selectionmanager.Instance.SelectedUnit.MoveUnit(Block.ID, ID);

                }
					
            }
        }// Right mouse button
        else if(Input.GetMouseButtonDown(1))
        {
            if (Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftControl))
            {
                Debug.Log("Remove block " + Block.ID);
                BlockManager.Remove(Block.ID);
            }
			else if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)){
				if(this.parentBlock.ColorTypeID < colPal.neutralCol.Length-2){
					this.parentBlock.ColorTypeID++;
				}else{
					this.parentBlock.ColorTypeID = 0;
				}

				this.parentBlock.setBaseCol();
			}
        }

    }


    //public void OnMouseDown()
    //{
    //    Debug.Log("CLicked " + ID);

    //    // LevelBuilder stuff
        
        
    //}

    #endregion

	#region Materials

	public void ChangeFace(int index){
		if (index != MaterialManager.Instance.size) {
			this.renderer.material = MaterialManager.Instance.matIndex [index];
		} else {
			this.renderer.material = MaterialManager.Instance.matIndex [0];
			this.TextureId = 0;
		}
	}

	public void ChangeTeam(int to, int from, float slerpCount){
		if (to == 0 && from == 1) {
			gameObject.renderer.material.color = Color.Lerp (colPal.neutralCol[parentBlock.ColorTypeID], colPal.teamOneCol, slerpCount);
		} else if (to == 2 && from == 1) {
			gameObject.renderer.material.color = Color.Lerp (colPal.neutralCol[parentBlock.ColorTypeID], colPal.teamTwoCol, slerpCount);
		} else if (to == 1 && from == 2) {
			gameObject.renderer.material.color = Color.Lerp (colPal.teamTwoCol, colPal.neutralCol[parentBlock.ColorTypeID], slerpCount);
		} else if (to == 1 && from == 0) {
			gameObject.renderer.material.color = Color.Lerp (colPal.teamOneCol, colPal.neutralCol[parentBlock.ColorTypeID], slerpCount);
		}
	}

	public void setBaseColor()
    {
		gameObject.renderer.material.color = colPal.neutralCol[parentBlock.ColorTypeID];
	}

	public void setTeamCol(int i){
		if(i == 0)gameObject.renderer.material.color = colPal.teamOneCol;
		if(i == 2)gameObject.renderer.material.color = colPal.teamTwoCol;
	}

	#endregion Materials

    #region Gizmos

    public void OnDrawGizmosSelected ()
    {
        Gizmos.color = new Color(0.1f, 0.8f, 0.1f, 0.3f);

        // Rotate towards normal
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, Rotation, Vector3.one);
        Gizmos.matrix = rotationMatrix;

        Gizmos.DrawCube(Vector3.zero, new Vector3(.15f,.25f,.15f));
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.1f, 0.1f, 0.1f, 0.3f);
        Gizmos.DrawSphere(transform.position, .15f);
    }

    #endregion
}
