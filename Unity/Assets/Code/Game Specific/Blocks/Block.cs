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

    #endregion

    #region Properties

    public int ID { get { return id; } set { id = value; } }

    public bool HasUnit { get { return Faces.Where(f => f.HasUnit).Any(); } }

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
    }

    public void Remove()
    {
        // UNRegister faces

        // UNSet ID

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
