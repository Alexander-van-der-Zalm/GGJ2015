using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Todo
// (X) ADD
// (X) Spawn and move
// (X) Register to lists (blocks & neighbours)
// (X) Art sauce it up
// (X) Remove
// Connected blocks?

// Block
// Spawn
// Delete

// BlockManager
// Create n register
// Delete n unregister

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
