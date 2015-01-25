using UnityEngine;
using System.Collections;

// Face Info?
// Has Unit
// Captured
// CapturedState?

[ExecuteInEditMode,System.Serializable]
public class BlockFace : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private int id;

    [SerializeField]
    private Block block;

	public int TextureId = 0;

    public bool HasUnit;

	public Colorpallet colPal;

    private Transform tr;

    #endregion

    #region Properties

    public int ID { get { return id; } set { id = value; } }

    public Block Block { get { return block; } set { block = value; } }

	void Start(){
		colPal = this.GetComponent<Colorpallet> ();
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
                UnitManager.Create(Block.ID, ID);
            }
			else if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)){
				this.ChangeFace(TextureId++);
			}
            else
            {
                //SelectionManager Stuff
                if (Selectionmanager.Instance.SelectedUnit != null)
                {
                    // Reimplement FacePing 
                    (GameObject.FindGameObjectWithTag("manager").GetComponent<Face_Ping>()).ping(this.transform);

					if(!Selectionmanager.Instance.SelectedUnit.capping){
						UnitManager.LocalMoveOrder(new UnitManager.FaceBlockID() { FaceID = ID, BlockID = Block.ID }, Selectionmanager.Instance.SelectedUnit.ID, new UnitManager.FaceBlockID());

					}
					if(block.team != Selectionmanager.Instance.SelectedUnit.team && !Selectionmanager.Instance.SelectedUnit.capping){
						block.StartCapture(Selectionmanager.Instance.SelectedUnit);
						Selectionmanager.Instance.SelectedUnit.capping = true;
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
				this.block.ChangeBlock(TextureId++);
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
			gameObject.renderer.material.color = Color.Lerp (colPal.neutralCol, colPal.teamOneCol, slerpCount);
		} else if (to == 2 && from == 1) {
			gameObject.renderer.material.color = Color.Lerp (colPal.neutralCol, colPal.teamTwoCol, slerpCount);
		} else if (to == 1 && from == 2) {
			gameObject.renderer.material.color = Color.Lerp (colPal.teamTwoCol, colPal.neutralCol, slerpCount);
		} else if (to == 1 && from == 0) {
			gameObject.renderer.material.color = Color.Lerp (colPal.teamOneCol, colPal.neutralCol, slerpCount);
		}
	}

	public void setBaseColor(){
		gameObject.renderer.material.color = colPal.neutralCol;
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
