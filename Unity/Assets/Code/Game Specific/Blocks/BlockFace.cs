﻿using UnityEngine;
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

    public int TextureId = 0;

    public bool HasUnit;

    [HideInInspector]
    public List<BlockFace> Neighbors;

    [HideInInspector]
	public ColorPallet ColorPallet;

    public OwnershipInfo OwnerInfo;

    [SerializeField]
    private int id;

    [SerializeField, HideInInspector]
    private Block parentBlock;

    private Mesh mesh;

    #endregion

    #region Properties

    public int ID { get { return id; } set { id = value; } }

    public int TeamID { get { return OwnerInfo.TeamID; } set { OwnerInfo.TeamID = value;  } }

    public Block Block { get { return parentBlock; } set { parentBlock = value; } }

	void Awake()
    {
		//colPal = GetComponent<Colorpallet> ();
        mesh = GetComponent<MeshFilter>().mesh;
        parentBlock = transform.parent.GetComponent<Block>();
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
            GameManagement.Input.LefClickOnFace(this);
        }// Right mouse button
        else if(Input.GetMouseButtonDown(1))
        {
            GameManagement.Input.RightClickOnFace(this);
        }
    }

    #endregion

	#region Changing colors

    public void ChangeTeamColor()
    {
        int ct = OwnerInfo.ContestantTeamID;
        int own = OwnerInfo.TeamID;
        float t = OwnerInfo.Progress;

        // No Need to change colors if not changed
        if(ct == own)
            return;

        // If now neutral
        if (own == 0)
        {
            SetColor(Color.Lerp(ColorPallet.NeutralColor[TextureId], ColorPallet.PlayerTeamColors[ct], t));
        } // or contetant is neutral
        else if (OwnerInfo.ContestantTeamID == 0)
            SetColor(Color.Lerp(ColorPallet.PlayerTeamColors[own], ColorPallet.NeutralColor[TextureId], t));
        else // both players
            SetColor(Color.Lerp(ColorPallet.PlayerTeamColors[own], ColorPallet.PlayerTeamColors[ct], t));
    }

    public void ChangeTeamColor(int to, int from, float slerpCount)
    {
		if (to == 0 && from == 1)
            SetColor(Color.Lerp(ColorPallet.NeutralColor[parentBlock.ColorTypeID], ColorPallet.TeamOneColor, slerpCount));
		 else if (to == 2 && from == 1)
            SetColor(Color.Lerp(ColorPallet.NeutralColor[parentBlock.ColorTypeID], ColorPallet.TeamTwoColor, slerpCount));
		else if (to == 1 && from == 2)
            SetColor(Color.Lerp(ColorPallet.TeamTwoColor, ColorPallet.NeutralColor[parentBlock.ColorTypeID], slerpCount));
		else if (to == 1 && from == 0)
            SetColor(Color.Lerp(ColorPallet.TeamOneColor, ColorPallet.NeutralColor[parentBlock.ColorTypeID], slerpCount));
	}

	public void setBaseColor()
    {
        SetColor(ColorPallet.NeutralColor[parentBlock.ColorTypeID]);
	}

    private void SetColor(Color newColor)
    {
        // Sets all the colors of the vertices
        SetVertexColor(newColor);
    }

    public void SetVertexColor(Color color)
    {
        if(mesh == null)
        {
            #if UNITY_EDITOR
                //Only do this in the editor
                MeshFilter mf = GetComponent<MeshFilter>();   //a better way of getting the meshfilter using Generics
                Mesh meshCopy = Mesh.Instantiate(mf.sharedMesh) as Mesh;  //make a deep copy
                mesh = mf.mesh = meshCopy;                    //Assign the copy to the meshes
            #else
                 //do this in play mode
                 mesh = GetComponent<MeshFilter>().mesh;
            #endif
        }

        int count = mesh.vertexCount;
        Color[] newColors = new Color[count];
        for (int i = 0; i < count; i++)
        {
            newColors[i] = color;
        }
        mesh.colors = newColors;
    }

	public void setTeamCol(int i)
    {
        if (i == 0) SetColor(ColorPallet.TeamOneColor);
        if (i == 2) SetColor(ColorPallet.TeamTwoColor);
	}

	#endregion Materials

    #region Gizmos

    //public void OnDrawGizmosSelected ()
    //{
    //    Gizmos.color = new Color(0.1f, 0.8f, 0.1f, 0.3f);

    //    // Rotate towards normal
    //    Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, Rotation, Vector3.one);
    //    Gizmos.matrix = rotationMatrix;

    //    Gizmos.DrawCube(Vector3.zero, new Vector3(.15f,.25f,.15f));
    //}

    //public void OnDrawGizmos()
    //{
    //    Gizmos.color = new Color(0.1f, 0.1f, 0.1f, 0.3f);
    //    Gizmos.DrawSphere(transform.position, .15f);
    //}

    #endregion
}
