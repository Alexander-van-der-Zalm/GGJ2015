using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class BasicUnit : MonoBehaviour
{
    #region Fields

    public BlockFace CurrentFace;
    private Transform tr;

	public int TeamID;

    //public int CreatureType;

    public ConquestRules.CaptureMethod CaptureState;

    private bool capping = false;

    public bool Capping { get { return capping; } set { capping = value; } }

	public Animator anim;

    [SerializeField]
    private int id;

    //public IEnumerator FaceCapture;
    //public IEnumerator 

    #endregion

    #region Properties

    public int ID { get { return id; } set { id = value; } }

    //public Color MaterialColor
    //{
    //    get { return GetComponent<MeshFilter>().mesh.colors.First(); }
    //    set
    //    {
    //        MeshFilter filter = GetComponent<MeshFilter>();
    //        Color[] colors = filter.mesh.colors;
    //        for (int i = 0; i < colors.Length; i++)
    //        {
    //            colors[i] = value;
    //        }
    //        filter.mesh.colors = colors;
    //    }
    //}

    #endregion

    #region Enable Disable

    public void OnEnable()
    {

		anim = gameObject.GetComponentInChildren<Animator> ();
        tr = transform;
        UnitManager.Register(this);
    }


    public void OnDisable()
    {
        UnitManager.UnRegister(this);
    }

    #endregion

    #region Surface select

    public void OnMouseOver()
    {
        // Left click
        if (Input.GetMouseButtonDown(0))
        {
			if(this.TeamID == UnitManager.Instance.team)
            {
                //Debug.Log("This.team");
                //Debug.Log(this.team);
                //Debug.Log("Unitmanager");
                //Debug.Log(UnitManager.Instance.team);

            	SelectionManager.SelectionChanged(this);
			}
        }// Right mouse button
        else if (Input.GetMouseButtonDown(1))
        {
            // Delete
           // UnitManager.Delete(this);
        }



    }

    #endregion

    #region Move Unit

    public void MoveUnit(int blockID,int blockFaceID)
    {
        // Update face state both faces
        // Change face  
        if(CurrentFace!=null)
            CurrentFace.HasUnit = false; 
       
		Block bl = BlockManager.Get (blockID);
		CurrentFace = bl.GetFace (blockFaceID);
		CurrentFace.HasUnit = true;

        // Change to destination and jump

        if (anim != null)
		    anim.SetBool ("Jump", true); 

		//transform.LookAt(CurrentFace.Normal);

		//this.transform.Rotate (Vector3.right, 90);
	/*
		Vector3 a = CurrentFace.transform.position - transform.position;
		Vector3 b = CurrentFace.Normal;
		float ang = Vector3.Angle (a, b);
		float c = Vector3.Dot (a, b);
		float d = c/Mathf.Cos(ang);

		Vector3 x = CurrentFace.transform.position + b;

		Vector3 final = x*c;
*/
		//Vector3 u = CurrentFace.transform.position - transform.position;
		Vector3 normal = CurrentFace.Normal;
		Vector3 u = (CurrentFace.transform.position - transform.position);
		Vector3 b = u - (Vector3.Dot( u , normal ) / normal.magnitude ) * normal ;
		//transform.rotation = Quaternion.LookRotation (transform.position-final, CurrentFace.Normal);

		transform.rotation = Quaternion.LookRotation (b, CurrentFace.Normal);
		
		// Rotate
	}

    #endregion

    void FixedUpdate()
    {
		if(anim != null)
            anim.SetBool("Jump", false);
        
		float step = 0.5f * Time.deltaTime;


		if (CurrentFace != null) 
        {
			transform.position = Vector3.MoveTowards (gameObject.transform.position, CurrentFace.transform.position, step);
		}
	}

    internal void Selectable(bool selectable)
    {
        BoxCollider collider = GetComponent<BoxCollider>();

        if(selectable)
        {
            collider.enabled = true;
        }
        else
        {
            collider.enabled = false;
        }
    }
}
