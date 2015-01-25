using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class BasicUnit : MonoBehaviour
{
    private BlockFace CurrentFace;
    private Transform tr;

    public Color SelectedTint;
    public Color NormalTint;

	public int team;

	public int CreatureType;

	public bool capping = false;

	public Animator anim;

    [SerializeField]
    private int id;

    public int ID { get { return id; } set { id = value; } }

    public Color MaterialColor 
    { 
        get { return GetComponent<MeshFilter>().mesh.colors.First(); }
        set
        {
            MeshFilter filter = GetComponent<MeshFilter>();
            Color[] colors = filter.mesh.colors;
            for(int i = 0; i < colors.Length;i++)
            {
                colors[i] = value;
            }
            filter.mesh.colors = colors;
        }
    }

    public void OnEnable()
    {

		anim = gameObject.GetComponentInChildren<Animator> ();
        tr = transform;
        UnitManager.Register(this);
    }


    public void OnDestroy()
    {
        UnitManager.UnRegister(this);
    }

    public void OnMouseOver()
    {
        // Left click
        if (Input.GetMouseButtonDown(0))
        {
			if(this.team == UnitManager.Instance.team){
				Debug.Log("This.team");
				Debug.Log(this.team);
				Debug.Log("Unitmanager");
				Debug.Log(UnitManager.Instance.team);
            // Select
            	Selectionmanager.SelectionChanged(this);
			}
        }// Right mouse button
        else if (Input.GetMouseButtonDown(1))
        {
            // Delete
           // UnitManager.Delete(this);
        }

    }

    public void MoveUnit(int blockID,int blockFaceID)
    {
        // Update face state both faces
        // Change face
        if(CurrentFace!=null)
            CurrentFace.HasUnit = false;

        Block bl = BlockManager.Get(blockID);
        CurrentFace = bl.GetFace(blockFaceID);
        CurrentFace.HasUnit = true;

         // Translate (TELEPORT HACK)
        // Change to destination and walk
		transform.LookAt (CurrentFace.transform);

		Vector3 newForward = transform.forward;

		anim.SetBool ("Jump", true);

		transform.LookAt(CurrentFace.Normal);

		this.transform.Rotate (Vector3.right, 90);

		
		// Rotate
	}
	
	void FixedUpdate(){
		anim.SetBool ("Jump", false);

		float step = 0.5f * Time.deltaTime;



		transform.position = Vector3.MoveTowards(gameObject.transform.position, CurrentFace.transform.position, step);
	}

}
