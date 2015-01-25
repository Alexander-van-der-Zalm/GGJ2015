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

	public bool capping = false;

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
            // Select
            Selectionmanager.SelectionChanged(this);
        }// Right mouse button
        else if (Input.GetMouseButtonDown(1))
        {
            // Delete
            UnitManager.Delete(this);
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
        tr.position = CurrentFace.transform.position;
        // Rotate
        tr.rotation = CurrentFace.Rotation;
    }
}
