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

    #endregion

    #region Properties

    public int ID { get { return id; } set { id = value; } }

    public bool HasUnit { get { return Faces.Where(f => f.HasUnit).Any(); } }

    #endregion

    public BlockFace GetFace(int blockFaceID)
    {
        return Faces.Where(f => f.ID == blockFaceID).First();
    }

    public void Create()
    {
        // Register faces
        // Set ID
    }

    public void Remove()
    {
        // UNRegister faces
        // UNSet ID
    }


}
