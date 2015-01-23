using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class Block : MonoBehaviour 
{
    private int id;

    public int ID { get { return id; } set { id = value; } }

    public List<BlockFace> Faces;

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
