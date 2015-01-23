using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class Block : MonoBehaviour 
{
    [SerializeField]
    private int id;

    public int ID { get { return id; } set { id = value; } }

    [HideInInspector]
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
